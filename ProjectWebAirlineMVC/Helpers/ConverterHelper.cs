﻿using ProjectWebAirlineMVC.Data.Entities;
using ProjectWebAirlineMVC.Data.Interfaces;
using ProjectWebAirlineMVC.Models;
using System;
using System.Threading.Tasks;

namespace ProjectWebAirlineMVC.Helpers
{
    public class ConverterHelper : IConverterHelper
    {

        private readonly IAircraftRepository _aircraftRepository;
        private readonly IUserHelper _userHelper;

        public ConverterHelper(IAircraftRepository aircraftRepository, IUserHelper userHelper)
        {
            _aircraftRepository = aircraftRepository;
            _userHelper = userHelper;
        }

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

        public Country ToCountry(CountryViewModel model, Guid imageId, bool isNew)
        {
            return new Country
            {
                Id = isNew ? 0 : model.Id,
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

        public Flight ToFlight(FlightViewModel model, bool isNew)
        {
            return new Flight
            {
                Id = isNew ? 0 : model.Id,
                Date = model.Date,
                OriginCountryId = model.OriginCountryId,
                DestinationCountryId = model.DestinationCountryId,
                AircraftId = model.AircraftId,
                FlightNumber = model.FlightNumber,
                User = model.User
            };
        }

        public FlightViewModel ToFlightViewModel(Flight flight)
        {

            return new FlightViewModel
            {
                Id = flight.Id,
                Date = flight.Date,
                OriginCountryId = flight.OriginCountryId,
                DestinationCountryId = flight.DestinationCountryId,
                AircraftId = flight.AircraftId,
                FlightNumber = flight.FlightNumber,
                User = flight.User
            };
        }

    }
}
