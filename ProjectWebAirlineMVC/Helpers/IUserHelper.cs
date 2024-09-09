using Microsoft.AspNetCore.Identity;
using ProjectWebAirlineMVC.Data.Entities;
using ProjectWebAirlineMVC.Models;
using System.Threading.Tasks;

namespace ProjectWebAirlineMVC.Helpers
{
    public interface IUserHelper
    {
        Task<User> GetUserByEmailAsync(string email);

        Task<IdentityResult> AddUserAsync (User user, string password);


        Task<SignInResult> LoginAsync(LoginViewModel model);

        Task LogOutAsync();
    }
}
