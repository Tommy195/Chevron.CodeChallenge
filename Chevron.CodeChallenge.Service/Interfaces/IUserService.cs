using Chevron.CodeChallenge.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using System;

namespace Chevron.CodeChallenge.Service.Interfaces
{
    public interface IUserService
    {
        Task<UserDTO> Authenticate(string email, JwtSecurityToken token);
        void Update(Guid id, UserDTO model);
        Task<UserDTO> GetByIdToLogin(Guid id);
    }
}
