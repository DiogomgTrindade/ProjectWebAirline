using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProjectWebAirlineMVC.Data.Entities;
using ProjectWebAirlineMVC.Data.Interfaces;
using ProjectWebAirlineMVC.Helpers;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWebAirlineMVC.Controllers.API
{
    public class AuthService : IAuthService
    {
        private readonly IUserHelper _userHelper;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;

        public AuthService(IUserHelper userHelper, UserManager<User> userManager, IConfiguration configuration)
        {
            _userHelper = userHelper;
            _userManager = userManager;
            _configuration = configuration;
        }


        public async Task<string> AuthenticateAsync(string username, string password)
        {
            var user = await _userHelper.GetUserByEmailAsync(username);
            if (user == null)
            {
                return null;
            }

            var result = await _userHelper.ValidatePasswordAsync(user, password);
            if (!result.Succeeded)
            {
                return null;
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
               issuer: _configuration["Tokens:Issuer"],
               audience: _configuration["Tokens:Audience"],
               claims: claims,
               expires: DateTime.UtcNow.AddDays(7),
               signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<bool> RegisterAsync(string username, string password, string firstName, string lastName)
        {
            var user = await _userHelper.GetUserByEmailAsync(username);
            if (user != null)
            {
                return false;
            }

            user = new User
            {
                UserName = username,
                Email = username,
                FirstName = firstName,
                LastName = lastName,
            };

            var result = await _userHelper.AddUserAsync(user, password);
            if (!result.Succeeded)
            {
                return false;
            }

            await _userHelper.AddUserToRoleAsync(user, "Customer");

            return true;
        }

        
    }
}
