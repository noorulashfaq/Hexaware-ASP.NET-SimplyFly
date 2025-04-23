using SimplyFlyServer.Exceptions;
using SimplyFlyServer.Interface;
using SimplyFlyServer.Models;
using SimplyFlyServer.Models.DTOs;

namespace SimplyFlyServer.Service
{
    public class SeatService : ISeatService
    {
        private readonly IRepository<int, Seat> _seatRepository;
        private readonly IRepository<int, Flight> _flightRepo;
        private readonly IRepository<int, Booking> _bookingRepo;
        private readonly ILogger<SeatService> _logger;


        public SeatService(IRepository<int, Seat> seatRepository,
                           IRepository<int, Flight> flightRepo,
                           IRepository<int, Booking> bookingRepo,
                           ILogger<SeatService> logger)
        {
            _seatRepository = seatRepository;
            _flightRepo = flightRepo;
            _bookingRepo = bookingRepo;
            _logger = logger;
        }

        public async Task<SeatResponse> AddSeat(SeatRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var seat = new Seat
            {
                FlightId = request.FlightId,
                SeatNumber = request.SeatNumber,
                Status = request.Status
            };

            var result = await _seatRepository.Add(seat);

            return new SeatResponse
            {
                SeatId = result.SeatId,
                FlightId = result.FlightId,
                BookingId = result.BookingId,
                SeatNumber = result.SeatNumber,
                Status = result.Status
            };
        }

        public async Task<SeatResponse> UpdateSeat(int id, SeatRequest request)
        {
            var existing = await _seatRepository.GetById(id);
            if (existing == null)
                throw new SeatNotFoundException($"Seat with ID {id} not found.");

            if (request.FlightId > 0)
                existing.FlightId = request.FlightId;

            if (request.SeatNumber>0)
                existing.SeatNumber = request.SeatNumber;

            if (!string.IsNullOrEmpty(request.Status))
                existing.Status = request.Status;

            var result = await _seatRepository.Update(id, existing);

            return new SeatResponse
            {
                SeatId = result.SeatId,
                FlightId = result.FlightId,
                BookingId = result.BookingId,
                SeatNumber = result.SeatNumber,
                Status = result.Status
            };
        }

        public async Task<SeatResponse> DeleteSeat(int seatId)
        {
            var existing = await _seatRepository.GetById(seatId);
            if (existing == null)
                throw new SeatNotFoundException($"Seat with ID {seatId} not found.");

            var result = await _seatRepository.Delete(seatId);

            return new SeatResponse
            {
                SeatId = result.SeatId,
                FlightId = result.FlightId,
                BookingId = result.BookingId,
                SeatNumber = result.SeatNumber,
                Status = result.Status
            };
        }

        public async Task<IEnumerable<SeatResponse>> GetAllSeats()
        {
            var seats = await _seatRepository.GetAll();
            return seats.Select(MapToSeatResponse);
        }

        private SeatResponse MapToSeatResponse(Seat seat)
        {
            return new SeatResponse
            {
                SeatId = seat.SeatId,
                FlightId = seat.FlightId,
                BookingId = seat.BookingId,
                SeatNumber = seat.SeatNumber,
                Status = seat.Status
            };
        }


        public async Task<SeatResponse> GetSeatById(int seatId)
        {
            var seat = await _seatRepository.GetById(seatId);
            if (seat == null)
                throw new SeatNotFoundException($"Seat with ID {seatId} not found.");

            return new SeatResponse
            {
                SeatId = seat.SeatId,
                FlightId = seat.FlightId,
                BookingId = seat.BookingId,
                SeatNumber = seat.SeatNumber,
                Status = seat.Status
            };
        }
        public async Task ReleaseSeatsForArrivedFlights()
        {
            var flights = await _flightRepo.GetAll();
            var pastFlights = flights.Where(f => f.ArrivalTime <= DateTime.Now).ToList();

            var allSeats = await _seatRepository.GetAll();
            var allBookings = await _bookingRepo.GetAll();

            foreach (var flight in pastFlights)
            {
                var bookings = allBookings
                    .Where(b => b.FlightId == flight.FlightId && b.Status == Booking.BookingStatus.Confirmed)
                    .ToList();

                foreach (var booking in bookings)
                {
                    var bookedSeats = allSeats
                        .Where(s => s.BookingId == booking.BookingId)
                        .ToList();

                    foreach (var seat in bookedSeats)
                    {
                        seat.Status = "Available";
                        seat.BookingId = null;
                        await _seatRepository.Update(seat.SeatId, seat);
                    }
                }

                _logger.LogInformation($"Released seats for Flight ID: {flight.FlightId} as it has already arrived.");
            }
        }

    }
}

