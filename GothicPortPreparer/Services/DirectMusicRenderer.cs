using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using GothicPortPreparer.Models;

namespace GothicPortPreparer.Services
{
    public class DirectMusicRenderer
    {
        private readonly string _dmrenderPath;

        public DirectMusicRenderer(string dmrenderPath)
        {
            _dmrenderPath = dmrenderPath;
        }

        public async Task<int> RenderMusicAsync(
            BakeOptions options,
            string outputWavDir,
            IProgress<PreparationProgress>? progress,
            CancellationToken cancellationToken)
        {
            Directory.CreateDirectory(outputWavDir);

            // Find music folders
            var musicFolders = new List<string>();
            var baseMusic = Path.Combine(options.GameDirectory, "_work", "DATA", "Music");
            if (!Directory.Exists(baseMusic))
                baseMusic = Path.Combine(options.GameDirectory, "_work", "Data", "Music");

            if (!Directory.Exists(baseMusic))
            {
                progress?.Report(new PreparationProgress(20, "DirectMusic folder (_work/DATA/Music) not found. Skipping music rendering.", "Missing music directory: " + baseMusic));
                return 0;
            }

            if (options.GameType == GameType.Gothic2)
            {
                var nw = Path.Combine(baseMusic, "newworld");
                var addon = Path.Combine(baseMusic, "AddonWorld");
                if (Directory.Exists(nw)) musicFolders.Add(nw);
                if (Directory.Exists(addon)) musicFolders.Add(addon);
            }

            if (musicFolders.Count == 0)
            {
                musicFolders.Add(baseMusic);
            }

            foreach (var folder in musicFolders)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var folderName = Path.GetFileName(folder);
                progress?.Report(new PreparationProgress(15, $"Rendering DirectMusic themes ({folderName})...", $"Running dmrender for: {folder}"));

                var psi = new ProcessStartInfo
                {
                    FileName = _dmrenderPath,
                    Arguments = $"\"{folder}\" \"{outputWavDir}\" --rate {options.MusicSampleRate}",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    WorkingDirectory = outputWavDir
                };

                using var process = new Process { StartInfo = psi, EnableRaisingEvents = true };

                process.OutputDataReceived += (s, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                    {
                        var line = e.Data;
                        if (line.Contains("->"))
                        {
                            var theme = line.Split(new[] { "->" }, StringSplitOptions.None)[0].Trim();
                            progress?.Report(new PreparationProgress(25, $"Rendering theme: {theme}", line));
                        }
                        else
                        {
                            progress?.Report(new PreparationProgress(25, "Rendering music...", line));
                        }
                    }
                };

                process.ErrorDataReceived += (s, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                    {
                        progress?.Report(new PreparationProgress(25, "Rendering music...", e.Data));
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

                if (process.ExitCode != 0 && process.ExitCode != 1)
                {
                    progress?.Report(new PreparationProgress(25, $"dmrender exited with code {process.ExitCode}", $"Warning: dmrender for {folder} returned code {process.ExitCode}"));
                }
            }

            var wavCount = Directory.GetFiles(outputWavDir, "*.wav").Length;
            progress?.Report(new PreparationProgress(30, $"Rendered {wavCount} DirectMusic WAV tracks.", $"Total WAV files in {outputWavDir}: {wavCount}"));
            return wavCount;
        }
    }
}
