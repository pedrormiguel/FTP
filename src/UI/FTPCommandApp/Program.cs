using System.Threading.Tasks;
using Autofac;
using CliFx;

namespace CommandFtpApp
{
    static class Program
    {
        public static IContainer Container { get; set; }
        public static async Task<int> Main(string[] args)
        {
            Container = ContainerConfig.Configure();

            return await new CliApplicationBuilder()
            .AddCommandsFromThisAssembly()
            .Build()
            .RunAsync(args);
        }
    }
}
