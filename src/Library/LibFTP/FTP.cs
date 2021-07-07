using System.Net;
using System;
using static System.Console;
using FluentFTP;
using System.Threading.Tasks;

namespace LibFTP
{
    public class FTP
    {
        private FtpClient _client;
        private NetworkCredential _credentials;
        public bool IsConnected { get => _client.IsConnected;  }

        public FTP(string host, string user, string password, int port = 21)
        {
            _credentials = new NetworkCredential(userName: user, password: password);

            _client = new FtpClient(host: host ,port: port, credentials: _credentials);
        }

        public bool Connect()
        {
            try
            {
               var profile = _client.AutoDetect();

                _client.LoadProfile(profile[0]);

                _client.Connect();
            }
            catch (Exception ex)
            {
                WriteLine("It's not Connected to the server");  
                WriteLine($"Error's {ex.Data} \n {ex.Message} \n {ex.Source} {ex.TargetSite}");
            }

            return _client.IsConnected;
        }
        public async Task ListItems()
        {
            var directory = await _client.GetNameListingAsync();

            WriteLine("Files on the remote server : \n");

            foreach (var item in directory)
            {
                WriteLine($"- {item}");
            }
        }
        public async Task ListItemsFiles(string folderPath)
        {
            WriteLine("\t Files on the remote server : \n");

             foreach (FtpListItem item in await _client.GetListingAsync(folderPath))
            {
                if (item.Type == FtpFileSystemObjectType.File)
                {
                    long size = _client.GetFileSize(item.FullName); 
                    FtpHash hash = _client.GetChecksum(item.FullName);

                    WriteLine($"\t {item.FullName} - {item.OwnerPermissions} - {size}");
                }

                DateTime time = _client.GetModifiedTime(item.FullName);
            }

        }
        public async Task<bool> UploadFile(string localPath, string remotePath)
        {
            if (_client.IsConnected)
            {
                WriteLine("Uploading File");

                try
                {
                    var status = await _client.UploadFileAsync(localPath, remotePath, createRemoteDir: true);

                    return true;
                }
                catch (Exception ex)
                {
                    WriteLine($"Error's {ex.Data} \n {ex.Message} \n {ex.Source} {ex.TargetSite}");
                }
            }

            return false;
        }
        public async Task DownloadFile(string localPathToDownload, string remotePathFile)
        {
            await _client.DownloadFileAsync(localPathToDownload, remotePathFile); 
        }
    }
}