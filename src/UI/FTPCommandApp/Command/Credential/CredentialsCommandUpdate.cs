using System;
using System.Linq;
using System.Threading.Tasks;
using CliFx.Attributes;
using CliFx.Infrastructure;
using CORE.Domain.Common;
using FTPPersistence.Interfaces;
using Credentials = CORE.Domain.Entities.Credential;


namespace CommandFtpApp.Command.Credential
{
    [Command("Credentials Update", Description = "Update Credential")]
    public class CredentialCommandUpdate : CredentialsBaseCommand
    {
        public CredentialCommandUpdate(IDbFile dbFile) : base(dbFile)
        {
        }
        
        [CommandOption("ID", shortName: 'I', IsRequired = true, Description = "ID of the credential.")]
        public new string Id { get; set; }
        
        public override async ValueTask ExecuteAsync(IConsole console)
        {
            var credentials = await DbFile.ReadAll();
            console.WithColors(ConsoleColor.Yellow, ConsoleColor.Black);
 
            if (!credentials.Data.Any())
            {
                await console.Error.WriteLineAsync($"Not has register any credential.");
                return;
            }

            var credentialToUpdate = credentials.Data.FirstOrDefault(x => x.Contains(Id));

            if (!string.IsNullOrEmpty(credentialToUpdate) && !string.IsNullOrEmpty(Id))
            {
                var response = await DbFile.Update(GenerateCredential(DtoConnectionSever.Map(credentialToUpdate)));

                if (response.Success)
                    await console.Output.WriteLineAsync("Record Updated Successfully");
                else
                    ShowError(console, response);
            }
            else
                await console.Error.WriteLineAsync($"Not exist a credentials with that Id.");
        }

        private Credentials GenerateCredential(Credentials credential)
        {
            Console.WriteLine($"{UserName}");
            
            credential.HostName = string.IsNullOrEmpty(FtpServer) ? credential.HostName : FtpServer;
            credential.UserName = string.IsNullOrEmpty(UserName) ? credential.UserName : UserName;
            credential.Password = string.IsNullOrEmpty(Password) ? credential.Password : Password;
            credential.Port = string.IsNullOrEmpty(Port) ? credential.Port : int.Parse(Port);
            
            return credential;
        }
    }
}
