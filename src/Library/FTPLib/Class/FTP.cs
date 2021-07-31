using System;
using System.Collections.Generic;
using FluentFTP;
using System.Threading.Tasks;
using FluentFTP.Helpers;
using FTPLib.Class.Common;
using FTPLib.Class.Dtos;
using FTPLib.Interfaces;

namespace FTPLib.Class
{
    public class Ftp : IFtp
    {
        private readonly FtpClient _client;
        public bool IsConnected => _client.IsConnected;
        public Ftp(string host, string user, string password, int port = 21)
        {
            _client = new FtpClient(host: host, user: user, pass: password, port: 21);
        }

        public Response<bool> Connect()
        {
            var response = new Response<bool>();

            try
            {
                var profile = _client.AutoDetect();
                _client.LoadProfile(profile[0]);
                _client.Connect();
                response.Status = true;
            }
            catch (Exception ex)
            {
                response.ErrorMapException(ex);
            }

            return response;
        }

        public async Task<Response<string[]>> GetListItems()
        {
            var response = new Response<string[]>();

            try
            {
                var directory = await _client.GetNameListingAsync();
                response.Data = directory;
            }
            catch (Exception e)
            {
                response.ErrorMapException(e);
            }

            return response;
        }
        public async Task<Response<IEnumerable<DtoItem>>> GetListItemsFiles(string folderPath)
        {
            // WriteLine("\t Files on the remote server : \n");

            var response = new Response<IEnumerable<DtoItem>>();
            var directory = new List<DtoItem>();

            try
            {
                var items = await _client.GetListingAsync(folderPath);

                foreach (FtpListItem item in items)
                {
                    if (item.Type == FtpFileSystemObjectType.File)
                    {
                        // long size    =  _client.GetFileSize(item.FullName);
                        // FtpHash hash = _client.GetChecksum(item.FullName);
                        directory.Add(DtoItem.Map(item.FullName, item.OwnerPermissions.ToString(), item.Size));
                        //WriteLine($"\t {item.FullName} - {item.OwnerPermissions} - {size}");
                    }

                    //DateTime time = _client.GetModifiedTime(item.FullName);

                    response.Data = directory;
                }
            }
            catch (Exception e)
            {
                response.ErrorMapException(e);
            }

            return response;
        }
        public async Task<Response<string>> UploadFile(string localPath, string remotePath)
        {
            var response = new Response<string>();
            var status = FtpStatus.Failed;

            if (!_client.IsConnected)
            {
                response.Error = "It's not connected";
                return response;
            }

            //WriteLine("Uploading File");

            try
            {
                status = await _client.UploadFileAsync(localPath, remotePath, createRemoteDir: true);
            }
            catch (Exception ex)
            {
                response.ErrorMapException(ex);
                //WriteLine($"Error's {ex.Data} \n {ex.Message} \n {ex.Source} {ex.TargetSite}");
            }

            response.Data = GetStatus(status);
            response.Status = status.IsSuccess();

            return response;
        }

        public async Task<Response<string>> DownloadFile(string localPathToDownload, string remotePathFile)
        {
            var response = new Response<string>();
            var status = FtpStatus.Failed;

            try
            {
                status = await _client.DownloadFileAsync(localPathToDownload, remotePathFile);
            }
            catch (Exception e)
            {
                response.ErrorMapException(e);
            }

            response.Data = GetStatus(status);
            response.Status = status.IsSuccess();

            return response;
        }

        private string GetStatus(FtpStatus status)
        {
            var response = status switch
            {
                FtpStatus.Failed => FtpStatusResponse.Failed,
                FtpStatus.Success => FtpStatusResponse.Success,
                FtpStatus.Skipped => FtpStatusResponse.Skipped,
                _ => "Not was successful."
            };

            return response;
        }
    }
}