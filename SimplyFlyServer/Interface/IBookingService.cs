using SimplyFlyServer.Models.DTOs;
namespace SimplyFlyServer.Interface
{
    public interface IBookingService
    {
        Task<BookingResponse> AddBooking(BookingRequest request);
        Task<IEnumerable<BookingResponse>> GetAllBookings();
        Task<BookingResponse> GetBookingById(int id);
    }
}
