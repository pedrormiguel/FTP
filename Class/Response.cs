using System;

namespace src.Class
{
    public class Response
    {
        public bool status { get; set; } = false;
        public Object Data { get; set; } = null;
        public string Error { get; set;}
    }
}