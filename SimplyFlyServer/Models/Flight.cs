using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimplyFlyServer.Models
{
    public class Flight
    {
        
        [Key]
        public int FlightId { get; set; }
        public int AircraftId {  get; set; }
        public Aircraft Aircraft { get; set; }
        public int RouteId {  get; set; }
        public FlightRoute FlightRoute { get; set; }

        [ForeignKey("Airline")]
        public int AirlineId { get; set; }  
        public Airline Airline { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
		public string BaggageInfo { get; set; } = string.Empty;
        public int AvailableSeats { get; set; }
    }
}
