using Chevron.CodeChallege.Context;
using Chevron.CodeChallenge.Interfaces;
using Chevron.CodeChallenge.Models;

namespace Chevron.CodeChallenge.Implementations
{
    public class BoxerRepository : BaseRepository<Boxer>, IBoxerRepository
    {
        public BoxerRepository(IMongoDatabaseContext context) : base(context)
        {
        }
    }
}
