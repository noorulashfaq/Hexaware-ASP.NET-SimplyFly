using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimplyFlyServer.Models
{
	public class Seat
	{
        [Key]
        public int SeatId { get; set; }

        [ForeignKey("Booking")]
        public int? BookingId { get; set; } 
        public Booking? Booking { get; set; }
        
        [ForeignKey("Flight")]
        public int FlightId { get; set; }
        public Flight Flight { get; set; }

        public int SeatNumber { get; set; }

        public string Status { get; set; } = "Available";

        }
}
