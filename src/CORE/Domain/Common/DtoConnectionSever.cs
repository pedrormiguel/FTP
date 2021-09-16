using System;
using CORE.Domain.Entities;

namespace FTPConsole.Class.Dto
{
    public class DtoConnectionSever : Credential
    {
        internal void Assing(string hostname, string username, string passwrod, string port)
        {
            HostName = hostname;
            UserName = username;
            Password = passwrod;
            Port = int.Parse(port);
        }

        internal void Assing(string hostname, string username, string passwrod, int port)
        {
            HostName = hostname;
            UserName = username;
            Password = passwrod;
            Port = port;
        }

        public static DtoConnectionSever Map(string line)
        {
            var credentials = line.Split(";");

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
