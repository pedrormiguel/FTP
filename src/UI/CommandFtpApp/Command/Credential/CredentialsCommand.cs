using System.Threading.Tasks;
using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;

namespace CommandFtpApp.Command.Credential
{
    [Command("Credentials")]
    public class CredentialsCommand : ICommand
    {
        public ValueTask ExecuteAsync(IConsole console)
        {
            console.Output.WriteLine("Working");
            return default;
        }
    }
}