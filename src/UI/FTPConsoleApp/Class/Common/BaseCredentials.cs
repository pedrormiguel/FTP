using System;

namespace FTPConsole.Class.Common
{
    public class BaseCredentials
    {
        public Guid Id { get; set;}

        public BaseCredentials()
        {
            Id = Guid.NewGuid();
        }
    }
}