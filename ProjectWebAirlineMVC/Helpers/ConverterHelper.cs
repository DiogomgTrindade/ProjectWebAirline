using ProjectWebAirlineMVC.Data.Entities;
using ProjectWebAirlineMVC.Models;

namespace ProjectWebAirlineMVC.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        public Aircraft ToAircraft(AircraftViewModel model, string path, bool isNew)
        {
            return new Aircraft
            {
                Id = isNew ? 0 : model.Id,
                ImageUrl = path,
                Name = model.Name,
                Capacity = model.Capacity,
                User = model.User,
            };
        }

        public AircraftViewModel ToAircraftViewModel(Aircraft aircraft)
        {
            return new AircraftViewModel
            {
                Id = aircraft.Id,
                ImageUrl = aircraft.ImageUrl,
                Name = aircraft.Name,
                Capacity = aircraft.Capacity,
                User = aircraft.User,
            };
        }
    }
}
