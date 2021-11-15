using CommandFtpApp.Command.Credential;
using CommandFtpApp.Command.Server;
using FTPLib.Class;
using FTPLib.Interfaces;
using FTPPersistence.Interfaces;
using FTPPersistence.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace CommandFtpApp
{
    public static class ContainerConfig
    {
        public static ServiceProvider Configure()
        {
            var servicesCollection = new ServiceCollection();
            
            servicesCollection.AddSingleton<IDbFile, DbFileHandler>();
            servicesCollection.AddSingleton<IFtp, Ftp>();
            servicesCollection.AddTransient<CredentialsCommandAdd>();
            servicesCollection.AddTransient<CredentialsCommandList>();
            servicesCollection.AddTransient<CredentialsCommandDelete>();
            servicesCollection.AddTransient<CredentialCommandUpdate>();
            servicesCollection.AddTransient<FtpCommand>();
            servicesCollection.AddTransient<FtpCommandDisplay>();

            return servicesCollection.BuildServiceProvider();
        }
    }
}
