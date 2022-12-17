using CliFx;
using System.Threading.Tasks;

namespace CommandFtpApp
{
    static class Program
    {
        public static async Task<int> Main(string[] args)
        {
            var servicesProvider = ContainerConfig.Configure();

            return await new CliApplicationBuilder()
            .AddCommandsFromThisAssembly()
            .UseTypeActivator(servicesProvider.GetService)
            .Build()
            .RunAsync(args);
        }
    }
}
