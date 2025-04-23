namespace SimplyFlyServer.Models.DTOs
{
    public class CancellationResponse
    {
        public int CancelId { get; set; }
        public int BookingId { get; set; }
        public DateTime CancellationDate { get; set; }
        public string Reason { get; set; }
        public decimal RefundAmount { get; set; }
        public string RefundStatus { get; set; }
    }
}
