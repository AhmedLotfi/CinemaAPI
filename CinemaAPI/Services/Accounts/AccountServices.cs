using AuthenticationPlugin;
using CinemaAPI.Data;
using CinemaAPI.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CinemaAPI.Services.Accounts
{
    public class AccountServices : IAccountServices
    {
        private readonly IConfiguration _configuration;
        private readonly CinemaDbContext _cinemaDbContext;
        private readonly AuthService _auth;

        public AccountServices(IConfiguration configuration, CinemaDbContext cinemaDbContext)
        {
            _configuration = configuration;
            _cinemaDbContext = cinemaDbContext;
            _auth = new AuthService(_configuration);
        }

        public async Task<(bool, string, object)> Login(UserLoginDto userLoginDto)
        {
            try
            {
                var userExists = await _cinemaDbContext.User.Where(user => user.Email.Equals(userLoginDto.Email)).SingleOrDefaultAsync();

                if (userExists == null) return (false, "User Not Found !!", null);

                if (!SecurePasswordHasherHelper.Verify(userLoginDto.Password, userExists.Password))
                    return (false, "UnAuthenticated User !!", null);

                var claims = new[] {
                               new Claim(JwtRegisteredClaimNames.Email, userExists.Email),
                               new Claim(ClaimTypes.Email, userExists.Email),
                               new Claim(ClaimTypes.Role , userExists.Role)
                             };

                var token = _auth.GenerateAccessToken(claims);

                var result = new
                {
                    access_token = token.AccessToken,
                    expires_in = token.ExpiresIn,
                    token_type = token.TokenType,
                    creation_Time = token.ValidFrom,
                    expiration_Time = token.ValidTo,
                    user_id = userExists.Id
                };

                return (true, string.Empty, result);
            }
            catch (Exception x)
            {
                return (false, x.InnerException?.Message ?? x.Message, null);
            }
        }
    }
}
