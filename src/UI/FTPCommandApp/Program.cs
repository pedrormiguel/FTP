using System;
using System.Threading.Tasks;
using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;

namespace CommandFtpApp
{
    //[Command]
    //public class HelloWordCommand : ICommand
    //{
    //    [CommandOption("Subjt", 's', Description = "Subject")]
    //    public string Subject { get; set; }

    //    public async ValueTask ExecuteAsync(IConsole console)
    //    {
    //        await console.Output.WriteLineAsync($"Hello {Subject}");
    //    }
    //}

    //[Command]
    //public class HelloWordCommand2 : ICommand
    //{
    //    [CommandOption("aaaaa", 'a', Description = "aaaaa")]
    //    public string Subject { get; set; }

    //    public async ValueTask ExecuteAsync(IConsole console)
    //    {
    //        await console.Output.WriteLineAsync($"Hello aaa {Subject}");
    //    }
    //}

    class Program
    {
        public static async Task<int> Main(string[] args) =>
        await new CliApplicationBuilder()
        .AddCommandsFromThisAssembly()
        .Build()
        .RunAsync(args);
    }
}
