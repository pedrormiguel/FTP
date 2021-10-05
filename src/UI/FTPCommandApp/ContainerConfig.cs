using CommandFtpApp.Command.Credential;
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
            servicesCollection.AddTransient<CredentialsCommandAdd>();
            servicesCollection.AddTransient<CredentialsCommandList>();
            servicesCollection.AddTransient<CredentialsCommandDelete>();
            servicesCollection.AddTransient<CredentialCommandUpdate>();

            return servicesCollection.BuildServiceProvider();
        }
    }
}
