namespace SimplyFlyServer.Models.DTOs
{
    public class PriceResponse
    {
        public int PriceId { get; set; }
        public int FlightId { get; set; }
        public decimal EconomyClass { get; set; }
        public decimal PremiumClass { get; set; }
        public decimal BusinessClass { get; set; }
    }
}
