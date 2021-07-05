using System.Threading.Tasks;
using src.Class;

namespace src.Interfaces
{
    public interface IPersistence
    {
        Task<Response> Add(BaseCredentials credentials);
        Task<Response> ReadAll();
        Task<Response> Update(BaseCredentials credentials);
        Task<Response> Delete(BaseCredentials credentials);
    }
}