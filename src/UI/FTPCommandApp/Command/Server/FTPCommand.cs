using System;
using System.Threading.Tasks;
using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using CommandFtpApp.Common;
using FTPLib.Class;
using FTPPersistence.Interfaces;

namespace CommandFtpApp.Command.Server
{
    [Command("FTP")]
    public abstract class FTP : ICommand
    {
        protected Guid GuidId;
        protected Ftp _ftpClient;
        protected readonly IDbFile _dbFile;

        protected FTP(IDbFile dbFile)
        {
            _dbFile = dbFile;
        }

        [CommandOption("ID", shortName: 'I', IsRequired = true, Description = "ID of the credential.")]
        public string Id { get; set; }

        public abstract ValueTask ExecuteAsync(IConsole console);
    }

    [Command("FTP Test Connection")]
    public class FtpCommand : FTP
    {
        public FtpCommand(IDbFile dbFile) : base(dbFile)
        {
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
    public class FtpCommandDisplay : FTP
    {
        public FtpCommandDisplay(IDbFile dbFile) : base(dbFile)
        {
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

            var responseFilesTree = await _ftpClient.GetListItems();

            console.Output.WriteLine("Files on the server : \n");

            if (responseFilesTree.Data is not null)
            {
                foreach (var item in responseFilesTree.Data)
                {
                    console.Output.WriteLine($"- {item}");
                }
            }
            else
            {
                console.Output.WriteLine("There's not file on the server");
            }

            return;
        }
    }

    [Command("FTP UploadFile")]
    public class FtpCommandUploadFile : FTP
    {
        public FtpCommandUploadFile(IDbFile dbFile) : base(dbFile)
        {
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

            if(!response.Success)
            {
                await console.Error.WriteLineAsync($"Not register {Id}.");
                return;
            }

            _ftpClient = new Ftp(response.Data);
            _ftpClient.Connect();

            console.Output.Write("* Insert the local path :");
            var localPath = console.Input.ReadLine();
 
            console.Output.Write("* Insert the remote path :");
            var remotePath = console.Input.ReadLine();

            console.Output.WriteLine("Uploading File....");

            var responseUploadFile = await _ftpClient.UploadFile(localPath, remotePath);

            if (!responseUploadFile.Success)
            {
                console.ShowError(responseUploadFile.Error, responseUploadFile.ValidationErrors);
                return;
            }
          
            console.Output.WriteLine(responseUploadFile.Data);
            
            console.Output.WriteLine("Hit a key to return to the menu.");
            console.Input.ReadLine();
          }
    }
}