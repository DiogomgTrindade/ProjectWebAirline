using System.ComponentModel.DataAnnotations;

namespace ProjectWebAirlineMVC.Models
{
    public class RecoverPasswordViewModel
    {

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
