namespace SimplyFlyServer.Models.DTOs
{
    public class CancellationRequest
    {
        public int BookingId { get; set; }
        public string Reason { get; set; }
    }
}
