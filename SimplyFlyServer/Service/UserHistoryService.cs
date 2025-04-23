using SimplyFlyServer.Context;
using SimplyFlyServer.Models.DTOs;
using SimplyFlyServer.Interface;
using Microsoft.EntityFrameworkCore;

namespace SimplyFlyServer.Service
{
    public class UserHistoryService : IUserHistoryService
    {
        private readonly SimplyFlyContext _context;

        public UserHistoryService(SimplyFlyContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserHistoryResponse>> GetUserHistory(int userId)
        {
            var bookings = await _context.Bookings
                .Include(b => b.Flight)
                .Include(b => b.Payment)
                .Include(b => b.User)
                .Where(b => b.UserId == userId)
                .ToListAsync();

            var cancellations = await _context.Cancellations.ToListAsync();

            var result = bookings.Select(b => {
                var cancellation = cancellations.FirstOrDefault(c => c.BookingId == b.BookingId);
                return new UserHistoryResponse
                {
                    BookingId = b.BookingId,
                    BookingDate = b.BookingDate,
                    TotalPrice = b.TotalPrice,
                    ClassType = b.ClassType.ToString(),
                    BookingStatus = b.Status.ToString(),
                    FlightId = b.FlightId,
                    CancellationId = cancellation?.CancelId,
                    CancellationDate = cancellation?.CancellationDate,
                    CancellationReason = cancellation?.Reason,
                    RefundAmount = cancellation?.RefundAmount,
                    RefundStatus = cancellation?.RefundStatus.ToString()
                };
            });

            return result;
        }
    }
}
