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


        [Display(Name = "Flight Number")]
        public int FlightNumber { get; set; }

        public IEnumerable<SelectListItem> Countries { get; set; }

        public IEnumerable<SelectListItem> Aircrafts { get; set; }



        [Required]
        [Display(Name = "Origin Country")]
        public int OriginCountryId { get; set; }

        public string OriginCountryName { get; set; }



        [Required]
        [Display(Name = "Destination Country")]
        public int DestinationCountryId { get; set; }

        public string DestinationCountryName { get; set; }


        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; } = DateTime.Now;



        [Display(Name = "Aircraft")]
        public int AircraftId { get; set; }
        public Aircraft Aircraft { get; set; }



        public User User { get; set; }
    }
}
