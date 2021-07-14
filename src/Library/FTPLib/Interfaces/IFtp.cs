using System.Threading.Tasks;
using FTPLib.Class.Common;

namespace FTPLib.Interfaces
{
    public interface IFtp
    {
        Response Connect();
        Task<Response> GetListItems();
        Task<Response> GetListItemsFiles(string folderPath);
        Task<Response> UploadFile(string localPath, string remotePath);
        Task<Response> DownloadFile(string localPathToDownload, string remotePathFile);
    }
}