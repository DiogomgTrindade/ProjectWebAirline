using ProjectWebAirlineMVC.Data.Entities;
using System.Collections;
using System.Collections.Generic;

namespace ProjectWebAirlineMVC.Models
{
    public class TicketListViewModel
    {
        public Flight Flight { get; set; }

        public IEnumerable<Tickets> AvailableTickets { get; set; }

    }
}
