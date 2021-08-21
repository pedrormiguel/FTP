using FTPLib.Class.Entities;
using Microsoft.EntityFrameworkCore;

namespace FTPPersistence
{
    public class FtpDbContext : DbContext
    {
        public FtpDbContext(DbContextOptions<FtpDbContext> options) 
            : base(options)
        {
        }

        public DbSet<Credential> Credential { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //It search for all the EntityConfiguration inside FTPDbContext [ Configuration Folder ]
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(FtpDbContext).Assembly);
        }
    }
}