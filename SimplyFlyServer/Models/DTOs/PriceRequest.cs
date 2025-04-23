namespace SimplyFlyServer.Models.DTOs
{
    public class PriceRequest
    {
        public int FlightId { get; set; }
        public decimal EconomyClass { get; set; }
        public decimal PremiumClass { get; set; }
        public decimal BusinessClass { get; set; }
    }
}
