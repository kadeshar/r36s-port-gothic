using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GothicPortPreparer.Models
{
    [DataContract]
    public class GitHubRelease
    {
        [DataMember(Name = "tag_name")]
        public string TagName { get; set; } = string.Empty;

        [DataMember(Name = "name")]
        public string Name { get; set; } = string.Empty;

        [DataMember(Name = "published_at")]
        public string PublishedAt { get; set; } = string.Empty;

        [DataMember(Name = "assets")]
        public List<GitHubAsset> Assets { get; set; } = new List<GitHubAsset>();
    }

    [DataContract]
    public class GitHubAsset
    {
        [DataMember(Name = "name")]
        public string Name { get; set; } = string.Empty;

        [DataMember(Name = "size")]
        public long Size { get; set; }

        [DataMember(Name = "browser_download_url")]
        public string BrowserDownloadUrl { get; set; } = string.Empty;
    }
}
