using System;
using System.Collections.Generic;
using System.IO;
using GothicPortPreparer.Models;

namespace GothicPortPreparer.Services
{
    public class DetectedGame
    {
        public GameType GameType { get; set; }
        public string Path { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Source { get; set; } = string.Empty; // e.g. "Steam", "GOG", "Custom"
    }

    public static class GameDetector
    {
        public static List<DetectedGame> FindInstalledGames()
        {
            var results = new List<DetectedGame>();
            var checkedPaths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            void CheckPath(string dir, string source)
            {
                if (string.IsNullOrWhiteSpace(dir) || checkedPaths.Contains(dir) || !Directory.Exists(dir))
                    return;

                checkedPaths.Add(dir);
                var validation = ValidatePath(dir);
                if (validation.IsValid && validation.DetectedType.HasValue)
                {
                    results.Add(new DetectedGame
                    {
                        GameType = validation.DetectedType.Value,
                        Path = dir,
                        Name = validation.DetectedType.Value == GameType.Gothic1 ? "Gothic 1" : "Gothic 2",
                        Source = source
                    });
                }
            }

            // 1. Standard Steam locations
            var standardSteam = @"C:\Program Files (x86)\Steam\steamapps\common";
            CheckPath(Path.Combine(standardSteam, "Gothic"), "Steam");
            CheckPath(Path.Combine(standardSteam, "Gothic II"), "Steam");
            CheckPath(Path.Combine(standardSteam, "Gothic 2"), "Steam");

            // 2. Read Steam libraryfolders.vdf for custom library locations
            try
            {
                var vdfPath = @"C:\Program Files (x86)\Steam\steamapps\libraryfolders.vdf";
                if (File.Exists(vdfPath))
                {
                    foreach (var line in File.ReadAllLines(vdfPath))
                    {
                        var trimmed = line.Trim();
                        if (trimmed.StartsWith("\"path\"", StringComparison.OrdinalIgnoreCase))
                        {
                            var parts = trimmed.Split(new[] { '"' }, StringSplitOptions.RemoveEmptyEntries);
                            if (parts.Length >= 2)
                            {
                                var libPath = parts[parts.Length - 1].Replace(@"\\", @"\");
                                var common = Path.Combine(libPath, "steamapps", "common");
                                CheckPath(Path.Combine(common, "Gothic"), "Steam");
                                CheckPath(Path.Combine(common, "Gothic II"), "Steam");
                                CheckPath(Path.Combine(common, "Gothic 2"), "Steam");
                            }
                        }
                    }
                }
            }
            catch { }

            // 3. GOG locations
            var gogDirs = new[]
            {
                @"C:\GOG Games\Gothic",
                @"C:\GOG Games\Gothic 2 Gold",
                @"C:\GOG Games\Gothic II",
                @"D:\GOG Games\Gothic",
                @"D:\GOG Games\Gothic 2 Gold"
            };
            foreach (var gog in gogDirs)
            {
                CheckPath(gog, "GOG");
            }

            return results;
        }

        public class PathValidationResult
        {
            public bool IsValid { get; set; }
            public GameType? DetectedType { get; set; }
            public string Message { get; set; } = string.Empty;
        }

        public static PathValidationResult ValidatePath(string dir)
        {
            if (string.IsNullOrWhiteSpace(dir) || !Directory.Exists(dir))
            {
                return new PathValidationResult { IsValid = false, Message = "Directory does not exist." };
            }

            var hasData = Directory.Exists(Path.Combine(dir, "Data")) || Directory.Exists(Path.Combine(dir, "data"));
            var hasWork = Directory.Exists(Path.Combine(dir, "_work")) || Directory.Exists(Path.Combine(dir, "_Work"));

            if (!hasData || !hasWork)
            {
                return new PathValidationResult
                {
                    IsValid = false,
                    Message = "Folder does not contain required 'Data' and '_work' directories."
                };
            }

            var g2Exe = Path.Combine(dir, "system", "Gothic2.exe");
            var g1Exe = Path.Combine(dir, "system", "GOTHIC.EXE");

            if (File.Exists(g2Exe))
            {
                return new PathValidationResult
                {
                    IsValid = true,
                    DetectedType = GameType.Gothic2,
                    Message = "Gothic 2 detected (Gothic2.exe)"
                };
            }

            if (File.Exists(g1Exe))
            {
                return new PathValidationResult
                {
                    IsValid = true,
                    DetectedType = GameType.Gothic1,
                    Message = "Gothic 1 detected (GOTHIC.EXE)"
                };
            }

            // Secondary check: look for world files
            var vdfDir = Path.Combine(dir, "Data");
            if (File.Exists(Path.Combine(vdfDir, "Worlds.vdf")) || File.Exists(Path.Combine(vdfDir, "worlds.vdf")))
            {
                return new PathValidationResult
                {
                    IsValid = true,
                    DetectedType = GameType.Gothic2,
                    Message = "Gothic 2 detected (Worlds.vdf)"
                };
            }

            return new PathValidationResult
            {
                IsValid = true,
                DetectedType = GameType.Gothic1,
                Message = "Gothic installation detected (Data / _work structure)"
            };
        }
    }
}
