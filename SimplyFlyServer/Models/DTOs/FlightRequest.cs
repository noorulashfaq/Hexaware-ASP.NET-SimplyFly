namespace SimplyFlyServer.Models.DTOs
{
    public class FlightRequest
    {

        public int AircraftId { get; set; }
        public int RouteId { get; set; }
        public int AirlineId { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public string BaggageInfo { get; set; } = string.Empty;
        public int AvailableSeats { get; set; }

    }
}
