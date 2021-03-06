using FocusWarden.Tests.UI.Reporting.Interfaces;
using FocusWarden.Tests.UI.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FocusWarden.Tests.UI.Reporting
{
    public class FtpHelper : IFtpHelper
    {
        public async Task<bool> TryCreateDirectoryAsync(string directory)
        {

            var request = (FtpWebRequest)WebRequest.Create($"{Configuration.FtpTargetDirectory}/{directory}");
            request.Credentials = new NetworkCredential(Configuration.FtpUser, Configuration.FtpPassword);
            request.Method = WebRequestMethods.Ftp.MakeDirectory;
            request.UseBinary = true;
            request.UsePassive = true;
            request.KeepAlive = false;
            request.ServicePoint.Expect100Continue = false;
            try
            {
                var resp = await request.GetResponseAsync();
                return ((FtpWebResponse)resp).StatusCode == FtpStatusCode.PathnameCreated;
            }
            catch (WebException ex)
            {
                Console.WriteLine($"Response: {ex.Response}");
                Console.WriteLine($"Message: {ex.Message}");
                Console.WriteLine($"Status: {ex.Status}");
                Console.WriteLine($"Source: {ex.Source}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                if (ex.Response.Headers.HasKeys()) Console.WriteLine($"\tHeader: {string.Join(", ", ex.Response.Headers.AllKeys)}");
                return false;
            }
        }

        public async Task<string[]> TryUploadFilesAsync(string[] files, string destDirectory)
        {
            try
            {
                var urls = new List<string>(files.Length);
                using (WebClient client = new WebClient())
                {
                    client.Credentials = new NetworkCredential(Configuration.FtpUser, Configuration.FtpPassword);
                    foreach (string file in files)
                    {
                        var address = Path.Combine($"ftp://{Configuration.FtpHostName}/public_html/d/testing/{destDirectory}", Path.GetFileName(file));
                        urls.Add(address);
                        await client.UploadFileTaskAsync(address, file);
                    }
                    return urls.Select(u => u.Replace("ftp://", "https://").Replace("public_html/", "").Replace('\\', '/')).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        //public Task<bool> TryCreateDirectoryAsync(string directory)
        //{
        //    var client = new FtpClient
        //    {
        //        Host = Configuration.FtpHostName,
        //        Port = Configuration.FtpPort,
        //        Credentials = new NetworkCredential(Configuration.FtpUser, Configuration.FtpPassword),
        //        DataConnectionType = FtpDataConnectionType.PASV,
        //        ValidateAnyCertificate = true,
        //        RetryAttempts = 3,
        //        EncryptionMode = FtpEncryptionMode.None
        //    };

        //    client.Connect();

        //    var result = client.CreateDirectory($"{Configuration.FtpTargetDirectory}/{directory}");

        //    client.Disconnect();

        //    return Task.FromResult(result);

        //}

        //public Task<string[]> TryUploadFilesAsync(string[] files, string directory)
        //{

        //    var client = new FtpClient
        //    {
        //        Host = Configuration.FtpHostName,
        //        Port = Configuration.FtpPort,
        //        Credentials = new NetworkCredential(Configuration.FtpUser, Configuration.FtpPassword),
        //        DataConnectionType = FtpDataConnectionType.PASV,
        //        ValidateAnyCertificate = true,
        //        RetryAttempts = 3,
        //        EncryptionMode = FtpEncryptionMode.None
        //    };

        //    client.Connect();

        //    var result = client.UploadFiles(files, $"{Configuration.FtpTargetDirectory}/{directory}");

        //    client.Disconnect();

        //    var urls = files.Select(f => $"{Configuration.FtpTargetDirectory}/{directory}/{Path.GetFileName(f)}".Replace("ftp://", "https://").Replace("public_html/", "").Replace('\\', '/'))
        //                    .ToArray();

        //    return Task.FromResult(urls);
        //}
    }
}
