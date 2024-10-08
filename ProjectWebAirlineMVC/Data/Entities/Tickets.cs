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
    }
}
