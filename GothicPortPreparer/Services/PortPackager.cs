using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GothicPortPreparer.Models;

namespace GothicPortPreparer.Services
{
    public class PortPackager
    {
        private readonly string _runtimeTemplateDir;
        private const string TitleId = "000400000FA3D500";

        public PortPackager(string runtimeTemplateDir)
        {
            _runtimeTemplateDir = runtimeTemplateDir;
        }

        public async Task<string> PackagePortAsync(
            BakeOptions options,
            string bakedDataDir,
            IProgress<PreparationProgress>? progress,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var isG2 = options.GameType == GameType.Gothic2;
            var portFolderName = isG2 ? "Gothic 2 R36S" : "Gothic R36S";
            var rootLauncherName = isG2 ? "Gothic-2-R36S.sh" : "Gothic-R36S.sh";
            var localLauncherName = isG2 ? "Gothic 2.sh" : "Gothic.sh";

            var targetPortDir = Path.Combine(options.OutputDirectory, portFolderName);
            Directory.CreateDirectory(targetPortDir);

            progress?.Report(new PreparationProgress(88, "Structuring R36S port files...", $"Target directory: {targetPortDir}"));

            var pnDir = Path.Combine(targetPortDir, "pocket-native");
            var gameDir = Path.Combine(pnDir, "games", TitleId);
            var sysdataDir = Path.Combine(gameDir, "sysdata");
            var sdmcDir = Path.Combine(gameDir, "data", "user", "sdmc", "3ds");
            var gothicSdDir = Path.Combine(sdmcDir, "gothic3ds");
            var configDir = Path.Combine(pnDir, "config");
            var titlesDir = Path.Combine(pnDir, "titles");

            Directory.CreateDirectory(sysdataDir);
            Directory.CreateDirectory(gothicSdDir);
            Directory.CreateDirectory(configDir);
            Directory.CreateDirectory(titlesDir);

            // 1. Copy runtime template files
            progress?.Report(new PreparationProgress(90, "Copying pocket-native engine and AArch64 libraries...", "Copying runtime template files..."));
            await Task.Run(() =>
            {
                CopyDirectory(Path.Combine(_runtimeTemplateDir, "pocket-native"), pnDir);

                // Ensure games/000400000FA3D500 files
                var srcGame = Path.Combine(_runtimeTemplateDir, "games", TitleId);
                if (Directory.Exists(srcGame))
                {
                    File.Copy(Path.Combine(srcGame, "game.3ds"), Path.Combine(gameDir, "game.3ds"), true);
                    File.Copy(Path.Combine(srcGame, "title.so"), Path.Combine(gameDir, "title.so"), true);
                    var srcFont = Path.Combine(srcGame, "sysdata", "shared_font.bin");
                    if (File.Exists(srcFont))
                    {
                        File.Copy(srcFont, Path.Combine(sysdataDir, "shared_font.bin"), true);
                    }
                }

                // Write 1-byte HLE DSP marker to bypass libctru check
                var dspMarker = Path.Combine(sdmcDir, "dspfirm.cdc");
                File.WriteAllBytes(dspMarker, new byte[] { (byte)'x' });

                // Update profile.toml for Gothic 2 if needed
                var profilePath = Path.Combine(titlesDir, TitleId, "profile.toml");
                if (isG2 && File.Exists(profilePath))
                {
                    var text = File.ReadAllText(profilePath);
                    text = text.Replace("\"Gothic 3DS\"", "\"Gothic 2 3DS\"");
                    File.WriteAllText(profilePath, text, new UTF8Encoding(false));
                }
            }, cancellationToken);

            // 2. Copy baked .pak files
            progress?.Report(new PreparationProgress(93, "Copying generated game packages (.pak)...", "Moving PAK archives to target directory..."));
            await Task.Run(() =>
            {
                var pakFiles = Directory.GetFiles(bakedDataDir, "*.pak");
                foreach (var pak in pakFiles)
                {
                    var destPak = Path.Combine(gothicSdDir, Path.GetFileName(pak));
                    File.Copy(pak, destPak, true);
                    var fi = new FileInfo(destPak);
                    progress?.Report(new PreparationProgress(94, $"Copied: {Path.GetFileName(pak)} ({fi.Length / 1024 / 1024} MB)"));
                }
            }, cancellationToken);

            // 3. Create Unix launchers
            progress?.Report(new PreparationProgress(96, "Generating ArkOS launcher bash scripts...", "Writing launcher scripts..."));

            var unixNewline = "\n";
            // Local launcher inside the folder
            var localSh = "#!/bin/bash" + unixNewline +
                          "exec \"$(dirname \"$0\")/pocket-native/run.sh\" " + TitleId + unixNewline;
            File.WriteAllText(Path.Combine(targetPortDir, localLauncherName), localSh, new UTF8Encoding(false));

            // Root launcher beside the port folder (convenient when copying directly to roms/ports/)
            var rootSh = "#!/bin/bash" + unixNewline +
                         "exec \"$(dirname \"$0\")/" + portFolderName + "/pocket-native/run.sh\" " + TitleId + unixNewline;
            File.WriteAllText(Path.Combine(options.OutputDirectory, rootLauncherName), rootSh, new UTF8Encoding(false));

            progress?.Report(new PreparationProgress(98, "Port packaging completed successfully.", $"Ready port: {targetPortDir}"));
            return targetPortDir;
        }

        private static void CopyDirectory(string sourceDir, string targetDir)
        {
            Directory.CreateDirectory(targetDir);
            foreach (var file in Directory.GetFiles(sourceDir))
            {
                var dest = Path.Combine(targetDir, Path.GetFileName(file));
                File.Copy(file, dest, true);
            }
            foreach (var sub in Directory.GetDirectories(sourceDir))
            {
                var destSub = Path.Combine(targetDir, Path.GetFileName(sub));
                CopyDirectory(sub, destSub);
            }
        }
    }
}
