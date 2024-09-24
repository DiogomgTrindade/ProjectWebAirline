using ProjectWebAirlineMVC.Data.Entities;
using ProjectWebAirlineMVC.Models;
using System;
using System.Threading.Tasks;

namespace ProjectWebAirlineMVC.Helpers
{
    public interface IConverterHelper
    {

        Aircraft ToAircraft(AircraftViewModel model, Guid imageId, bool isNew);

        AircraftViewModel ToAircraftViewModel(Aircraft aircraft);

        Country ToCountry (CountryViewModel model, Guid imageId, bool isNews);

        CountryViewModel ToCountryViewModel(Country country);

        Flight ToFlightAsync (FlightViewModel model);

        FlightViewModel ToFlightViewModel(Flight flight);

    }
}
