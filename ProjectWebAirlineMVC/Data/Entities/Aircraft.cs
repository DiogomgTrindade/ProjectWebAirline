using System.ComponentModel.DataAnnotations;

namespace ProjectWebAirlineMVC.Data.Entities
{
    public class Aircraft
    {

        public int Id { get; set; }

        public string Name { get; set; }



        [Display(Name ="Image")]
        public string ImageUrl { get; set; }




        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        public int Capacity { get; set; }

    }
}
