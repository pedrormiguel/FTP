using System;

namespace FTPLib.Class.Common
{
    public class Response<T>
    {
        public bool Status { get; set; } = false;
        public T Data { get; set; }
        public string Error { get; set; }

        public void ErrorMapException(Exception ex)
        {
            Error = $"Error's {ex.Data} \n {ex.Message} \n {ex.Source} {ex.TargetSite}";
        }
    }
}