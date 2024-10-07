using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectWebAirlineMVC.Data.Entities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectWebAirlineMVC.Data.Interfaces
{
    public interface IFlightRepository : IGenericRepository<Flight>
    {
        public IQueryable GetAllWithUsersAndAircrafts();


        Task<Flight> GetByIdWithUsersAndAircraftsAsync(int id);

        Task<IEnumerable<Flight>> GetAllFlightsWithCountriesAsync();

        Task<Flight> GetByIdWithCountries(int id);


        public IQueryable GetAllWithCountriesAndAircrafts();

        Task<IEnumerable<Flight>> GetAllFlightsWithCountriesAndAircraftAsync();


    }
}

