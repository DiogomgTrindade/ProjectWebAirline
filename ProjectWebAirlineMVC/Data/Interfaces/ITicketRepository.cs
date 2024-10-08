using ProjectWebAirlineMVC.Data.Entities;
using System.Threading.Tasks;

namespace ProjectWebAirlineMVC.Data.Interfaces
{
    public interface ITicketRepository : IGenericRepository<Tickets>
    {
        Task GenerateTicketsAsync(Flight flight);

        Task RemoveTicketsFromFlightAsync(Flight flight);

        Task UpdateTicketsFromFlightAsync(Flight flight);
    }
}
