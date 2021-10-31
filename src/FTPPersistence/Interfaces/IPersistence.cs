using System.Collections.Generic;
using System.Threading.Tasks;
using CORE.Domain.Common;
using FTPLib.Class.Common;

namespace FTPPersistence.Interfaces
{
    public interface IPersistence<T> where T : class
    {
        Task<Response<T>> Add(BaseEntity entity);
        Task<Response<IEnumerable<T>>> ReadAll();
        Task<Response<T>> Update(BaseEntity entity);
        Task<Response<T>> Delete(BaseEntity entity);
    }
}