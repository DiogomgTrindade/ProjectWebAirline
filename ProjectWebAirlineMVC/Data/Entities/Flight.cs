using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectWebAirlineMVC.Data.Entities
{
    public class Flight : IEntity
    {
        public int Id {  get; set; }



        public int OriginCountryId { get; set; }
        public Country OriginCountry { get; set; }


        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}", ApplyFormatInEditMode = false)]
        public DateTime Date { get; set; }


        public int DestinationCountryId { get; set; }
        public Country DestinationCountry { get; set; }





        [Required]
        public User User { get; set; }

    }
}
