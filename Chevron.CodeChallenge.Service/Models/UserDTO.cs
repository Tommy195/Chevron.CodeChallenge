using Chevron.CodeChallege.Repository.Helpers;
using System;

namespace Chevron.CodeChallenge.Models
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public Roles Role { get; set; }
        public string Token { get; set; }
    }
}
