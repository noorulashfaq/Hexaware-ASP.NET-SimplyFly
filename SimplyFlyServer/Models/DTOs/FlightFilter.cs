namespace SimplyFlyServer.Models.DTOs
{
    public class FlightFilter
    {

        public int? AirlineId { get; set; }
        public DateTime? DepartureTime { get; set; }
        public DateTime? ArrivalTime { get; set; }
        public int? RouteId { get; set; }


    }
}
