using Chevron.CodeChallege.Repository.Helpers;
using System;


namespace Chevron.CodeChallenge.Models
{
    public class User
    {
        public User(string username, string email, string token)
        {
            UserName = username;
            Email = email;
            Token = token;
            Id = Guid.NewGuid();
            Role = Roles.Admin;
        }

        public User(Guid id, string username, string email, string token, Roles role)
        {
            UserName = username;
            Email = email;
            Token = token;
            Id = id;
            Role = role;
        }

        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public Roles Role { get; set; }
    }
}
