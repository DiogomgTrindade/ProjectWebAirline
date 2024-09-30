using ProjectWebAirlineMVC.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectWebAirlineMVC.Data.Interfaces
{
    public interface IAircraftRepository : IGenericRepository<Aircraft>
    {

        public IQueryable GetAllWithUsers();


        Task<List<Aircraft>> GetAircraftListAsync();

    }
}
