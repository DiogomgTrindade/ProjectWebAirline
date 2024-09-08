using Microsoft.AspNetCore.Identity;
using ProjectWebAirlineMVC.Data.Entities;
using System.Threading.Tasks;

namespace ProjectWebAirlineMVC.Helpers
{
    public interface IUserHelper
    {
        Task<User> GetUserByEmailAsync(string email);

        Task<IdentityResult> AddUserAsync (User user, string password);


    }
}
