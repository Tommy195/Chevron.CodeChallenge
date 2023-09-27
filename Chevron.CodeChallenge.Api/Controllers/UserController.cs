using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using System;
using System.Linq;
using Chevron.CodeChallenge.Api.Authorization;
using Chevron.CodeChallenge.Service.Interfaces;
using Chevron.CodeChallenge.Models;

namespace Chevron.CodeChallege.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> Get(Guid id)
        {
            try
            {
                var user = await _userService.GetByIdToLogin(id);

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest("Could not retrieve user");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLogin(TokenDTO jwt)
        {
            try
            {
                //This is meant to be for google authentication
                var token = new JwtSecurityToken(jwt.Token);
                var email = token.Claims.First(c => c.Type == "email").Value;

                if (token.ValidTo > DateTime.UtcNow)
                {
                    var userResponse = await _userService.Authenticate(email.ToString(), token);

                    return Ok(userResponse);
                }

                return Ok();
            }
            catch (Exception)
            {
                return Unauthorized();
            }
        }

    }
}
