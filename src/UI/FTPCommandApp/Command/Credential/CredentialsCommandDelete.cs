using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using CORE.Domain.Common;
using FTPLib.Class.Common;

namespace CommandFtpApp.Command.Credential
{
    [Command("Credentials Delete", Description = "Delete credential server registered.")]
    public class CredentialsCommandDelete : CredentialsBaseCommand, ICommand
    {
        public async override ValueTask ExecuteAsync(IConsole console)
        {
            var credentials = await _dbFile.ReadAll();
            console.WithColors(ConsoleColor.Yellow, ConsoleColor.Black);

            if (!credentials.Data.Any())
            {
                await console.Error.WriteLineAsync($"Not has register any credential.");
                return;
            }

            var credentialSelected = credentials.Data.Where(x => x.Contains(Id)).FirstOrDefault();

            if (!string.IsNullOrEmpty(Id) && !string.IsNullOrEmpty(credentialSelected))
            {
                var dto = DtoConnectionSever.Map(credentialSelected);
                var response = await _dbFile.Delete(dto);

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