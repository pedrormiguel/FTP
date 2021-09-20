using System.Threading.Tasks;
using CliFx;
using CommandFtpApp.Command.Credential;
using FTPPersistence.Interfaces;
using FTPPersistence.Repository;
using Microsoft.Extensions.DependencyInjection;

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
