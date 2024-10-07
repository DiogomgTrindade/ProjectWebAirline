using ProjectWebAirlineMVC.Data.Entities;
using ProjectWebAirlineMVC.Data.Interfaces;
using System.Threading.Tasks;

namespace ProjectWebAirlineMVC.Data.Repositories
{
    public interface ITicketRepository : IGenericRepository<Tickets>
    {
        Task GenerateTicketsAsync(Flight flight);
    }
}
