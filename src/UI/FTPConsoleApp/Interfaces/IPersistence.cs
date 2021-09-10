using System.Collections;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FTPConsole.Class.Common;
using FTPLib.Class.Common;
using BaseCredentials = FTPConsole.Class.Common.BaseCredentials;

namespace FTPConsole.Interfaces
{
    public interface IPersistence
    {
        Task<Response<bool>> Add(BaseCredentials credentials);
        Task<Response<IEnumerable<string>>> ReadAll();
        Task<Response<string>> Update(BaseCredentials credentials);
        Task<Response<string>> Delete(BaseCredentials credentials);
    }
}