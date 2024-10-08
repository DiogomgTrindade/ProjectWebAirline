using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectWebAirlineMVC.Data.Entities;
using System.Collections.Generic;

namespace ProjectWebAirlineMVC.Models
{
    public class TicketPurchaseViewModel
    {
        public int TicketId { get; set; }
        public int FlightId { get; set; }

        // Dados do passageiro
        public string PassengerFirstName { get; set; }
        public string PassengerLastName { get; set; }
        public string PassengerPhoneNumber { get; set; }
        public string PassengerEmail { get; set; }

        // Mala de porão e extra
        public bool HasLuggage { get; set; }
        public bool ExtraLuggage { get; set; }

        // Assentos disponíveis
        public List<SelectListItem> AvailableSeats { get; set; }
        public string SelectedSeat { get; set; }

        // Informações do voo
        public Flight Flight { get; set; }

    }
}
