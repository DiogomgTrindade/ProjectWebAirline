using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectWebAirlineMVC.Data.Entities
{
    public class Flight : IEntity
    {
        public int Id {  get; set; }


        [Display(Name = "Flight Number")]
        public int FlightNumber { get; set; }



        [Display(Name ="Origin")]
        public int OriginCountryId { get; set; }


        [Display(Name = "Origin")]
        public Country OriginCountry { get; set; }


        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}", ApplyFormatInEditMode = false)]
        public DateTime Date { get; set; }


        [Display(Name = "Destination")]
        public int DestinationCountryId { get; set; }

        [Display(Name = "Destination")]
        public Country DestinationCountry { get; set; }


        [Display(Name ="Aircraft")]
        public int AircraftId { get; set; }
        public Aircraft Aircraft { get; set; }


        [Required]
        public User User { get; set; }

    }
}
