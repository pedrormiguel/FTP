using System;

namespace FTPLib.Class.Common
{
    public class Response
    {
        public bool Status { get; set; } = false;
        public Object Data { get; set; } = null;
        public string Error { get; set;}

        public void ErrorMapException(Exception ex)
        {
            Error = $"Error's {ex.Data} \n {ex.Message} \n {ex.Source} {ex.TargetSite}";
        }
    }
}