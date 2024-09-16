using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectWebAirlineMVC.Data.Entities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectWebAirlineMVC.Data
{
    public interface IFlightRepository : IGenericRepository<Flight>
    {
        //Task<IQueryable<Flight>> GetFlightAsync(int id);


        //Task<Flight> GetCountryAsync(Country country);



    }
}
