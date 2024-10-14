using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectWebAirlineMVC.Data.Entities
{
    public class Country : IEntity
    {
        public int Id { get; set; }


        [Required]
        public string Name { get; set; }



        [Display(Name = "Image")]
        public Guid ImageId { get; set; }


        public string ImageFullPath => ImageId == Guid.Empty
            ? $"https://supershopdmgt.blob.core.windows.net/aircrafts/noImage.png"
            : $"https://supershopdmgt.blob.core.windows.net/countries/{ImageId}";


    }
}
