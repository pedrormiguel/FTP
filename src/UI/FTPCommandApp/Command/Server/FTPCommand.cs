using System;
using System.Threading.Tasks;
using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using FTPLib.Class;
using FTPPersistence.Interfaces;

namespace CommandFtpApp.Command.Server
{
    [Command("FTP")]
    public abstract class FTP : ICommand
    {
        protected Guid GuidId;

        [CommandOption("ID", shortName: 'I', IsRequired = true, Description = "ID of the credential.")]
        public string Id { get; set; }

        public abstract ValueTask ExecuteAsync(IConsole console);
    }

    [Command("FTP Test Connection")]
    public class FtpCommand : FTP
    {
        private Ftp _ftpClient;
        private readonly IDbFile _dbFile;

        public FtpCommand(IDbFile dbFile)
        {
            _dbFile = dbFile;
        }

        public override async ValueTask ExecuteAsync(IConsole console)
        {
            console.WithColors(ConsoleColor.Yellow, ConsoleColor.Black);

            if (!Guid.TryParse(Id, out GuidId))
            {
                await console.Error.WriteLineAsync("Guid should contain 32 digits with 4 dashes (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx).");
                return;
            }

            var response = await _dbFile.GetById(GuidId);

            if (!response.Success)
            {
                await console.Error.WriteLineAsync($"Not register {Id}.");
                return;
            }

            _ftpClient = new Ftp(response.Data);
            var status = _ftpClient.Connect();
            await console.Output.WriteLineAsync($"Connection With Server Successful: {_ftpClient.IsConnected}");
        }
    }

    [Command("FTP DisplayFiles", Description = "Display all the files on the server.")]
    public class FtpCommandDisplay : ICommand
    {
        public ValueTask ExecuteAsync(IConsole console)
        {
            console.Output.WriteLine("Displaying");

            return default;
        }
    }
}