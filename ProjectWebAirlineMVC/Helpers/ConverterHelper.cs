using ProjectWebAirlineMVC.Data.Entities;
using ProjectWebAirlineMVC.Models;
using System;

namespace ProjectWebAirlineMVC.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        public Aircraft ToAircraft(AircraftViewModel model, Guid imageId, bool isNew)
        {
            return new Aircraft
            {
                Id = isNew ? 0 : model.Id,
                ImageId = imageId,
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
                ImageId = aircraft.ImageId,
                Name = aircraft.Name,
                Capacity = aircraft.Capacity,
                User = aircraft.User,
            };
        }

        public Country ToCountry(CountryViewModel model, Guid imageId, bool isNews)
        {
            return new Country
            {
                Id = isNews ? 0 : model.Id,
                ImageId = imageId,
                Name = model.Name
            };
        }

        public CountryViewModel ToCountryViewModel(Country country)
        {
            return new CountryViewModel
            {
                Id = country.Id,
                ImageId = country.ImageId,
                Name = country.Name,
            };
        }
    }
}
