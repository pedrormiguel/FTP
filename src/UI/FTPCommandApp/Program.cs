using System.Threading.Tasks;
using CliFx;
using FTPPersistence.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace CommandFtpApp
{
    class Program
    {
        public static async Task<int> Main(string[] args)
        {
            var services = new ServiceCollection();

            services.AddTransient<DbFileHandler>();

            var servicesProvider = services.BuildServiceProvider();

            return await new CliApplicationBuilder()
            .AddCommandsFromThisAssembly()
            .UseTypeActivator(servicesProvider.GetService)
            .Build()
            .RunAsync(args);
        }
    }
}
