using SimplyFlyServer.Exceptions;
using SimplyFlyServer.Interface;
using SimplyFlyServer.Models;
using SimplyFlyServer.Models.DTOs;

namespace SimplyFlyServer.Service
{
    public class AircraftService : IAircraftService
    {
        private readonly IRepository<int, Aircraft> _aircraftRepository;

        public AircraftService(IRepository<int, Aircraft> aircraftRepository)
        {
			_aircraftRepository = aircraftRepository;
        }

        public async Task<AircraftResponse> AddAircraft(AircraftRequest flight)
        {
            var flightEntity = MapFlight(flight);
            var result = await _aircraftRepository.Add(flightEntity);
            if (result == null)
                throw new FailedToAddAircraftException();

            return new AircraftResponse
            {
				AircraftId = result.AircraftId,
                AircraftName = result.AircraftName,
				AircraftNumber = result.AircraftNumber,
                TotalSeats = result.TotalSeats,
                FlightStatus = result.FlightStatus,
                AirlineId = result.AirlineId,
                
            };
        }

        public async Task<AircraftResponse> UpdateAircraft(int id, AircraftRequest flight)
        {
            var existingFlight = await _aircraftRepository.GetById(id);
            if (existingFlight == null)
                throw new Exception("Flight not found");

            if (!string.IsNullOrEmpty(flight.AircraftName))
                existingFlight.AircraftName = flight.AircraftName;

            if (!string.IsNullOrEmpty(flight.AircraftNumber))
                existingFlight.AircraftNumber = flight.AircraftNumber;

            if (flight.TotalSeats > 0)
                existingFlight.TotalSeats = flight.TotalSeats;

            if (!string.IsNullOrEmpty(flight.FlightStatus))
                existingFlight.FlightStatus = flight.FlightStatus;

            var result = await _aircraftRepository.Update(id, existingFlight);

            return new AircraftResponse
            {
				AircraftId = result.AircraftId,
                AircraftName = result.AircraftName,
				AircraftNumber = result.AircraftNumber,
                TotalSeats = result.TotalSeats,
                FlightStatus = result.FlightStatus,
                AirlineId = result.AirlineId,
            };
        }

        public async Task<bool> DeleteAircraft(int id)
        {
            var existingFlight = await _aircraftRepository.GetById(id);
            if (existingFlight == null)
                return false;

            await _aircraftRepository.Delete(id);
            return true;
        }

        public async Task<AircraftResponse> GetAircraftById(int id)
        {
            var flight = await _aircraftRepository.GetById(id);
            if (flight == null)
                throw new Exception("Flight not found");

            return new AircraftResponse
            {
				AircraftId = flight.AircraftId,
                AircraftName = flight.AircraftName,
				AircraftNumber = flight.AircraftNumber,
                TotalSeats = flight.TotalSeats,
                FlightStatus = flight.FlightStatus,
                AirlineId=flight.AirlineId,
            };
        }

        public async Task<IEnumerable<AircraftResponse>> GetAllAircrafts()
        {
            var flights = await _aircraftRepository.GetAll();

            return flights.Select(f => new AircraftResponse
            {
				AircraftId = f.AircraftId,
                AircraftName = f.AircraftName,
				AircraftNumber = f.AircraftNumber,
                TotalSeats = f.TotalSeats,
                FlightStatus = f.FlightStatus,
                AirlineId = f.AirlineId,
            }).ToList();
        }

        private Aircraft MapFlight(AircraftRequest request)
        {
            return new Aircraft
            {
                AircraftName = request.AircraftName,
				AircraftNumber = request.AircraftNumber,
                TotalSeats = request.TotalSeats,
                FlightStatus = request.FlightStatus,
                AirlineId = request.AirlineId,

            };
        }
    }
}
