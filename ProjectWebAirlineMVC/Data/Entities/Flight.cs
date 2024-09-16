using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectWebAirlineMVC.Data.Entities
{
    public class Flight : IEntity
    {
        public int Id {  get; set; }



        [Required]
        public Country Origin { get; set; }


        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}", ApplyFormatInEditMode = false)]
        public DateTime Date { get; set; }


        [Required]
        public Country Destination { get; set; }


        public ICollection<Country> Countries { get; set; }


        [Required]
        public User User { get; set; }

    }
}
