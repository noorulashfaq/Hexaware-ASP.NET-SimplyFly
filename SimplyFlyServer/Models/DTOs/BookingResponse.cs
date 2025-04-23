namespace SimplyFlyServer.Models.DTOs
{
    public class BookingResponse
    {
        public int BookingId { get; set; }

        public int UserId { get; set; }

        public int FlightId { get; set; }

        public DateTime BookingDate { get; set; }

        public decimal TotalPrice { get; set; }

        public string Status { get; set; }

        public string ClassType { get; set; }

        public int PassengerCount { get; set; }

        public int PaymentId { get; set; }

        public string PaymentStatus { get; set; }
    }
}
