using Microsoft.AspNetCore.Http;
using ProjectWebAirlineMVC.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace ProjectWebAirlineMVC.Models
{
    public class CountryViewModel : Country
    {
        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }
    }
}
