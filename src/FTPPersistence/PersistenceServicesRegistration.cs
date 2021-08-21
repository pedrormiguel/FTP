using CORE.Application.Contracts.Persistence;
using FTPPersistence.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FTPPersistence
{
    public static class PersistenceServicesRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<FtpDbContext>(
                opt => {
                    opt.UseSqlServer(configuration.GetConnectionString("FTPStringConnection"));
                });

            services.AddScoped(typeof(IAsyncRepository<>), typeof(BaseRepository<>));
            
            return services;
        }
    }
}