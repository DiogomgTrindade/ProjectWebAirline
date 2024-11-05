using System.Globalization;
using System.Threading.Tasks;

namespace ProjectWebAirlineMVC.Data.Interfaces
{
    public interface IAuthService
    {
        Task <string> AuthenticateAsync(string username, string password);

        Task<bool> RegisterAsync(string username, string password, string firstName, string lastName);
    }
}
