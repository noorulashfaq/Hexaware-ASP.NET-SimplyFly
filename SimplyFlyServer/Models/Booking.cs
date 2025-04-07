using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static SimplyFlyServer.Models.Flight;

namespace SimplyFlyServer.Models
{
	public class Booking
	{
		public enum classType
		{
			Economy,
			Premium,
			Business,
			FirstClass
		}
		[Key]
		public int BookingId { get; set; }
		[ForeignKey("Flight")]
		public int FlightId { get; set; }
		[ForeignKey("User")]
		public int UserId { get; set; }
		public double TotalPrice { get; set; }
		public string Status { get; set; }
		[ForeignKey("Payment")]
		public int PaymentId { get; set; }
		public DateTime BookingDate { get; set; }
		public classType ClassType { get; set; }
		public int PassengerCount { get; set; }
		public IEnumerable<Passenger> Passengers { get; set; }
		public IEnumerable<Seat> Seats { get; set; }
	}
}
