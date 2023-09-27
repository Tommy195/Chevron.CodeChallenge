using System.Threading.Tasks;
using System;
using MongoDB.Driver;

namespace Chevron.CodeChallege.Context
{
    public interface IMongoDatabaseContext : IDisposable
    {
        void AddCommand(Func<Task> func);
        Task<int> SaveChanges();
        IMongoCollection<T> GetCollection<T>(string name);
    }
}
