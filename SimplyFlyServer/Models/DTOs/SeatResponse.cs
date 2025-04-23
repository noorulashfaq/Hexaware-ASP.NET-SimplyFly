namespace SimplyFlyServer.Models.DTOs
{
    public class SeatResponse
    {
        public int SeatId { get; set; }
        public int FlightId { get; set; }
        public int? BookingId { get; set; }
        public int SeatNumber { get; set; }
        public string Status { get; set; }
    }
}
