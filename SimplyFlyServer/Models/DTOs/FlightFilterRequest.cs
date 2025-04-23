namespace SimplyFlyServer.Models.DTOs
{
    public class FlightFilterRequest
    {
        public FlightFilter? Filters { get; set; }
        public int? SortBy { get; set; }
        //public PaginationRequest? Pagination { get; set; }
    }
}
