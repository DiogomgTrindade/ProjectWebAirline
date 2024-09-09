using System.ComponentModel.DataAnnotations;

namespace ProjectWebAirlineMVC.Data.Entities
{
    public class Aircraft : IEntity
    {

        public int Id { get; set; }



        [Required]
        public string Name { get; set; }



        [Display(Name = "Image")]
        public string ImageUrl { get; set; }




        [Required]
        public int Capacity { get; set; }


        public User User { get; set; }


        public string ImageFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(ImageFullPath))
                {
                    return null;
                }
                return $"https://localhost:44330/{ImageUrl.Substring(1)}";
            }

        }



    }
}
