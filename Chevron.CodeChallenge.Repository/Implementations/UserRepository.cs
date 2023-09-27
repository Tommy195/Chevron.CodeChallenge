using Chevron.CodeChallege.Context;
using Chevron.CodeChallenge.Interfaces;
using Chevron.CodeChallenge.Models;

namespace Chevron.CodeChallenge.Implementations
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(IMongoDatabaseContext context) : base(context)
        {
        }
    }
}
