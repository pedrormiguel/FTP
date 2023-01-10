using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CORE.Domain.Entities;
using FluentFTP;
using FluentFTP.Helpers;
using FTPLib.Class.Common;
using FTPLib.Class.Dto;
using FTPLib.Interfaces;

namespace FTPLib.Class
{
    public class Ftp : IFtp
    {
        private readonly FtpClient _client;
        public bool IsConnected => _client.IsConnected;
        public Ftp(string host, string user, string password, int port = 21)
        {
            _client = new FtpClient(host: host, user: user, pass: password, port: port);
        }
        public Ftp(Credential credential)
        {
            if( !(credential.UserName.IsBlank() || credential.UserName.ToLower().Equals("-")) )
                _client = new FtpClient(host: credential.HostName, user: credential.UserName, pass: credential.Password, port: credential.Port);
            else
            _client = new FtpClient(host:credential.HostName);
        }

        public Response<bool> Connect()
        {
            var response = new Response<bool>();

            try
            {
                var profile = _client.AutoDetect();
                _client.LoadProfile(profile[0]);
                _client.Connect();
                response.Success = true;
                response.Data = true;
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
                response.Success = true;
            }
            catch (Exception e)
            {
                response.ErrorMapException(e);
            }

            return response;
        }

        public async Task<Response<IEnumerable<DtoItem>>> GetListItemsFiles(string folderPath)
        {
            var response = new Response<IEnumerable<DtoItem>>();
            var directory = new List<DtoItem>();

            try
            {
                var items = await _client.GetListingAsync(folderPath);

                foreach (FtpListItem item in items)
                {
                    if (item.Type == FtpFileSystemObjectType.File )
                        directory.Add(DtoItem.Map(item.FullName, item.OwnerPermissions.ToString(), item.Size));

                    response.Data = directory;
                }

                response.Success = true;

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
                response.Message = "It's not connected";
                return response;
            }

            try
            {
                status =  await _client.UploadFileAsync(localPath, remotePath, createRemoteDir: true);
            }
            catch (Exception ex)
            {
                response.ErrorMapException(ex);
            }

            response.Data = GetStatus(status);
            response.Success = status.IsSuccess();

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
            response.Success = status.IsSuccess();

            return response;
        }

        private string GetStatus(FtpStatus status)
        {
            string response;

            switch (status)
            {
                case FtpStatus.Failed:
                    response = FtpStatusResponse.Failed;
                    break;
                case FtpStatus.Success:
                    response = FtpStatusResponse.Success;
                    break;
                case FtpStatus.Skipped:
                    response = FtpStatusResponse.Skipped;
                    break;
                default:
                    response = "Not was successful.";
                    break;
            }

            return response;
        }
    }
}