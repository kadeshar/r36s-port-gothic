using System;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;
using GothicPortPreparer.Models;

namespace GothicPortPreparer.Services
{
    public class DependencyManager
    {
        private readonly string _baseDir;
        private readonly string _toolsDir;
        private readonly string _runtimeTemplateDir;
        private readonly string _runtimeTemplateZip;
        private readonly GitHubReleaseService _releaseService;

        // Remote fallback URL for precompiled R36S pocket-native runtime bundle
        public string RemoteRuntimeZipUrl { get; set; } =
            "https://github.com/kadeshar/r36s-port-gothic/releases/download/v1.0.0/RuntimeTemplate.zip";

        public string ToolsDirectory => _toolsDir;
        public string RuntimeTemplateDirectory => _runtimeTemplateDir;
        public string BakeExePath => Path.Combine(_toolsDir, "g3ds-bake.exe");
        public string DmrenderExePath => Path.Combine(_toolsDir, "dmrender.exe");
        public string FfmpegExePath => Path.Combine(_toolsDir, "ffmpeg.exe");

        public DependencyManager(string? baseDir = null)
        {
            _baseDir = baseDir ?? AppDomain.CurrentDomain.BaseDirectory;
            _toolsDir = Path.Combine(_baseDir, "tools");
            _runtimeTemplateDir = Path.Combine(_baseDir, "RuntimeTemplate");
            _runtimeTemplateZip = Path.Combine(_baseDir, "RuntimeTemplate.zip");
            _releaseService = new GitHubReleaseService();

            Directory.CreateDirectory(_toolsDir);
        }

        public bool AreToolsPresent()
        {
            return File.Exists(BakeExePath) && File.Exists(DmrenderExePath);
        }

        public async Task EnsureRuntimeTemplateAsync(IProgress<PreparationProgress>? progress, CancellationToken cancellationToken)
        {
            // 1. Check if directory exists and has pocket-native executable
            var pnPath = Path.Combine(_runtimeTemplateDir, "pocket-native", "pocket-native");
            if (File.Exists(pnPath))
            {
                progress?.Report(new PreparationProgress(0, "R36S runtime template is ready.", "pocket-native template found in RuntimeTemplate directory."));
                return;
            }

            // 2. Check if local RuntimeTemplate.zip exists
            if (File.Exists(_runtimeTemplateZip))
            {
                progress?.Report(new PreparationProgress(0, "Extracting R36S runtime template...", "Extracting RuntimeTemplate.zip..."));
                Directory.CreateDirectory(_runtimeTemplateDir);
                await Task.Run(() =>
                {
                    ZipFile.ExtractToDirectory(_runtimeTemplateZip, _runtimeTemplateDir);
                }, cancellationToken);

                progress?.Report(new PreparationProgress(0, "R36S runtime template extracted successfully."));
                return;
            }

            // 3. Fallback: check C:\AI fast\R36S\gothic\artifacts
            var fallbackPath = @"C:\AI fast\R36S\gothic";
            if (Directory.Exists(fallbackPath))
            {
                progress?.Report(new PreparationProgress(0, "Copying runtime template from local cache...", "Copying from " + fallbackPath));
                await Task.Run(() =>
                {
                    CopyFallbackTemplate(fallbackPath, _runtimeTemplateDir);
                }, cancellationToken);
                return;
            }

            // 4. Download from GitHub if remote URL is accessible
            if (!string.IsNullOrEmpty(RemoteRuntimeZipUrl))
            {
                try
                {
                    progress?.Report(new PreparationProgress(1, "Downloading R36S pocket-native runtime template from GitHub...", $"Downloading from {RemoteRuntimeZipUrl}..."));
                    await _releaseService.DownloadFileWithProgressAsync(RemoteRuntimeZipUrl, _runtimeTemplateZip, progress, "RuntimeTemplate.zip", cancellationToken);

                    if (File.Exists(_runtimeTemplateZip))
                    {
                        Directory.CreateDirectory(_runtimeTemplateDir);
                        await Task.Run(() =>
                        {
                            ZipFile.ExtractToDirectory(_runtimeTemplateZip, _runtimeTemplateDir);
                        }, cancellationToken);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    progress?.Report(new PreparationProgress(1, "Could not download runtime template from GitHub.", ex.Message));
                }
            }

            throw new InvalidOperationException("Could not find or download the R36S runtime template (RuntimeTemplate or RuntimeTemplate.zip).");
        }

        private void CopyFallbackTemplate(string src, string dst)
        {
            var pnTitles = Path.Combine(dst, "pocket-native", "titles");
            var gameSysdata = Path.Combine(dst, "games", "000400000FA3D500", "sysdata");

            Directory.CreateDirectory(pnTitles);
            Directory.CreateDirectory(gameSysdata);

            File.Copy(Path.Combine(src, @"artifacts\pocket-native"), Path.Combine(dst, @"pocket-native\pocket-native"), true);
            File.Copy(Path.Combine(src, @"artifacts\title.so"), Path.Combine(dst, @"games\000400000FA3D500\title.so"), true);
            File.Copy(Path.Combine(src, @"artifacts\game.3ds"), Path.Combine(dst, @"games\000400000FA3D500\game.3ds"), true);
            File.Copy(Path.Combine(src, @"artifacts\shared_font.bin"), Path.Combine(gameSysdata, "shared_font.bin"), true);

            var scriptsDir = Path.Combine(src, @"sources\pocket-native\scripts\device");
            if (Directory.Exists(scriptsDir))
            {
                File.Copy(Path.Combine(scriptsDir, "run.sh"), Path.Combine(dst, @"pocket-native\run.sh"), true);
                File.Copy(Path.Combine(scriptsDir, "power.sh"), Path.Combine(dst, @"pocket-native\power.sh"), true);
                File.Copy(Path.Combine(scriptsDir, "3ds.sh"), Path.Combine(dst, @"pocket-native\3ds.sh"), true);
            }

            var sourcesDir = Path.Combine(src, @"sources\pocket-native");
            if (Directory.Exists(sourcesDir))
            {
                File.Copy(Path.Combine(sourcesDir, "LICENSE"), Path.Combine(dst, @"pocket-native\LICENSE"), true);
                File.Copy(Path.Combine(sourcesDir, "CREDITS.md"), Path.Combine(dst, @"pocket-native\CREDITS.md"), true);
            }

            var titlesSrc = Path.Combine(src, @"sources\pocket-native\titles");
            if (Directory.Exists(titlesSrc))
            {
                foreach (var dir in Directory.GetDirectories(titlesSrc))
                {
                    var targetSub = Path.Combine(pnTitles, Path.GetFileName(dir));
                    Directory.CreateDirectory(targetSub);
                    foreach (var f in Directory.GetFiles(dir))
                    {
                        File.Copy(f, Path.Combine(targetSub, Path.GetFileName(f)), true);
                    }
                }
            }

            var profilePath = Path.Combine(pnTitles, "000400000FA3D500");
            Directory.CreateDirectory(profilePath);
            File.Copy(Path.Combine(src, @"config\gothic-profile.toml"), Path.Combine(profilePath, "profile.toml"), true);
        }

        public async Task EnsureToolsAsync(bool checkOnlineUpdates, IProgress<PreparationProgress>? progress, CancellationToken cancellationToken)
        {
            if (!checkOnlineUpdates && AreToolsPresent())
            {
                progress?.Report(new PreparationProgress(10, "Local tools are ready.", "Using local g3ds-bake and dmrender executables."));
                return;
            }

            progress?.Report(new PreparationProgress(2, "Checking newest GothicN3DS release on GitHub...", "Checking api.github.com/repos/ponuno/GothicN3DS/releases/latest..."));
            var release = await _releaseService.FetchLatestReleaseAsync(cancellationToken);

            if (release != null && release.Assets.Count > 0)
            {
                progress?.Report(new PreparationProgress(4, $"Found latest GothicN3DS release: {release.TagName}", $"Release: {release.Name} ({release.TagName})"));

                foreach (var asset in release.Assets)
                {
                    if (asset.Name.Equals("g3ds-bake.exe", StringComparison.OrdinalIgnoreCase))
                    {
                        var target = BakeExePath;
                        if (!File.Exists(target) || new FileInfo(target).Length != asset.Size)
                        {
                            progress?.Report(new PreparationProgress(5, "Downloading latest g3ds-bake.exe...", $"Downloading {asset.Name} ({asset.Size / 1024 / 1024} MB)..."));
                            await _releaseService.DownloadFileWithProgressAsync(asset.BrowserDownloadUrl, target, progress, "g3ds-bake.exe", cancellationToken);
                        }
                    }
                    else if (asset.Name.Equals("dmrender.exe", StringComparison.OrdinalIgnoreCase))
                    {
                        var target = DmrenderExePath;
                        if (!File.Exists(target) || new FileInfo(target).Length != asset.Size)
                        {
                            progress?.Report(new PreparationProgress(8, "Downloading latest dmrender.exe...", $"Downloading {asset.Name} ({asset.Size / 1024} KB)..."));
                            await _releaseService.DownloadFileWithProgressAsync(asset.BrowserDownloadUrl, target, progress, "dmrender.exe", cancellationToken);
                        }
                    }
                }
            }
            else
            {
                if (AreToolsPresent())
                {
                    progress?.Report(new PreparationProgress(10, "Using local tools (offline mode).", "Skipping online update; local g3ds-bake.exe and dmrender.exe are present."));
                }
                else
                {
                    throw new InvalidOperationException("Failed to download GothicN3DS tools and local tools are missing.");
                }
            }
        }
    }
}
