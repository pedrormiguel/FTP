using CORE.Application.Responses;
using System;

namespace FTPLib.Class.Common
{
    public class Response<T> : BaseResponse<T>
    {
        public T Data { get; set; }
        public string Error { get; private set; }

        public void ErrorMapException(Exception ex)
        {
            Error = $"Error's {ex.Data} \n {ex.Message} \n {ex.Source} {ex.TargetSite}";
            Success = false;
        }
    }
}