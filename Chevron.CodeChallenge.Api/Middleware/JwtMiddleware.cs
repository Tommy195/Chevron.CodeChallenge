
using Chevron.CodeChallenge.Models;
using Chevron.CodeChallenge.Service.Interfaces;
using Chevron.CodeChallege.Api.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Linq;
using Chevron.CodeChallenge.Api.Authorization;

namespace Chevron.CodeChallenge.Api.Middleware
{

    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private IConfiguration _configuration;

        public JwtMiddleware(RequestDelegate next, IConfiguration configuration )
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task Invoke(HttpContext context, IUserService userService)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var returnedToken = JwtUtils.ValidateToken(token);

            if (returnedToken != null)
            {
                if (returnedToken.Id.HasValue && returnedToken.NeedsRefresh == false)
                {
                    // attach user to context on successful jwt validation
                    context.Items["User"] = await userService.GetByIdToLogin(returnedToken.Id.Value);
                }
                else if (returnedToken.Id.HasValue && returnedToken.NeedsRefresh == true)
                {
                    var user = await userService.GetByIdToLogin(returnedToken.Id.Value);
                    if (user != null && returnedToken.NeedsRefresh == true)
                    {
                        var newToken = JwtUtils.GenerateToken(user);

                        user.Token = newToken;

                        var request = new UserDTO();
                        user.UserName = request.UserName;
                        user.Email = request.Email;
                        user.Token = request.Token;

                        userService.Update(user.Id, request);

                        user = await userService.GetByIdToLogin(returnedToken.Id.Value);

                        context.Items["User"] = user;
                    }
                }

            }

            await _next(context);
        }
    }
}
