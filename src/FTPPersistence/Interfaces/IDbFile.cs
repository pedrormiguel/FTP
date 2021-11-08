using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CORE.Domain.Common;
using CORE.Domain.Entities;
using FTPLib.Class.Common;

namespace FTPPersistence.Interfaces
{
    public interface IDbFile
    {
        Task<Response<Credential>> GetById(Guid id);
        string GetPathFile();
        string GetPathDirectory();
        Task<Response<bool>> Add(BaseEntity credentials);
        Task<Response<IEnumerable<string>>> ReadAll();
        Task<Response<string>> Delete(BaseEntity credentials);
        Task<Response<string>> Update(BaseEntity credentials);
    }
}