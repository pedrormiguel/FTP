﻿using CliFx;
using CliFx.Infrastructure;
using CommandFtpApp.Command.Credential;
using FTPPersistence.Interfaces;
using FTPPersistence.Repository;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace FTPCommandAppTest.Commands
{
    public class FtpCommandCredentialAdd
    {
        private readonly CliApplicationBuilder _app;
        private readonly string _pathOfFile;
        private readonly string _pathOfDirectory;

        public FtpCommandCredentialAdd()
        {
            var servicesCollection = new ServiceCollection();
            servicesCollection.AddTransient<IDbFile, DbFileHandler>();
            servicesCollection.AddTransient<CredentialsCommandAdd>();

            var provider = servicesCollection.BuildServiceProvider();

            _pathOfDirectory = provider.GetService<IDbFile>()?.GetPathDirectory();
            _pathOfFile = provider.GetService<IDbFile>()?.GetPathFile();


            _app = new CliApplicationBuilder()
                .AddCommand<CredentialsCommandAdd>()
                .UseTypeActivator(provider.GetService);
        }

        [Fact]
        public async Task Should_Add_Credential_Successfully()
        {
            // Arrange
            using var console = new FakeInMemoryConsole();
            var command = new[] { "Credentials Add", "-s", "Test2", "--User", "TEU2", "--Password", "*****" };
            var envVars = new Dictionary<string, string>();

            // Act
            await _app.UseConsole(console).Build().RunAsync(command, envVars);
            var stdOut = console.ReadOutputString();

            // Assert
            stdOut.ShouldBe("Adding credentials\nStatus of the request : True\n");
            stdOut.ShouldContain("True");
            ValidateAndCleanFile.CleanFile(_pathOfFile, _pathOfDirectory).ShouldBeTrue();

        }

        [Fact]
        public async Task Should_Add_Fail_WhenMissingTheServerParameters()
        {
            // Arrange
            using var console = new FakeInMemoryConsole();
            var command = new[] { "Credentials Add" };
            var envVars = new Dictionary<string, string>();

            // Act
            await _app.UseConsole(console).Build().RunAsync(command, envVars);
            var stdOut = console.ReadOutputString();

            var requirements = "* -s|--Server       Url of the FTP Server. \n* -u|--User         Name of user credential. \n* -p|--Password     Password of user credential. \n  -I|--ID           ID of the credential. \n  --Port            Port of the server. Default: \"21\".\n  -h|--help         Shows help text. ";

            // Assert
            stdOut.ShouldContain(requirements);
            ValidateAndCleanFile.CleanFile(_pathOfFile, _pathOfDirectory).ShouldBeTrue();

        }
    }
}
