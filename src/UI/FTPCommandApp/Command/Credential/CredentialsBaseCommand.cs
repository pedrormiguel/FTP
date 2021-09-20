using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using System.Threading.Tasks;
using FTPPersistence.Interfaces;
using Autofac;
using FTPLib.Class.Common;

namespace CommandFtpApp.Command.Credential
{
    [Command("Credentials")]
    public abstract class CredentialsBaseCommand : ICommand
    {
        public readonly IDbFile _dbFile;

        public CredentialsBaseCommand()
        {
            using var scope = Program.Container.BeginLifetimeScope();
            _dbFile = scope.Resolve<IDbFile>();
        }

        [CommandOption("ID", shortName: 'I', IsRequired = false, Description = "ID of the credential.")]
        public string Id { get; init; }

        [CommandOption("Server", shortName: 's', IsRequired = false, Description = "Url of the FTP Server.")]
        public virtual string FtpServer { get; init; }

        [CommandOption("User", shortName: 'u', IsRequired = false, Description = "Name of user credential.")]
        public virtual string UserName { get; init; }

        [CommandOption("Password", shortName: 'p', IsRequired = false, Description = "Password of user credential.")]
        public virtual string Password { get; init; }

        [CommandOption("Port", IsRequired = false, Description = "Port of the server.")]
        public virtual string Port { get; init; } = "21";

        public abstract ValueTask ExecuteAsync(IConsole console);

        public virtual void ShowError(IConsole console, Response<string> response)
        {
            console.Error.WriteLine($"Not was successful. Error  {response.Error}");

            foreach (var error in response.ValidationErrors)
            {
                console.Error.WriteLine($"*{error}\n");
            }
        }
    }
}