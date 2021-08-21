using System.Collections.Generic;
using System.Threading.Tasks;
using FTPLib.Class.Common;
using FTPLib.Class.Entities;
using FTPPersistence.Interfaces;

namespace FTPPersistence.Class
{
    public class BaseRepository<T> : IPersistence<T> where T : class
    {
        protected readonly FtpDbContext FtpDbContext;

        public BaseRepository(FtpDbContext ftpDbContext)
        {
            FtpDbContext = ftpDbContext;
        }
        
        public Task<Response<T>> Add(BaseEntity entity)
        {
            FtpDbContext.Set<T>().AddAsync(entity);
        }

        public Task<Response<IEnumerable<string>>> ReadAll()
        {
            throw new System.NotImplementedException();
        }

        public Task<Response<string>> Update(BaseEntity entity)
        {
            throw new System.NotImplementedException();
        }
 
        public Task<Response<string>> Delete(BaseEntity entity)
        {
            throw new System.NotImplementedException();
        }
    }
}