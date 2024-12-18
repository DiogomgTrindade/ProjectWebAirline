﻿using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectWebAirlineMVC.Data.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectWebAirlineMVC.Data.Interfaces
{
    public interface IFlightRepository : IGenericRepository<Flight>
    {

        Task<IEnumerable<Flight>> GetAllFlightsWithCountriesAsync();

        public IQueryable GetAllWithCountriesAndAircrafts();

        Task<IEnumerable<Flight>> GetAllFlightsWithCountriesAndAircraftAsync();

        Task<bool> CheckIfAircraftIsBookedForDateAsync(int aicraftId, DateTime date, int? flightId = null);

    }
}

