using Chevron.CodeChallenge.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using System;
using System.Linq;
using Chevron.CodeChallenge.Service.Interfaces;
using Chevron.CodeChallenge.Interfaces;
using Chevron.CodeChallenge.UnitOfWork;
using ServiceStack.Auth;
using Chevron.CodeChallege.Repository.Helpers;
using Microsoft.Extensions.Configuration;
using Chevron.CodeChallenge.Api.Authorization;
using Chevron.CodeChallege.Api.Helpers;

namespace Chevron.CodeChallenge.Service.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async void Update(Guid id, UserDTO value)
        {
            var user = await _userRepository.GetById(id);
            var users = await _userRepository.GetAll();

            if (value.UserName != user.UserName && users.Any(x => x.Email == value.Email))
                throw new Exception("Username '" + value.UserName + "' is already created");

            _userRepository.Update(user);
            await _unitOfWork.Commit();
        }

        public async Task<UserDTO> Authenticate(string email, JwtSecurityToken token)
        {
            var id = token.Claims.First(c => c.Type == "id").Value.ToString();

            var userResponse = new UserDTO();

            if (!String.IsNullOrEmpty(id))
            {
                var user = await _userRepository.GetById(new Guid(id));

                if (user != null)
                {
                    userResponse.UserName = user.UserName;
                    userResponse.Token = user.Token;
                    userResponse.Id = user.Id;
                    userResponse.Role = user.Role;
                    userResponse.Email = user.Email;

                    return userResponse;
                }
            }

            var userName = token.Claims.First(c => c.Type == "name").Value;

            var newUser = new User(userName, email, null);

            _userRepository.Add(newUser);

            await _unitOfWork.Commit();

            var createdUser = await _userRepository.GetById(newUser.Id);
            
            userResponse.UserName = createdUser.UserName;
            userResponse.Email = createdUser.Email;
            userResponse.Token = createdUser.Token;
            userResponse.Id = createdUser.Id;
            userResponse.Role = createdUser.Role;

            createdUser.Token = Api.Authorization.JwtUtils.GenerateToken(userResponse);

            userResponse.Token = createdUser.Token;

            _userRepository.Update(createdUser);

            await _unitOfWork.Commit();

            return userResponse;
        }

        public async Task<UserDTO> GetByIdToLogin(Guid id)
        {
            var user = await _userRepository.GetById(id);

            var userResponse = new UserDTO();
            userResponse.UserName = user.UserName;
            userResponse.Token = user.Token;
            userResponse.Id = user.Id;
            userResponse.Role = user.Role;

            return userResponse;
        }
    }
}