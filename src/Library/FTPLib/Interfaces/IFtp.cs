using FTPLib.Class.Common;
using FTPLib.Class.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FTPLib.Interfaces
{
    public interface IFtp
    {
        Response<bool> Connect();
        Task<Response<string[]>> GetListItems();
        Task<Response<IEnumerable<DtoItem>>> GetListItemsFiles(string folderPath);
        Task<Response<string>> UploadFile(string localPath, string remotePath);
        Task<Response<string>> DownloadFile(string localPathToDownload, string remotePathFile);
    }
}