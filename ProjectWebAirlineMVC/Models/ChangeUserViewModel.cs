using Microsoft.AspNetCore.Http;
using ProjectWebAirlineMVC.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace ProjectWebAirlineMVC.Models
{
    public class ChangeUserViewModel : User
    {
        [Display(Name = "Image")]
        public IFormFile ImageFile {  get; set; }


        [Required]
        [Display(Name ="First Name")]
        public string FirstName { get; set; }



        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
    }
}
