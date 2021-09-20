using System.Collections.Generic;
using System.Threading.Tasks;
using CliFx;
using CliFx.Infrastructure;
using CommandFtpApp.Command.Credential;
using FTPPersistence.Interfaces;
using FTPPersistence.Repository;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace FTPCommandAppTest
{
    public class FtpCommandCredentialAdd
    {
        private readonly CliApplicationBuilder _app;

        public FtpCommandCredentialAdd()
        {
            var servicesCollection = new ServiceCollection();
            servicesCollection.AddTransient<IDbFile, DbFileHandler>();
            servicesCollection.AddTransient<CredentialsCommandAdd>();
            var provider = servicesCollection.BuildServiceProvider();
            
            _app = new CliApplicationBuilder()
                .AddCommand<CredentialsCommandAdd>()
                .UseTypeActivator(provider.GetService);
        }

        [Fact]
        public async Task Should_Add_Credential_Successfully()
        {
            // Arrange
            using var console = new FakeInMemoryConsole();
            var command = new[] {"Credentials Add","--Server","Test2","--User","TEU2","--Password","*****"};
            var envVars = new Dictionary<string, string>();

            // Act
            await _app.UseConsole(console).Build().RunAsync(command,envVars);
            var stdOut = console.ReadOutputString();

            // Assert
            stdOut.ShouldBe("Adding credentials\nStatus of the request : True\n");
        }
    }
}
