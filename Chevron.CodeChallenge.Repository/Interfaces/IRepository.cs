using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Chevron.CodeChallenge.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void Add(TEntity obj);
        Task<TEntity> GetById(Guid id);
        Task<IEnumerable<TEntity>> GetAll();
        void Update(TEntity obj);
        void Remove(Guid id);
    }
}
