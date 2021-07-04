using System.Threading.Tasks;
using src.Class;

namespace src.Interfaces
{
    public interface IPersistence
    {
         Task<bool> Add(BaseCredentials credentials);
         Task Read(BaseCredentials credentials);
         Task Update(BaseCredentials credentials);
         Task Delete(BaseCredentials credentials);
    }
}