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


    }
}
