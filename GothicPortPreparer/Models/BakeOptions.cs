using System;

namespace GothicPortPreparer.Models
{
    public class BakeOptions
    {
        public GameType GameType { get; set; } = GameType.Gothic1;
        public string GameDirectory { get; set; } = string.Empty;
        public string OutputDirectory { get; set; } = string.Empty;
        public int MaxTextureSize { get; set; } = 128;
        public int VoiceSampleRate { get; set; } = 11025;
        public int MusicSampleRate { get; set; } = 22050;
        public int VideoFps { get; set; } = 10;
        public int Threads { get; set; } = Math.Max(1, Environment.ProcessorCount);
        public bool IncludeVideo { get; set; } = true;
        public bool CheckForUpdates { get; set; } = true;
    }
}
