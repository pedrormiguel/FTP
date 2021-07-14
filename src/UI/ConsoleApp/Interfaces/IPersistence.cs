using System.Threading.Tasks;
using ConsoleApp.Class;
using FTPLib.Class.Common;

namespace FTPConsole.Interfaces
{
    public interface IPersistence
    {
        Task<Response> Add(BaseCredentials credentials);
        Task<Response> ReadAll();
        Task<Response> Update(BaseCredentials credentials);
        Task<Response> Delete(BaseCredentials credentials);
    }
}