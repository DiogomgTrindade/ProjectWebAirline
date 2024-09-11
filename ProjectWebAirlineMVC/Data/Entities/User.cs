using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System;

namespace ProjectWebAirlineMVC.Data.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }



        //[Display(Name = "Image")]
        //public Guid ImageId { get; set; }


        //public string ImageFullPath => ImageId == Guid.Empty
        //    ? $"https://supershopdmgt.azurewebsites.net/images/noimage.png"
        //    : $"https://supershopdmgt.blob.core.windows.net/users/{ImageId}";


    }
}
