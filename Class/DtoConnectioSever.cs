using System;
using src.Class;

namespace src.Class
{
    public class DtoConnectioSever : BaseCredentials
    {
        public string HostName {get; set;}
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Port { get; set; } = 21;

        internal void Assing(string hostname, string username, string passwrod, string port)
        {
            HostName = hostname;
            UserName = username;
            Password = passwrod;
            Port = int.Parse(port);
        }

        public override string ToString()
        {
            return $"{this.Id}-{this.HostName}-{this.UserName}-{this.Password}-{this.Port}";
        }
    }
}
