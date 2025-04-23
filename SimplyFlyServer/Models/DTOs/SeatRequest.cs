namespace SimplyFlyServer.Models.DTOs
{
    public class SeatRequest
    {
        public int FlightId { get; set; }
        public int SeatNumber { get; set; }
        public string Status { get; set; }
    }
}
