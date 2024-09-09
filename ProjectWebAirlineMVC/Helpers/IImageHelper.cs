using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ProjectWebAirlineMVC.Helpers
{
    public interface IImageHelper
    {

        Task<string> UploadImageAsync(IFormFile file, string folder);
    }
}
