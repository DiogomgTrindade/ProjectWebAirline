using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectWebAirlineMVC.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectWebAirlineMVC.Data
{
    public interface ICountryRepository : IGenericRepository<Country>
    {

        IEnumerable<SelectListItem> GetComboCountries();

        Task<List<Country>> GetCountryListAsync();

    }
}
