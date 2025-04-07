using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimplyFlyServer.Models
{
    public class Aircraft
    {
        [Key]
        public int AircraftId { get; set; }
        
        public string AircraftName { get; set; }=string.Empty;
        public string AircraftNumber { get; set; } = string.Empty;
        public int TotalSeats {  get; set; }
        public string FlightStatus { get; set; } = "Active";
        public int AirlineId { get; set; }

        
        [ForeignKey("AirlineId")]
        public Airline Airline { get; set; }

        public List<Flight> Flights { get; set; } = new List<Flight>();

    }
}
