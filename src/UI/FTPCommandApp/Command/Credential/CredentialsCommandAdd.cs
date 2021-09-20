using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using System.Threading.Tasks;
using Credentials = CORE.Domain.Entities.Credential;

namespace CommandFtpApp.Command.Credential
{
    [Command("Credentials Add", Description = "Add new ftp server with credential")]
    public class CredentialsCommandAdd : CredentialsBaseCommand, ICommand
    {

        public override ValueTask ExecuteAsync(IConsole console)
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