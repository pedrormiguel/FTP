using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CORE.Application.Contracts.Persistence
{
    public interface IAsyncRepository<T> where T : class
    {
        Task<T> GetByIdAsync(Guid id);
    
        /// <summary>
        /// Get Event by Id
        /// </summary>
        /// <returns></returns>
        Task<IReadOnlyList<T>> ListAllAsync();

        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<T> DeleteAsync(T entity);
    }
}