using Autofac;
using FTPPersistence.Interfaces;
using FTPPersistence.Repository;

namespace CommandFtpApp
{
    public class ContainerConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<DbFileHandler>().As<IDbFile>().SingleInstance();

            return builder.Build();
        }
    }
}
