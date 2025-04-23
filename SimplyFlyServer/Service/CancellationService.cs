using AutoMapper;
using SimplyFlyServer.Exceptions;
using SimplyFlyServer.Interface;
using SimplyFlyServer.Models;
using SimplyFlyServer.Models.DTOs;
using SimplyFlyServer.Repository;

namespace SimplyFlyServer.Service
{
    public class CancellationService : ICancellationService
    {
        private readonly IRepository<int, Booking> _bookingRepo;
        private readonly IRepository<int, Flight> _flightRepo;
        private readonly IRepository<int, Seat> _seatRepo;
        private readonly IRepository<int, Cancellation> _cancellationRepo;
        private readonly ILogger<CancellationService> _logger;
        private readonly IMapper _mapper;

        public CancellationService(
            IRepository<int, Booking> bookingRepo,
            IRepository<int, Flight> flightRepo,
            IRepository<int, Seat> seatRepo,
            IRepository<int, Cancellation> cancellationRepo,
            ILogger<CancellationService> logger,
             IMapper mapper)
        {
            _bookingRepo = bookingRepo;
            _flightRepo = flightRepo;
            _seatRepo = seatRepo;
            _cancellationRepo = cancellationRepo;
            _logger = logger;
            _mapper = mapper;
        }


        public async Task<CancellationResponse> CancelBooking(CancellationRequest request)
        {
            var booking = await _bookingRepo.GetById(request.BookingId);
            if (booking == null)
            {
                _logger.LogWarning($"Booking not found with ID: {request.BookingId}");
                throw new BookingNotFoundException($"booking with ID {request.BookingId} not found.");
            }

            if (booking.Status == Booking.BookingStatus.Cancelled)
            {
                _logger.LogWarning($"Booking with ID {request.BookingId} is already cancelled.");
                throw new Exception($"Booking with ID {request.BookingId} is already cancelled.");
                
            }

            var flight = await _flightRepo.GetById(booking.FlightId);
            if (flight == null)
                throw new FlightNotFoundException($"Flight with ID {booking.FlightId} not found.");

            booking.Status = Booking.BookingStatus.Cancelled;
            await _bookingRepo.Update(booking.BookingId, booking);

            flight.AvailableSeats += booking.PassengerCount;
            await _flightRepo.Update(flight.FlightId, flight);

            // Get all seats for this booking
            var seats = await _seatRepo.GetAll();
            var bookedSeats = seats.Where(s => s.BookingId == request.BookingId).ToList();

            // Update status to "Available" and clear BookingId
            foreach (var seat in bookedSeats)
            {
                seat.Status = "Available";
                seat.BookingId = null;
                await _seatRepo.Update(seat.SeatId, seat);
            }


            var cancellation = _mapper.Map<Cancellation>(request);
            cancellation.CancellationDate = DateTime.Now;
            cancellation.RefundAmount = booking.TotalPrice;
            cancellation.RefundStatus = Cancellation.RefundStatusEnum.Processed;

            var savedCancellation = await _cancellationRepo.Add(cancellation);

            _logger.LogInformation($"Cancellation saved successfully. CancelId: {savedCancellation.CancelId}");

            return _mapper.Map<CancellationResponse>(savedCancellation);
        }

        public async Task<IEnumerable<CancellationResponse>> GetAllCancellations()
        {
            var cancellations = await _cancellationRepo.GetAll();
            return _mapper.Map<IEnumerable<CancellationResponse>>(cancellations);
        }

        public async Task<CancellationResponse> GetCancellationById(int id)
        {
            var cancellation = await _cancellationRepo.GetById(id);
            if (cancellation == null)
                throw new CancellationNotFoundException($"Cancellation with ID {id} not found.");

            return _mapper.Map<CancellationResponse>(cancellation);
        }

        public async Task<CancellationResponse> UpdateRefundStatus(int cancelId, string newStatus)
        {
            var cancellation = await _cancellationRepo.GetById(cancelId);
            if (cancellation == null)
                throw new CancellationNotFoundException($"Cancellation with ID {cancelId} not found.");

            if (!Enum.TryParse<Cancellation.RefundStatusEnum>(newStatus, true, out var parsedStatus))
                throw new ArgumentException($"Invalid refund status: {newStatus}");

            cancellation.RefundStatus = parsedStatus;
            var updated = await _cancellationRepo.Update(cancelId, cancellation);

            return _mapper.Map<CancellationResponse>(updated);
        }
    }
}