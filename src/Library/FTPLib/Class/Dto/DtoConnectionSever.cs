using CORE.Domain.Common;
using System;

namespace FTPLib.Class.Dto
{
    public class DtoConnectionSever : BaseEntity
    {
        public string HostName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Port { get; set; } = 21;

        internal void Assing(string hostname, string username, string password, string port)
        {
            HostName = hostname;
            UserName = username;
            Password = password;
            Port = int.Parse(port);
        }

        internal void Assing(string hostname, string username, string password, int port)
        {
            HostName = hostname;
            UserName = username;
            Password = password;
            Port = port;
        }

        public override string ToString()
        {
            return $"{this.Id};{this.HostName};{this.UserName};{this.Password};{this.Port}";
        }

        public static DtoConnectionSever Map(string line)
        {
            var credentials = line.Split(';');

            return new DtoConnectionSever
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
