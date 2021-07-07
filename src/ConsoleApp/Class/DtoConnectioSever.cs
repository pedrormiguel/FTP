using System;
using ConsoleApp.Class;

namespace ConsoleApp.Class
{
    public class DtoConnectioSever : BaseCredentials
    {
        public string HostName { get; set; }
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
            return $"{this.Id};{this.HostName};{this.UserName};{this.Password};{this.Port}";
        }

        public static DtoConnectioSever Map(string Line)
        {
            var credentials = Line.Split(";");

            return new DtoConnectioSever
            {
                Id = Guid.Parse(credentials[0]),
                HostName = credentials[1],
                UserName = credentials[2],
                Password = credentials[3],
                Port = int.Parse(credentials[4])
            };
        }
    }
}
