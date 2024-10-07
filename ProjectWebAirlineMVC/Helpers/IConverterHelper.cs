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

        Country ToCountry (CountryViewModel model, Guid imageId, bool isNew);

        CountryViewModel ToCountryViewModel(Country country);

        Flight ToFlight (FlightViewModel model, bool isNew);

        FlightViewModel ToFlightViewModel(Flight flight);

    }
}
