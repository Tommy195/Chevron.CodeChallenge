using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using Chevron.CodeChallenge.Models;
using Chevron.CodeChallege.Api.Helpers;
using System;
using System.Collections.Generic;
using ServiceStack.Configuration;

namespace Chevron.CodeChallenge.Api.Authorization
{

    public static class JwtUtils
    {
        public static string GenerateToken(UserDTO user)
        {
            var appsettings = CodeChallege.Api.Helpers.AppSettings.Secret;

            var tokenHandler = new JwtSecurityTokenHandler();

            var secretKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(appsettings));

            var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: JwtSettings.Issuer,
                audience: JwtSettings.Audience,
                claims: new List<Claim> {
                          new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                          new Claim(ClaimTypes.Name, user.UserName.ToString()),
                          new Claim(ClaimTypes.Email, user.Email.ToString()),
                },
                expires: DateTime.UtcNow.AddMinutes(JwtSettings.MinutesToExpiration),
                signingCredentials: signingCredentials
            );

            return tokenHandler.WriteToken(token);
        }

        public static Token ValidateToken(string token)
        {
            var appsettings = CodeChallege.Api.Helpers.AppSettings.Secret;

            if (token == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(appsettings);

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key),
                    RequireSignedTokens = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out Microsoft.IdentityModel.Tokens.SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = new Guid(jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);

                var tokenToReturn = new Token();
                tokenToReturn.Id = userId;
                tokenToReturn.NeedsRefresh = false;

                return tokenToReturn;
            }
            catch (Exception ex)
            {
                var exceptionMessage = ex.Message;

                var exceptionMessageSplitted = exceptionMessage.Split('V');

                if (exceptionMessageSplitted[0] == "IDX10223: Lifetime validation failed. The token is expired. ")
                {
                    var jtoken = tokenHandler.ReadJwtToken(token);

                    var userId = new Guid(jtoken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);

                    if (userId != null && jtoken != null)
                    {
                        var tokenToReturn = new Token();
                        tokenToReturn.Id = userId;
                        tokenToReturn.NeedsRefresh = true;

                        return tokenToReturn;
                    }
                }

                return null;
            }
        }
    }
}
