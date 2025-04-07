using System.ComponentModel.DataAnnotations;

namespace SimplyFlyServer.Models
{
    public class FlightRoute
    {
        [Key]
        public int RouteId { get; set; }
        public string Source { get; set; } = string.Empty;
        public string Destination { get; set; }= string.Empty;
        public List<Flight> Flyings { get; set; } = new List<Flight>();

    }
}
