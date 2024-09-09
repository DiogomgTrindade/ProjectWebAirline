using System.ComponentModel.DataAnnotations;

namespace ProjectWebAirlineMVC.Models
{
    public class LoginViewModel
    {

        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }


        public bool RememberMe { get; set; }
    }
}
