using System;
using System.Linq;
using System.Threading.Tasks;
using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using CORE.Domain.Common;
using Credentials = CORE.Domain.Entities.Credential;


namespace CommandFtpApp.Command.Credential
{
    [Command("Credentials Update", Description = "Update Credential")]
    public class CredentialCommandUpdate : CredentialsBaseCommand, ICommand
    {
        [CommandOption("ID", shortName: 'I', IsRequired = true, Description = "ID of the credential.")]
        public new string Id { get; init; }

        [CommandOption("Server", shortName: 's', IsRequired = false, Description = "Url of the FTP Server.")]
        public override string FtpServer { get; init; }

        [CommandOption("User", shortName: 'u', IsRequired = false, Description = "Name of user credential.")]
        public override string UserName { get; init; }

        [CommandOption("Password", shortName: 'p', IsRequired = false, Description = "Password of user credential.")]
        public override string Password { get; init; }

        [CommandOption("Port", IsRequired = false, Description = "Port of the server.")]
        public override string Port { get; init; } = "21";

        public override async ValueTask ExecuteAsync(IConsole console)
        {
            var credentials = await _dbFile.ReadAll();
            console.WithColors(ConsoleColor.Yellow, ConsoleColor.Black);

            if (!credentials.Data.Any())
            {
                console.Error.WriteLine($"Not has register any credential.");
                return;
            }

            var credentialToUpdate = credentials.Data.Where(x => x.Contains(Id)).FirstOrDefault();

            if (!string.IsNullOrEmpty(credentialToUpdate) && !string.IsNullOrEmpty(Id))
            {
                var response = await _dbFile.Update(GenerateCredential(DtoConnectionSever.Map(credentialToUpdate)));

                if (response.Success)
                    console.Output.WriteLine("Record Updated Succesfuly");
                else
                    ShowError(console, response);
            }
            else
                console.Error.WriteLine($"Not exist a credentials with that Id.");
        }

        private Credentials GenerateCredential(Credentials credential)
        {
            Console.WriteLine($"{UserName}");

            var newCredential = new Credentials
            {
                HostName = string.IsNullOrEmpty(FtpServer) ? credential.HostName : FtpServer,
                UserName = string.IsNullOrEmpty(UserName) ? credential.UserName : UserName,
                Password = string.IsNullOrEmpty(Password) ? credential.Password : Password,
                Port = string.IsNullOrEmpty(Port) ? credential.Port : int.Parse(Port),
            };

            return newCredential;
        }
    }
}
