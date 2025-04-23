using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SimplyFlyServer.Exceptions;
using SimplyFlyServer.Interface;
using SimplyFlyServer.Models;
using SimplyFlyServer.Models.DTOs;
using SimplyFlyServer.Repository;

namespace SimplyFlyServer.Service
{
    public class FlightService : IFlightService
    {
        private readonly IRepository<int, Flight> _flightRepository;
        private readonly IRepository<int, Aircraft> _aircraftRepository;
        private readonly IMapper _mapper;

        public FlightService(IRepository<int, Flight> flyingRepository,
                             IRepository<int, Aircraft> aircraftRepository,
                             IMapper mapper)
        {
            _flightRepository = flyingRepository;
            _aircraftRepository = aircraftRepository;
            _mapper = mapper;
        }

        public async Task<FlightResponse> AddFlight(FlightRequest request)
        {
            try
            {
                var aircraft = await _aircraftRepository.GetById(request.AircraftId);
                if (aircraft == null)
                    throw new AircraftNotFoundException($"Aircraft with ID {request.AircraftId} not found.");

                if (!aircraft.FlightStatus.Equals("Active", StringComparison.OrdinalIgnoreCase))
                    throw new FailedToAddFlightException("Flight cannot be added. The associated aircraft is not active.");

                var flight = _mapper.Map<Flight>(request);
                var result = await _flightRepository.Add(flight);
                return _mapper.Map<FlightResponse>(result);
            }
            catch (DbUpdateException)
            {
                throw new FailedToAddFlightException();
            }
        }

        public async Task<FlightResponse> UpdateFlight(int id, FlightRequest request)
        {
            var existing = await _flightRepository.GetById(id);
            if (existing == null)
                throw new FlightNotFoundException($"Flight with ID {id} not found.");

            _mapper.Map(request, existing);

            var result = await _flightRepository.Update(id, existing);
            return _mapper.Map<FlightResponse>(result);
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
            var flights = await _flightRepository.GetAll();
            return flights.Select(f => _mapper.Map<FlightResponse>(f));
        }

        public async Task<FlightResponse> GetFlightById(int id)
        {
            var flight = await _flightRepository.GetById(id);
            if (flight == null)
                throw new FlightNotFoundException($"Flight with ID {id} not found.");

            return _mapper.Map<FlightResponse>(flight);
        }

        public async Task<IEnumerable<FlightResponse>> GetFlightsByFilter(FlightFilterRequest request)
        {
            var flights = (await _flightRepository.GetAll()).ToList();

            if (request.Filters != null)
                flights = FlightsByFilter(request.Filters, flights);

            if (request.SortBy.HasValue)
                flights = SortFlight((int)request.SortBy, flights);

            return flights.Select(f => _mapper.Map<FlightResponse>(f)).ToList();
        }

        private List<Flight> FlightsByFilter(FlightFilter filter, List<Flight> flights)
        {
            if (filter.AirlineId.HasValue)
                flights = flights.Where(f => f.Aircraft.AirlineId == filter.AirlineId.Value).ToList();

            if (filter.DepartureTime.HasValue)
                flights = flights.Where(f => f.DepartureTime.Date == filter.DepartureTime.Value.Date).ToList();

            if (filter.ArrivalTime.HasValue)
                flights = flights.Where(f => f.ArrivalTime.Date == filter.ArrivalTime.Value.Date).ToList();

            if (filter.RouteId.HasValue)
                flights = flights.Where(f => f.RouteId == filter.RouteId.Value).ToList();

            return flights;
        }

        private List<Flight> SortFlight(int sortBy, List<Flight> flights)
        {
            switch (sortBy)
            {
                case -1:
                    flights = flights.OrderByDescending(f => f.BasePrice).ToList();
                    break;
                case 1:
                    flights = flights.OrderBy(f => f.BasePrice).ToList();
                    break;
            }
            return flights;
        }
    }
}
