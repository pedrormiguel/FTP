using System;

namespace ConsoleApp.Class
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