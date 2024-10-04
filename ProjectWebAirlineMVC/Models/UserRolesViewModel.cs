using System.Collections.Generic;

namespace ProjectWebAirlineMVC.Models
{
    public class UserRolesViewModel
    {
        public string UserId { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string CurrentRole { get; set; }

        public List<string> AvailableRoles { get; set; }
    }
}
