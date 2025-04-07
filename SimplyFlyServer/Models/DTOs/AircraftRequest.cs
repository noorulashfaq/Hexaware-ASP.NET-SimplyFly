namespace SimplyFlyServer.Models.DTOs
{
    public class AircraftRequest
    {
        public string AircraftName { get; set; } = string.Empty;
        public string AircraftNumber { get; set; } = string.Empty;
        public int TotalSeats { get; set; }
        public string FlightStatus { get; set; } = string.Empty;
        public int AirlineId { get; set; }
    }
}
