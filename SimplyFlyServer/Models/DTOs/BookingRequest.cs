using System.ComponentModel.DataAnnotations;

namespace SimplyFlyServer.Models.DTOs
{
    public class BookingRequest
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int FlightId { get; set; }

        [Required]
        public int PriceId { get; set; }


        [Required]
        public int PassengerCount { get; set; }

       
        [Required]
        public Payment.Paymentmethod PaymentMethod { get; set; }

        public List<int> SeatId { get; set; } = new List<int>();

        [Required]
        public List<Booking.classType> ClassTypes { get; set; } = new List<Booking.classType>();
    }
}
