using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectWebAirlineMVC.Data.Entities;
using ProjectWebAirlineMVC.Data.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectWebAirlineMVC.Data.Repositories
{
    public class CountryRepository : GenericRepository<Country>, ICountryRepository
    {
        private readonly DataContext _context;

        public CountryRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> CountryNameExistsAsync(string name, int? id = null)
        {
            return await _context.Countries
                         .AnyAsync(c => c.Name.ToLower() == name.ToLower() && (!id.HasValue || c.Id != id.Value));
        }

        public IEnumerable<SelectListItem> GetComboCountries()
        {
            var list = _context.Countries.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            }).OrderBy(l => l.Text).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "Select a country...",
                Value = "0"
            });

            return list;
        }


        public Task<List<Country>> GetCountryListAsync()
        {
            return _context.Countries.ToListAsync();
        }
    }
}
