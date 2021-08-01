using System;
using System.Threading.Tasks;
using CliFx;

namespace CommandFtpApp
{
    class Program
    {
        public static async Task<int> Main() =>
        await new CliApplicationBuilder()
        .AddCommandsFromThisAssembly()
        .Build()
        .RunAsync();
    }
}
