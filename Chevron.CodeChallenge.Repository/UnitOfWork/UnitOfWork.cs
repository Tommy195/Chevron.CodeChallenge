using Chevron.CodeChallege.Context;
using System.Threading.Tasks;

namespace Chevron.CodeChallenge.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IMongoDatabaseContext _context;

        public UnitOfWork(IMongoDatabaseContext context)
        {
            _context = context;
        }

        public async Task<bool> Commit()
        {
            var changeAmount = await _context.SaveChanges();

            return changeAmount > 0;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
