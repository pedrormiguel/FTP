using System.Collections.Generic;
using System.Threading.Tasks;
using CORE.Domain.Common;
using FTPLib.Class.Common;

namespace FTPPersistence.Interfaces
{
    public interface IDbFile
    {
        Task<Response<bool>> Add(BaseEntity credentials);
        Task<Response<string>> Delete(BaseEntity credentials);
        Task<Response<IEnumerable<string>>> ReadAll();
        Task<Response<string>> Update(BaseEntity credentials);
    }
}