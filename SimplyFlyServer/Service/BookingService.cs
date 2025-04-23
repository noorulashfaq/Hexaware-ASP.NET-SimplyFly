using SimplyFlyServer.Interface;
using SimplyFlyServer.Models.DTOs;
using SimplyFlyServer.Models;
using SimplyFlyServer.Repository;
using SimplyFlyServer.Exceptions;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace SimplyFlyServer.Service
{
    public class BookingService : IBookingService
    {
        private readonly IRepository<int, Booking> _bookingRepo;
        private readonly IRepository<int, Payment> _paymentRepo;
        private readonly IRepository<int, Price> _priceRepo;
        private readonly IRepository<int, Flight> _flightRepo;
        private readonly IRepository<int, Seat> _seatsRepo;
        private readonly ILogger<BookingService> _logging;
        private readonly IMapper _mapper;

        public BookingService(IRepository<int, Booking> bookingRepo,
                              IRepository<int, Payment> paymentRepo,
                              IRepository<int, Price> priceRepo,
                              IRepository<int, Flight> flightRepo,
                              IRepository<int, Seat> seatsRepo,
                              ILogger<BookingService> logger,
                              IMapper mapper)
        {
            _bookingRepo = bookingRepo;
            _paymentRepo = paymentRepo;
            _priceRepo = priceRepo;
            _flightRepo = flightRepo;
            _seatsRepo = seatsRepo;
            _logging=logger;
            _mapper = mapper;
        }

        public async Task<BookingResponse> AddBooking(BookingRequest request)
        {
            if (request == null)
            
                throw new ArgumentNullException(nameof(request));
            
            if (request.SeatId == null || request.SeatId.Count == 0)
                throw new Exception("Seat id list is missing or empty.");

            if (request.ClassTypes == null || request.ClassTypes.Count == 0)
                throw new Exception("Class types list is missing or empty.");

            if (request.SeatId.Count != request.ClassTypes.Count)
                throw new Exception("seatid count and ClassTypes count must match.");

            

            var price = await _priceRepo.GetById(request.PriceId);
            if (price == null)
                throw new Exception($"Price with ID {request.PriceId} not found.");

            var flight = await _flightRepo.GetById(request.FlightId);
            if (flight == null)
            {
                _logging.LogError("Flight ID {FlightId} not found.", request.FlightId);
                throw new FlightNotFoundException("Flight Id not found ");
            }
            if (flight.AvailableSeats < request.PassengerCount)
            {
                _logging.LogWarning("Insufficient seats for Flight ID {FlightId}.", request.FlightId);
                throw new AvailableSeatException();
            }
            foreach (var seatId in request.SeatId)
            {
                var seat = await _seatsRepo.GetById(seatId);

                if (seat == null)
                    throw new Exception($"Seat ID {seatId} not found.");

                if (seat.FlightId != request.FlightId)
                    throw new Exception($"Seat ID {seatId} does not belong to Flight ID {request.FlightId}.");

                if (seat.Status != null && seat.Status.Equals("Booked", StringComparison.OrdinalIgnoreCase))
                    throw new Exception($"Seat ID {seatId} is already booked.");
            }


          
            decimal totalAmount = 0;

           
            for (int i = 0; i < request.ClassTypes.Count; i++)
            {
                totalAmount += request.ClassTypes[i] switch
                {
                    Booking.classType.Business => price.BusinessClass,
                    Booking.classType.Premium => price.PremiumClass,
                    Booking.classType.Economy => price.EconomicClass,
                    _ => throw new InvalidChoiseException()
                };
            }

            _logging.LogInformation("Total amount calculated: {TotalAmount}", totalAmount);

            var payment = new Payment
            {
                PaymentDate = DateTime.Now,
                Amount = totalAmount,
                PaymentMethod = request.PaymentMethod,
                PaymentStatus = "Success"
            };

            var savedPayment = await _paymentRepo.Add(payment);

            var booking = new Booking
            {
                UserId = request.UserId,
                FlightId = request.FlightId,
                BookingDate = DateTime.Now,
                TotalPrice = totalAmount,
                Status = Booking.BookingStatus.Confirmed,
                PaymentId = savedPayment.PaymentId,
                ClassType = request.ClassTypes[0],
                PassengerCount = request.PassengerCount
            };

            var savedBooking = await _bookingRepo.Add(booking);

            flight.AvailableSeats -= request.PassengerCount;
            await _flightRepo.Update(flight.FlightId, flight);

            foreach (var seatId in request.SeatId)
            {
                var seat = await _seatsRepo.GetById(seatId);
                seat.BookingId = savedBooking.BookingId;
                seat.Status = "Booked";
                await _seatsRepo.Update(seat.SeatId, seat);
            }
            _logging.LogInformation("Booking completed successfully for BookingId: {BookingId}", savedBooking.BookingId);


            return new BookingResponse
            {
                BookingId = savedBooking.BookingId,
                UserId = savedBooking.UserId,
                FlightId = savedBooking.FlightId,
                TotalPrice = savedBooking.TotalPrice,
                ClassType = savedBooking.ClassType.ToString(),
                PassengerCount = request.PassengerCount,
                BookingDate = savedBooking.BookingDate,
                Status = savedBooking.Status.ToString(),
                PaymentId = savedBooking.PaymentId,
                PaymentStatus = "Success"
            };
        }

        public async Task<IEnumerable<BookingResponse>> GetAllBookings()
        {
            var bookings = await _bookingRepo.GetAll();

            return bookings.Select(b => new BookingResponse
            {
                BookingId = b.BookingId,
                UserId = b.UserId,
                FlightId = b.FlightId,
                TotalPrice = b.TotalPrice,
                ClassType = b.ClassType.ToString(),
                PassengerCount = b.PassengerCount,
                BookingDate = b.BookingDate,
                Status = b.Status.ToString(),
                PaymentId = b.PaymentId,
                PaymentStatus = "Success"
            });
        }

        public async Task<BookingResponse> GetBookingById(int id)
        {
            var booking = await _bookingRepo.GetById(id);
            if (booking == null)
                throw new BookingNotFoundException($"Booking with ID {id} not found.");

            return new BookingResponse
            {
                BookingId = booking.BookingId,
                UserId = booking.UserId,
                FlightId = booking.FlightId,
                TotalPrice = booking.TotalPrice,
                ClassType = booking.ClassType.ToString(),
                PassengerCount = booking.PassengerCount,
                BookingDate = booking.BookingDate,
                Status = booking.Status.ToString(),
                PaymentId = booking.PaymentId,
                PaymentStatus = "Success"
            };
        }
    }
}
