using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ProjectWebAirlineMVC.Models
{
    public class ChangeUserRoleViewModel
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string CurrentRole { get; set; }
        public List<SelectListItem> Roles { get; set; }
    }
}
