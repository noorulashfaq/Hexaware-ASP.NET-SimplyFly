using SimplyFlyServer.Interface;
using SimplyFlyServer.Models;
using SimplyFlyServer.Models.DTOs;

namespace SimplyFlyServer.Service
{
    public class AnalysiService : IAnalysisService
    {
        private readonly IRepository<int, Booking> _bookingRepo;
        private readonly IRepository<int, Cancellation> _cancellationRepo;

        public AnalysiService(IRepository<int, Booking> bookingRepo, IRepository<int, Cancellation> cancellationRepo)
        {
            _bookingRepo = bookingRepo;
            _cancellationRepo = cancellationRepo;
        }

        public async Task<AnalysisResponse> GetBookingStatistics()
        {
            var allBookings = await _bookingRepo.GetAll();
            var allCancellations = await _cancellationRepo.GetAll();

            var confirmedBookings = allBookings
                .Where(b => b.Status == Booking.BookingStatus.Confirmed)
                .ToList();

            var totalBookings = confirmedBookings.Count;
            var totalCancellations = allCancellations.Count();

            var totalRevenue = allBookings
                .Where(b => b.Status == Booking.BookingStatus.Confirmed)
                .Sum(b => b.TotalPrice);

            return new AnalysisResponse
            {
                TotalBookings = totalBookings,
                TotalCancellations = totalCancellations,
                TotalRevenue = totalRevenue
            };
        }
    }
}
