using ProjectWebAirlineMVC.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectWebAirlineMVC.Data
{
    public interface IRepository
    {
        void AddAircraft(Aircraft aircraft);

        Aircraft GetAircraft(int id);

        IEnumerable<Aircraft> GetAircrafts();

        bool AircraftExists(int id);

        void RemoveAircraft(Aircraft aircraft);

        Task<bool> SaveAllAsync();

        void UpdateAircraft(Aircraft aircraft);


    }
}
