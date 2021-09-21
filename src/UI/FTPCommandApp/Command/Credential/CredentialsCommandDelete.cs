using System;
using System.Linq;
using System.Threading.Tasks;
using CliFx.Attributes;
using CliFx.Infrastructure;
using CORE.Domain.Common;
using FTPPersistence.Interfaces;

namespace CommandFtpApp.Command.Credential
{
    
    [Command("Credentials Delete", Description = "Delete credential server registered.")]
    public class CredentialsCommandDelete : CredentialsBaseCommand
    {
        public CredentialsCommandDelete(IDbFile dbFile) : base(dbFile)
        {
        }
        
        [CommandOption("ID", shortName: 'I', IsRequired = true, Description = "ID of the credential.")]
        public override string Id { get; init; }
        
        public override async ValueTask ExecuteAsync(IConsole console)
        {
            var credentials = await DbFile.ReadAll();
            console.WithColors(ConsoleColor.Yellow, ConsoleColor.Black);

            if (!credentials.Data.Any())
            {
                await console.Error.WriteLineAsync($"Not has register any credential.");
                return;
            }

            var credentialSelected = credentials.Data.FirstOrDefault(x => x.Contains(Id));

            if (!string.IsNullOrEmpty(Id) && !string.IsNullOrEmpty(credentialSelected))
            {
                var dto = DtoConnectionSever.Map(credentialSelected);
                var response = await DbFile.Delete(dto);

                if (response.Success)
                    await console.Output.WriteLineAsync($"Element deleted. {dto.HostName}");
                else
                {
                    ShowError(console, response);
                }
            }
            else
                await console.Error.WriteLineAsync($"Not exist a credentials with that Id.");
        }
    }
}