using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GothicPortPreparer.Models;

namespace GothicPortPreparer.Services
{
    public class G3dsBaker
    {
        private readonly string _bakeExePath;

        public G3dsBaker(string bakeExePath)
        {
            _bakeExePath = bakeExePath;
        }

        public async Task BakeWorldAsync(
            BakeOptions options,
            string musicWavDir,
            string outputDataPakPath,
            string? ffmpegPath,
            IProgress<PreparationProgress>? progress,
            CancellationToken cancellationToken)
        {
            var outputDir = Path.GetDirectoryName(outputDataPakPath);
            if (!string.IsNullOrEmpty(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            var args = new StringBuilder();
            args.Append("world ");
            args.Append($"--game \"{options.GameDirectory}\" ");

            if (options.GameType == GameType.Gothic2)
            {
                args.Append("--zen NEWWORLD.ZEN ");
            }

            args.Append($"-o \"{outputDataPakPath}\" ");

            if (!string.IsNullOrEmpty(musicWavDir) && Directory.Exists(musicWavDir) && Directory.GetFiles(musicWavDir, "*.wav").Length > 0)
            {
                args.Append($"--music-dir \"{musicWavDir}\" ");
                args.Append($"--music-rate {options.MusicSampleRate} ");
            }

            args.Append($"--voice-codec ima --voice-rate {options.VoiceSampleRate} ");
            args.Append($"--max-tex {options.MaxTextureSize} ");
            args.Append("--ao off --lamp-extra 0 ");
            args.Append($"--threads {options.Threads} ");

            if (options.IncludeVideo)
            {
                args.Append($"--video-fps {options.VideoFps} ");
                if (!string.IsNullOrEmpty(ffmpegPath) && File.Exists(ffmpegPath))
                {
                    args.Append($"--ffmpeg \"{ffmpegPath}\" ");
                }
            }
            else
            {
                args.Append("--no-video ");
            }

            progress?.Report(new PreparationProgress(35, "Starting game asset baking (g3ds-bake)...", $"Command: {_bakeExePath} {args}"));

            var psi = new ProcessStartInfo
            {
                FileName = _bakeExePath,
                Arguments = args.ToString(),
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                WorkingDirectory = outputDir
            };

            using var process = new Process { StartInfo = psi, EnableRaisingEvents = true };

            int currentPct = 35;
            process.OutputDataReceived += (s, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    var line = e.Data;
                    if (line.Contains("WORLD.ZEN") || line.Contains("NEWWORLD.ZEN"))
                    {
                        currentPct = 40;
                        progress?.Report(new PreparationProgress(currentPct, "Loading and baking world meshes...", line));
                    }
                    else if (line.Contains("tex/") || line.Contains("textures"))
                    {
                        currentPct = 55;
                        progress?.Report(new PreparationProgress(currentPct, "Converting and compressing textures...", line));
                    }
                    else if (line.Contains("script") || line.Contains("GOTHIC.DAT"))
                    {
                        currentPct = 70;
                        progress?.Report(new PreparationProgress(currentPct, "Processing Daedalus scripts and dialogues...", line));
                    }
                    else if (line.Contains("video") || line.Contains(".bik"))
                    {
                        currentPct = 80;
                        progress?.Report(new PreparationProgress(currentPct, "Encoding video cutscenes...", line));
                    }
                    else if (line.Contains("packed") || line.Contains("entries") || line.Contains(".pak"))
                    {
                        currentPct = 85;
                        progress?.Report(new PreparationProgress(currentPct, "Writing PAK archives...", line));
                    }
                    else
                    {
                        progress?.Report(new PreparationProgress(currentPct, "Baking game assets...", line));
                    }
                }
            };

            process.ErrorDataReceived += (s, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    progress?.Report(new PreparationProgress(currentPct, "g3ds-bake: output...", e.Data));
                }
            };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            using (cancellationToken.Register(() =>
            {
                try { if (!process.HasExited) process.Kill(); } catch { }
            }))
            {
                await Task.Run(() => process.WaitForExit(), cancellationToken);
            }

            if (process.ExitCode != 0)
            {
                throw new InvalidOperationException($"g3ds-bake failed with exit code: {process.ExitCode}. Review the logs above.");
            }

            progress?.Report(new PreparationProgress(88, "World and assets baked successfully.", "data.pak and associated archives created successfully."));
        }
    }
}
