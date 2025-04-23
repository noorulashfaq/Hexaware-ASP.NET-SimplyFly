namespace SimplyFlyServer.Models.DTOs
{
    public class AnalysisResponse
    {
        public int TotalBookings { get; set; }
        public int TotalCancellations { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
