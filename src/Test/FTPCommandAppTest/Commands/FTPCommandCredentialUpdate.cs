using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CliFx;
using CliFx.Infrastructure;
using CommandFtpApp.Command.Credential;
using CORE.Domain.Common;
using CORE.Domain.Entities;
using FTPPersistence.Interfaces;
using FTPPersistence.Repository;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace FTPCommandAppTest.Commands
{
    public class FtpCommandCredentialUpdate
    {
        private readonly CliApplicationBuilder _app;
        private readonly ServiceProvider _provider;
        private readonly string _pathOfFile;
        private readonly string _pathOfDirectory;
        
        public FtpCommandCredentialUpdate()
        {
            var servicesCollection = new ServiceCollection();
            servicesCollection.AddTransient<IDbFile, DbFileHandler>();
            servicesCollection.AddTransient<CredentialCommandUpdate>();
            
            _provider = servicesCollection.BuildServiceProvider();
            
            _pathOfDirectory = _provider.GetService<IDbFile>()?.GetPathDirectory();
            _pathOfFile = _provider.GetService<IDbFile>()?.GetPathFile();
            
            _app = new CliApplicationBuilder()
                .AddCommand<CredentialCommandUpdate>()
                .UseTypeActivator(_provider.GetService);
        }

        [Fact]
        public async Task Should_Update_Credential_Success()
        {
            // Arrange
            using var console = new FakeInMemoryConsole();
            var envVars = new Dictionary<string, string>();
            var db = _provider.GetService<IDbFile>();
            var newValue = "CHANGE";
            string newPort = "22";
            
            var credentials = new Credential()
                { HostName = "UTEST", UserName = "UTEST", Password = "UTEST", Port = 21 };
            
            await db.Add(credentials);
            
            var command = new[] { "Credentials Update", "-I", $"{credentials.Id}", "--User", newValue, "--Server", newValue, "--Password", newValue, "--Port", newPort };

            // Act
            await _app.UseConsole(console).Build().RunAsync(command, envVars);
            var stdOut = console.ReadOutputString();
            var items = await db.ReadAll();
            var selected = items.Data.FirstOrDefault(x => x.Contains(credentials.Id.ToString()));
            var dto = DtoConnectionSever.Map(selected);
            
            // Assert
            ValidateAndCleanFile.CleanFile(_pathOfFile,_pathOfDirectory).ShouldBeTrue();
            dto.HostName.ShouldBe(newValue);
            dto.UserName.ShouldBe(newValue);
            dto.Password.ShouldBe(newValue);
            dto.Port.ToString().ShouldBe(newPort);
        } 
    }
}