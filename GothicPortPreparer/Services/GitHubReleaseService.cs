using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Threading;
using System.Threading.Tasks;
using GothicPortPreparer.Models;

namespace GothicPortPreparer.Services
{
    public class GitHubReleaseService
    {
        private const string UserAgent = "GothicPortPreparer/1.0 (+https://github.com/r36s-gothic)";
        private const string LatestReleaseUrl = "https://api.github.com/repos/ponuno/GothicN3DS/releases/latest";

        static GitHubReleaseService()
        {
            // Ensure modern TLS protocols are enabled
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;
        }

        public async Task<GitHubRelease?> FetchLatestReleaseAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(LatestReleaseUrl);
                request.Method = "GET";
                request.UserAgent = UserAgent;
                request.Accept = "application/vnd.github.v3+json";

                using (cancellationToken.Register(() => request.Abort()))
                {
                    using var response = (HttpWebResponse)await request.GetResponseAsync();
                    using var stream = response.GetResponseStream();
                    if (stream == null) return null;

                    var serializer = new DataContractJsonSerializer(typeof(GitHubRelease));
                    return (GitHubRelease?)serializer.ReadObject(stream);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task DownloadFileWithProgressAsync(
            string url,
            string destinationPath,
            IProgress<PreparationProgress>? progress,
            string fileLabel,
            CancellationToken cancellationToken = default)
        {
            var tempFile = destinationPath + ".download";
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.UserAgent = UserAgent;

                using (cancellationToken.Register(() => request.Abort()))
                using (var response = (HttpWebResponse)await request.GetResponseAsync())
                using (var responseStream = response.GetResponseStream())
                using (var fileStream = new FileStream(tempFile, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    var totalBytes = response.ContentLength;
                    var buffer = new byte[81920];
                    long totalRead = 0;
                    int read;

                    while ((read = await responseStream.ReadAsync(buffer, 0, buffer.Length, cancellationToken)) > 0)
                    {
                        await fileStream.WriteAsync(buffer, 0, read, cancellationToken);
                        totalRead += read;

                        if (totalBytes > 0 && progress != null)
                        {
                            var pct = (int)((totalRead * 100) / totalBytes);
                            var readMb = totalRead / (1024.0 * 1024.0);
                            var totalMb = totalBytes / (1024.0 * 1024.0);
                            progress.Report(new PreparationProgress(
                                pct,
                                $"Pobieranie {fileLabel} ({readMb:F1} / {totalMb:F1} MB)..."
                            ));
                        }
                    }
                }

                if (File.Exists(destinationPath))
                    File.Delete(destinationPath);

                File.Move(tempFile, destinationPath);
            }
            catch
            {
                if (File.Exists(tempFile))
                {
                    try { File.Delete(tempFile); } catch { }
                }
                throw;
            }
        }
    }
}
