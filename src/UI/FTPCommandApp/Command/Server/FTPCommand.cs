using System;
using System.Threading.Tasks;
using FTPLib.Class;
using CliFx;
using CliFx.Attributes;
using CliFx.Exceptions;
using CliFx.Infrastructure;

namespace CommandFtpApp.Command
{
    [Command("FTP Connect", Description = "Connect to a server")]
    public class FTPCommand : ICommand
    {
        [CommandOption("Server", shortName: 's', IsRequired = true, Description = "Url of the FTP Server.")]
        public string FtpServer { get; init; }

        [CommandOption("User", shortName: 'u', IsRequired = true, Description = "Name of user credential.")]
        public string UserName { get; init; }

        [CommandOption("Password", shortName: 'p', IsRequired = true, Description = "Password of user credential.")]
        public string Password { get; init; }

        [CommandOption("Port",Description = "Port of the server.")]
        public string Port { get; init; } = "21";

        public  ValueTask ExecuteAsync(IConsole console)
        {
            var isNumber = int.TryParse(Port, out var intPort);

            if (!isNumber)
                throw new CommandException("Error Code : 0 \nThe port has to be a numeric value.", 0);

            var client = new Ftp(host: FtpServer, user: UserName, password: Password, port: intPort);
            var status = client.Connect();

            console.Output.WriteLine($"FTP : {client.IsConnected} - status {status.Data} -Error {status.Error}");
            console.Output.WriteLine();
            return default;
        }
    }

    [Command("FTP Display")]
    public class FTPCommandDisplay : ICommand
    {
        public ValueTask ExecuteAsync(IConsole console)
        {
            console.Output.WriteLine("Displaying");

            return default;
        }
    }
}