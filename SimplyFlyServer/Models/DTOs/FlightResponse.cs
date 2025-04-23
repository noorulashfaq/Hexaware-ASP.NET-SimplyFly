namespace SimplyFlyServer.Models.DTOs
{
    public class FlightResponse
    {
        public int FlightId { get; set; }
        public int AircraftId { get; set; }
        public int RouteId { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public string BaggageInfo { get; set; }
        public int AvailableSeats { get; set; }
        public int AirlineId { get; set; }
        public decimal BasePrice {  get; set; }
    }
}
