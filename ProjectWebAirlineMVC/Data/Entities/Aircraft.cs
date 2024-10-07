using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectWebAirlineMVC.Data.Entities
{
    public class Aircraft : IEntity
    {

        public int Id { get; set; }



        [Required]
        public string Name { get; set; }



        [Display(Name = "Image")]
        public Guid ImageId{ get; set; }




        [Required]
        public int Capacity { get; set; }


        public User User { get; set; }


        public string ImageFullPath => ImageId == Guid.Empty
            ? $"https://supershopdmgt.azurewebsites.net/images/noimage.png"
            : $"https://supershopdmgt.blob.core.windows.net/aircrafts/{ImageId}";




    }
}
