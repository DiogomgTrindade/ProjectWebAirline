using ProjectWebAirlineMVC.Data.Entities;
using ProjectWebAirlineMVC.Models;

namespace ProjectWebAirlineMVC.Helpers
{
    public interface IConverterHelper
    {

        Aircraft ToAircraft(AircraftViewModel model, string path, bool isNew);

        AircraftViewModel ToAircraftViewModel(Aircraft aircraft);
    }
}
