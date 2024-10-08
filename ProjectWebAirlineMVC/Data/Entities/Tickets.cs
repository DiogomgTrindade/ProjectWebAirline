using System.Collections.Generic;

namespace ProjectWebAirlineMVC.Data.Entities
{
    public class Tickets : IEntity
    {
        public int Id { get; set; }

        public int FlightId { get; set; }

        public Flight Flight { get; set; }

        public string Seat {  get; set; }

        public bool IsAvailable { get; set; } = true;

        public string PassengerId { get; set; }

        public User Passenger { get; set; }

        public string PassengerFirstName { get; set; }

        public string PassengerLastName { get; set; }

        public string PassengerPhoneNumber { get; set; }

        public string PassengerEmail { get; set; }

        public bool HasLuggage { get; set; }

        public bool ExtraLuggage { get; set; }

    }
}
