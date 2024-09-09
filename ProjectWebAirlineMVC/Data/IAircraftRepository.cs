using ProjectWebAirlineMVC.Data.Entities;
using System.Linq;

namespace ProjectWebAirlineMVC.Data
{
    public interface IAircraftRepository : IGenericRepository<Aircraft>
    {

        public IQueryable GetAllWithUsers();

    }
}
