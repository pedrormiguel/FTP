using System;
using System.Threading.Tasks;
using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using ConsoleTables;
using CORE.Domain.Common;
using FTPPersistence.Interfaces;

namespace CommandFtpApp.Command.Credential
{
    [Command("Credentials List", Description = "List all the registered credentials server.")]
    public class CredentialsCommandList : CredentialsBaseCommand
    {
        private readonly string[] _columns = new string[] { "ID", "HOSTNAME", "USERNAME", "PORT" };

        public override async ValueTask ExecuteAsync(IConsole console)
        {
            var response = await DbFile.ReadAll();

            var table = new ConsoleTable(_columns);
            console.WithColors(ConsoleColor.Yellow, ConsoleColor.Black);

            await console.Output.WriteLineAsync("\nLIST OF ALL CREDENTIAL'S SERVER\n");

            if (response.Success)
                foreach (var credential in response.Data)
                {
                    var dto = DtoConnectionSever.Map(credential);
                    table.AddRow(dto.Id, dto.HostName, dto.UserName, dto.Port);
                }

            table.Write();
            await console.Output.WriteLineAsync();
        }

        public CredentialsCommandList(IDbFile dbFile) : base(dbFile)
        {
        }
    }
}