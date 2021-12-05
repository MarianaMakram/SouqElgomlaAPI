using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IGenericRepository<T>
    {
        Task<IEnumerable<T>> GetAsync();
        Task<T> GetByIDAsync(int Id);
        Task<T> Add(T entity);
        Task<T> Update(T entity);
        Task<T> Remove(T entity);
        Task<T> UpdatePatch(int id, JsonPatchDocument entity);
    }
}
