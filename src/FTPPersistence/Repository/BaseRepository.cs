using CORE.Application.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FTPPersistence.Repository
{
    public class BaseRepository<TEntity> : IAsyncRepository<TEntity> where TEntity : class
    {
        protected readonly FtpDbContext FtpDbContext;

        public BaseRepository(FtpDbContext ftpDbContext)
        {
            FtpDbContext = ftpDbContext;
        }

        public async Task<TEntity> GetByIdAsync(Guid id)
        {
            return await FtpDbContext.Set<TEntity>().FindAsync(id);
        }

        public async Task<IReadOnlyList<TEntity>> ListAllAsync()
        {
            return await FtpDbContext.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await FtpDbContext.Set<TEntity>().AddAsync(entity);
            await FtpDbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            FtpDbContext.Entry(entity).State = EntityState.Modified;
            await FtpDbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<TEntity> DeleteAsync(TEntity entity)
        {
            FtpDbContext.Set<TEntity>().Remove(entity);

            await FtpDbContext.SaveChangesAsync();

            return entity;
        }
    }
}