using Microsoft.EntityFrameworkCore;
using SimplyFlyServer.Interface;
using SimplyFlyServer.Models;
using SimplyFlyServer.Models.DTOs;

namespace SimplyFlyServer.Service
{
    public class FlightService : IFlightService
	{
        private readonly IRepository<int, Flight> _flightRepository;

        public FlightService(IRepository<int, Flight> flyingRepository)
        {
			_flightRepository = flyingRepository;
        }

        public async Task<FlightResponse> AddFlight(FlightRequest request)
        {
            try
            {
                var flying = MapToFlying(request);
                var result = await _flightRepository.Add(flying);
                return MapToResponse(result);
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception(dbEx.InnerException?.Message ?? dbEx.Message);
            }
        }


        public async Task<FlightResponse> UpdateFlight(int id, FlightRequest request)
        {
            var existing = await _flightRepository.GetById(id);
            if (existing == null)
                throw new Exception("Flying record not found");

            if (request.AircraftId > 0)
                existing.AircraftId = request.AircraftId;

            if (request.RouteId > 0)
                existing.RouteId = request.RouteId;

            if (request.DepartureTime != default)
                existing.DepartureTime = request.DepartureTime;

            if (request.ArrivalTime != default)
                existing.ArrivalTime = request.ArrivalTime;

            if (!string.IsNullOrEmpty(request.BaggageInfo))
                existing.BaggageInfo = request.BaggageInfo;

            if (request.AvailableSeats > 0)
                existing.AvailableSeats = request.AvailableSeats;

            var result = await _flightRepository.Update(id, existing);

            return MapToResponse(result);
        }

        public async Task<bool> DeleteFlight(int id)
        {
            var existing = await _flightRepository.GetById(id);
            if (existing == null)
                return false;

            await _flightRepository.Delete(id);
            return true;
        }

        public async Task<IEnumerable<FlightResponse>> GetAllFlights()
        {
            var flyings = await _flightRepository.GetAll();
            return flyings.Select(MapToResponse);
        }

        public async Task<FlightResponse> GetFlightById(int id)
        {
            var flying = await _flightRepository.GetById(id);
            if (flying == null)
                throw new Exception("Flight record not found");

            return MapToResponse(flying);
        }

        private Flight MapToFlying(FlightRequest request)
        {
            return new Flight
            {
				AircraftId = request.AircraftId,
                RouteId = request.RouteId,
                AirlineId = request.AirlineId,
                DepartureTime = request.DepartureTime,
                ArrivalTime = request.ArrivalTime,
                BaggageInfo = request.BaggageInfo,
                AvailableSeats = request.AvailableSeats
            };
        }

        private FlightResponse MapToResponse(Flight flying)
        {
            return new FlightResponse
            {
                FlightId = flying.FlightId,
                AircraftId = flying.AircraftId,
                RouteId = flying.RouteId,
                DepartureTime = flying.DepartureTime,
                ArrivalTime = flying.ArrivalTime,
                BaggageInfo = flying.BaggageInfo,
                AvailableSeats = flying.AvailableSeats,
                AirlineId = flying.Aircraft?.AirlineId ?? 0
            };
        }

        public async Task<IEnumerable<FlightResponse>> GetFlightsByFilter(FlightFilterRequest request)
        {
            var flights = (await _flightRepository.GetAll()).ToList();

            if (request.Filters != null)
                flights = FlightsByFilter(request.Filters, flights);

            return flights.Select(MapToResponse).ToList();
        }


        private List<Flight> FlightsByFilter(FlightFilter filter, List<Flight> flights)
        {
            if (filter.AirlineId.HasValue)
                flights = flights.Where(f => f.Aircraft.AirlineId == filter.AirlineId.Value).ToList();

            if (filter.DepartureTime.HasValue)
                flights = flights.Where(f => f.DepartureTime.Date == filter.DepartureTime.Value.Date).ToList();

            if (filter.ArrivalTime.HasValue)
                flights = flights.Where(f => f.ArrivalTime.Date == filter.ArrivalTime.Value.Date).ToList();

            return flights;
        }

    }
}
