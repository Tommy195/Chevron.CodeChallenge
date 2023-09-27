using System.Threading.Tasks;
using System;

namespace Chevron.CodeChallenge.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        Task<bool> Commit();
    }
}
