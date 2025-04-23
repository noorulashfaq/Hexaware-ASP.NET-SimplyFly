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
        public enum BookingStatus
        {
            Confirmed,
            Cancelled,
            Refunded
        }
        [Key]
        public int BookingId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

        [ForeignKey("Flight")]
        public int FlightId { get; set; }
        public Flight Flight { get; set; }

        public DateTime BookingDate { get; set; }
        public decimal TotalPrice { get; set; }
        public BookingStatus Status { get; set; }

        [ForeignKey("Payment")]
        public int PaymentId { get; set; }
        public Payment Payment { get; set; }
        public classType ClassType { get; set; }
		public int PassengerCount { get; set; }
		
	}
}
