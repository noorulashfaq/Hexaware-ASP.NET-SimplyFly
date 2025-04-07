namespace SimplyFlyServer.Models.DTOs
{
    public class AircraftResponse
    {
        public int AircraftId { get; set; }
        public string AircraftName { get; set; }
        public string AircraftNumber { get; set; }
        public int TotalSeats { get; set; }
        public string FlightStatus { get; set; }
        public int AirlineId { get; set; }
    }
}
