namespace SimplyFlyServer.Models.DTOs
{
    public class UserHistoryResponse
    {
        public int BookingId { get; set; }
        public DateTime BookingDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string ClassType { get; set; }
        public string BookingStatus { get; set; }
        public int FlightId { get; set; }
        public int? CancellationId { get; set; }
        public string? CancellationReason { get; set; }
        public DateTime? CancellationDate { get; set; }
        public decimal? RefundAmount { get; set; }
        public string? RefundStatus { get; set; }
    }
}
