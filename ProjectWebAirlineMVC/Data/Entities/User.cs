using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System;
using System.Security.Principal;
using System.Collections.Generic;

namespace ProjectWebAirlineMVC.Data.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public List<Tickets> TicketList { get; set; } = new List<Tickets>();

        //[Display(Name = "Image")]
        //public Guid ImageId { get; set; }


        //public string ImageFullPath => ImageId == Guid.Empty
        //    ? $"https://supershopdmgt.azurewebsites.net/images/noimage.png"
        //    : $"https://supershopdmgt.blob.core.windows.net/users/{ImageId}";


    }
}
