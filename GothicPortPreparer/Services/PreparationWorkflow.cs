using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using GothicPortPreparer.Models;

namespace GothicPortPreparer.Services
{
    public class PreparationWorkflow
    {
        private readonly DependencyManager _depManager;

        public PreparationWorkflow(string? baseDir = null)
        {
            _depManager = new DependencyManager(baseDir);
        }

        public async Task<string> RunPreparationAsync(
            BakeOptions options,
            IProgress<PreparationProgress>? progress,
            CancellationToken cancellationToken)
        {
            var stopwatch = Stopwatch.StartNew();
            progress?.Report(new PreparationProgress(0, "Initializing port preparation...", "=== Started Gothic Port Preparation for R36S console ==="));

            // 1. Validate inputs
            var validation = GameDetector.ValidatePath(options.GameDirectory);
            if (!validation.IsValid)
            {
                throw new ArgumentException($"Game path error: {validation.Message}");
            }

            if (string.IsNullOrWhiteSpace(options.OutputDirectory))
            {
                throw new ArgumentException("Output directory cannot be empty.");
            }

            Directory.CreateDirectory(options.OutputDirectory);

            // 2. Ensure Runtime Template
            progress?.Report(new PreparationProgress(2, "Step 1/5: Checking R36S runtime template..."));
            await _depManager.EnsureRuntimeTemplateAsync(progress, cancellationToken);

            // 3. Ensure Tools (download newest release if requested)
            progress?.Report(new PreparationProgress(5, "Step 1/5: Checking and downloading latest GothicN3DS tools..."));
            await _depManager.EnsureToolsAsync(options.CheckForUpdates, progress, cancellationToken);

            // 4. Temporary working directory
            var tempWorkDir = Path.Combine(Path.GetTempPath(), "GothicPortPreparer_" + Guid.NewGuid().ToString("N"));
            var tempMusicDir = Path.Combine(tempWorkDir, "music_wavs");
            var tempPakDir = Path.Combine(tempWorkDir, "paks");
            Directory.CreateDirectory(tempMusicDir);
            Directory.CreateDirectory(tempPakDir);

            string finalPortDir;
            try
            {
                // 5. Render DirectMusic themes
                progress?.Report(new PreparationProgress(15, "Step 2/5: Converting DirectMusic themes to WAV..."));
                var renderer = new DirectMusicRenderer(_depManager.DmrenderExePath);
                await renderer.RenderMusicAsync(options, tempMusicDir, progress, cancellationToken);

                // 6. Bake game world and assets
                progress?.Report(new PreparationProgress(35, "Step 3/5: Baking world and assets into PAK archives (this may take a few minutes)..."));
                var baker = new G3dsBaker(_depManager.BakeExePath);
                var dataPakPath = Path.Combine(tempPakDir, "data.pak");

                var ffmpegPath = File.Exists(_depManager.FfmpegExePath) ? _depManager.FfmpegExePath : null;
                await baker.BakeWorldAsync(options, tempMusicDir, dataPakPath, ffmpegPath, progress, cancellationToken);

                // 7. Verify PAKs
                if (!File.Exists(dataPakPath))
                {
                    throw new FileNotFoundException("Expected output file data.pak was not created by g3ds-bake.");
                }

                // 8. Package the R36S port
                progress?.Report(new PreparationProgress(88, "Step 4/5: Packaging ready-to-run port for R36S..."));
                var packager = new PortPackager(_depManager.RuntimeTemplateDirectory);
                finalPortDir = await packager.PackagePortAsync(options, tempPakDir, progress, cancellationToken);

                // 9. Finalize
                stopwatch.Stop();
                var elapsed = stopwatch.Elapsed;
                progress?.Report(new PreparationProgress(100, "Step 5/5: Port preparation complete!",
                    $"Success! Total preparation time: {elapsed.Minutes} min {elapsed.Seconds} s.\nPort location: {finalPortDir}"));

                // Open destination folder
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "explorer.exe",
                        Arguments = $"\"{finalPortDir}\"",
                        UseShellExecute = true
                    });
                }
                catch (Exception ex)
                {
                    progress?.Report(new PreparationProgress(100, "Port is ready.", $"Could not automatically open Explorer: {ex.Message}"));
                }
            }
            finally
            {
                // Clean up temporary workspace
                try
                {
                    if (Directory.Exists(tempWorkDir))
                    {
                        Directory.Delete(tempWorkDir, true);
                    }
                }
                catch { }
            }

            return finalPortDir;
        }
    }
}
