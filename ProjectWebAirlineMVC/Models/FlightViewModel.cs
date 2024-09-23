using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectWebAirlineMVC.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectWebAirlineMVC.Models
{
    public class FlightViewModel
    {
        public int Id { get; set; }



        public IEnumerable<SelectListItem> Countries { get; set; }


        [Required]
        [Display(Name = "Origin Country")]
        public int OriginCountryId { get; set; }

        public string OriginCountryName { get; set; }



        [Required]
        [Display(Name = "Destination Country")]
        public int DestinationCountryId { get; set; }

        public string DestinationCountryName { get; set; }


        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}", ApplyFormatInEditMode = false)]
        public DateTime Date { get; set; }



        public User User { get; set; }
    }
}
