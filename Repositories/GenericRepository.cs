using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Models;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseModel
    {
        SouqElgomlaContext Context;
        DbSet<T> Table;

        public GenericRepository(SouqElgomlaContext context)
        {
            Context = context;
            Table = context.Set<T>();
        }
        public async Task<T> Add(T entity)
        {
            await Table.AddAsync(entity);
            return entity;
        }

        public async Task<IEnumerable<T>> GetAsync()
        {
            return Table;
        }

        public async Task<T> GetByIDAsync(int Id)
        {
            return Table.Find(Id);
        }

        public async Task<T> Remove(T entity)
        {
            Table.Remove(entity);
            return entity;
        }

        public async Task<T> Update(T entity)
        {
            Table.Update(entity);
            return entity;
        }

        public async Task<T> UpdatePatch(int id, JsonPatchDocument entity)
        {
            T temp = Table.Find(id);
            entity.ApplyTo(temp);
            return temp;
        }
    }
}
