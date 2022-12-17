using CliFx;
using CliFx.Infrastructure;
using CommandFtpApp.Command.Credential;
using CORE.Domain.Entities;
using FTPPersistence.Interfaces;
using FTPPersistence.Repository;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace FTPCommandAppTest.Commands
{
    public class FtpCommandCredentialDelete
    {
        private readonly CliApplicationBuilder _app;
        private readonly ServiceProvider _provider;
        private readonly string _pathOfFile;
        private readonly string _pathOfDirectory;


        public FtpCommandCredentialDelete()
        {
            var servicesCollection = new ServiceCollection();
            servicesCollection.AddTransient<IDbFile, DbFileHandler>();
            servicesCollection.AddTransient<CredentialsCommandDelete>();
            _provider = servicesCollection.BuildServiceProvider();

            _pathOfDirectory = _provider.GetService<IDbFile>()?.GetPathDirectory();
            _pathOfFile = _provider.GetService<IDbFile>()?.GetPathFile();

            _app = new CliApplicationBuilder()
                .AddCommand<CredentialsCommandDelete>()
                .UseTypeActivator(_provider.GetService);
        }

        [Fact]
        public async Task Should_Delete_Successfully()
        {
            // Arrange
            var console = new FakeInMemoryConsole();
            var credential = new Credential { HostName = "HT", UserName = "UT", Password = "PT", Port = 24 };
            var db = _provider.GetRequiredService<IDbFile>();

            // Act
            await db.Add(credential);
            var args = new[] { "Credentials Delete", "-I", $"{credential.Id}" };

            await _app.UseConsole(console).Build().RunAsync(args);
            var stdOutput = console.ReadOutputString();

            // Assert
            stdOutput.ShouldBe($"Element deleted. {credential.HostName}\n");
            ValidateAndCleanFile.CleanFile(_pathOfFile, _pathOfDirectory).ShouldBeTrue();
        }

    }
}