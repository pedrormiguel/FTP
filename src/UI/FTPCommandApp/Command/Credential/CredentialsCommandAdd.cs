using CliFx.Attributes;
using CliFx.Infrastructure;
using System.Threading.Tasks;
using FTPPersistence.Interfaces;
using Credentials = CORE.Domain.Entities.Credential;

namespace CommandFtpApp.Command.Credential
{
    [Command("Credentials Add", Description = "Add new ftp server with credential")]
    public class CredentialsCommandAdd : CredentialsBaseCommand
    {
        [CommandOption("Server", shortName: 's', IsRequired = true, Description = "Url of the FTP Server.")]
        public override string FtpServer { get; init; }

        [CommandOption("User", shortName: 'u', IsRequired = true, Description = "Name of user credential.")]
        public override string UserName { get; init; }

        [CommandOption("Password", shortName: 'p', IsRequired = true, Description = "Password of user credential.")]
        public override string Password { get; init; }

        public CredentialsCommandAdd(IDbFile dbFile) : base(dbFile)
        {
        }
        
        public override ValueTask ExecuteAsync(IConsole console)
        {
            console.Output.WriteLine("Adding credentials");

            var port = int.Parse(Port);
            var credential = new Credentials { HostName = this.FtpServer, UserName = this.UserName, Password = this.Password, Port = port };
            var response = DbFile.Add(credential);

            console.Output.WriteLine($"Status of the request : {response.Result.Success}");

            if (!response.Result.Success)
                console.Output.WriteLine($"Error Messages : {response.Result.Message}");

            return default;
        }
    }
}