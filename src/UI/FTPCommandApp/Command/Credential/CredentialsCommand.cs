using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using System.Threading.Tasks;
using Credentials = CORE.Domain.Entities.Credential;
using FTPPersistence.Interfaces;
using Autofac;

namespace CommandFtpApp.Command.Credential
{
    [Command("Credentials Add", Description = "Add new ftp server with credential")]
    public class CredentialsCommand : ICommand
    {
        private readonly IDbFile _dbFile;

        public CredentialsCommand()
        {
            using var scope = Program.Container.BeginLifetimeScope();
            _dbFile = scope.Resolve<IDbFile>();
        }

        [CommandOption("Server", shortName: 's', IsRequired = true, Description = "Url of the FTP Server.")]
        public string FtpServer { get; init; }

        [CommandOption("User", shortName: 'u', IsRequired = true, Description = "Name of user credential.")]
        public string UserName { get; init; }

        [CommandOption("Password", shortName: 'p', IsRequired = true, Description = "Password of user credential.")]
        public string Password { get; init; }

        [CommandOption("Port", Description = "Port of the server.")]
        public string Port { get; init; } = "21";

        public ValueTask ExecuteAsync(IConsole console)
        {
            console.Output.WriteLine("Adding credentials");

            var port = int.Parse(Port);
            var credential = new Credentials { HostName = this.FtpServer, UserName = this.UserName, Password = this.Password, Port = port };
            var response = _dbFile.Add(credential);

            console.Output.WriteLine($"Status of the request : {response.Result.Success}");

            if (!response.Result.Success)
                console.Output.WriteLine($"Error Messages : {response.Result.Message}");

            return default;
        }
    }
}