using SimplyFlyServer.Models;
using SimplyFlyServer.Models.DTOs;

namespace SimplyFlyServer.Interface
{
    public interface ISeatService
    {
        Task<SeatResponse> AddSeat(SeatRequest seat);
        Task<SeatResponse> UpdateSeat(int seatId, SeatRequest seat);
        Task<SeatResponse> DeleteSeat(int seatId);
        Task<IEnumerable<SeatResponse>> GetAllSeats();
        Task<SeatResponse> GetSeatById(int seatId);
        public  Task ReleaseSeatsForArrivedFlights();

    }
}
