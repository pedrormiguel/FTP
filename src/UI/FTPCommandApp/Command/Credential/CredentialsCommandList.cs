using System;
using System.Threading.Tasks;
using Autofac;
using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using ConsoleTables;
using CORE.Domain.Common;
using FTPPersistence.Interfaces;

namespace CommandFtpApp.Command.Credential
{
    [Command("Credentials List", Description = "List all the registered credentials server.")]
    public class CredentialsCommandList : ICommand
    {
        private readonly IDbFile _dbFile;
        
        public CredentialsCommandList()
        {
            using var scope = Program.Container.BeginLifetimeScope();
            _dbFile = scope.Resolve<IDbFile>();
        }
        
        public async ValueTask ExecuteAsync(IConsole console)
        {
            var response = await _dbFile.ReadAll();
            var columns = new[] { "ID", "HOSTNAME", "USERNAME", "PORT"};
            var table = new ConsoleTable(columns);
            console.WithColors(ConsoleColor.Yellow,ConsoleColor.Black);
            
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
    }
}