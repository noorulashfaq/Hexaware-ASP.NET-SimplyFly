using SimplyFlyServer.Exceptions;
using SimplyFlyServer.Interface;
using SimplyFlyServer.Models;
using SimplyFlyServer.Models.DTOs;

namespace SimplyFlyServer.Service
{
	public class AirlineService : IAirlineService
	{
        private readonly IRepository<int, Airline> _airlineRepository;
        private readonly IRepository<int, User> _userRepository;

        public AirlineService(IRepository<int, Airline> airlineRepository, IRepository<int, User> userRepository)
        {
            _airlineRepository = airlineRepository;
            _userRepository = userRepository;
        }

        public async Task<AirlineResponse> AddAirline(AirlineRequest airline)
        {
            var owner = await _userRepository.GetById(airline.OwnerId);
            if (owner == null)
                throw new Exception($"User with ID {airline.OwnerId} not found.");

            if (!owner.Role.ToString().Equals("FlightOwner", StringComparison.OrdinalIgnoreCase))
                throw new Exception("Only users with the role 'FlightOwner' can be set as airline owners.");

            var airlineEntity = MapAirline(airline);
            var result = await _airlineRepository.Add(airlineEntity);

            if (result == null)
                throw new FailedToAddAircraftException("Failed to create Airline. Enter correct details.");

            return new AirlineResponse
            {
                AirlineId = result.AirlineId,
                AirlineName = result.AirlineName,
                AirlineCode = result.AirlineCode,
                OwnerId = result.OwnerId,
                Country = result.Country,
            };
        }


        public async Task<AirlineResponse> UpdateAirline(int id, AirlineRequest airline)
		{
			var existingAirline = await _airlineRepository.GetById(id);
			if (existingAirline == null)
				throw new AircraftNotFoundException($"Airline with ID {id} not found.");

			if (!string.IsNullOrEmpty(airline.AirlineName))
				existingAirline.AirlineName = airline.AirlineName;
			if (!string.IsNullOrEmpty(airline.AirlineCode))
				existingAirline.AirlineCode = airline.AirlineCode;
			if (airline.OwnerId > 0)
				existingAirline.OwnerId = airline.OwnerId;
			if (!string.IsNullOrEmpty(airline.Country))
				existingAirline.Country = airline.Country;
			var result = await _airlineRepository.Update(id, existingAirline);
			return new AirlineResponse
			{
				AirlineId = result.AirlineId,
				AirlineName = result.AirlineName,
				AirlineCode = result.AirlineCode,
				OwnerId = result.OwnerId,
				Country = result.Country,
			};
		}


		public async Task<bool> DeleteAirline(int id)
		{
			var existingAirline = await _airlineRepository.GetById(id);
			if (existingAirline == null)
				return false;

			await _airlineRepository.Delete(id);
			return true;
		}

		public async Task<AirlineResponse> GetAirlineById(int id)
		{
			var flight = await _airlineRepository.GetById(id);
			if (flight == null)
                throw new AircraftNotFoundException($"Airline with ID {id} not found.");

            return new AirlineResponse
			{
				AirlineId = flight.AirlineId,
				AirlineName = flight.AirlineName,
				AirlineCode = flight.AirlineCode,
				OwnerId = flight.OwnerId,
				Country = flight.Country,
			};
		}

		public async Task<IEnumerable<AirlineResponse>> GetAllAirlines()
		{
			var airlines = await _airlineRepository.GetAll();

			return airlines.Select(airline => new AirlineResponse
			{
				AirlineId = airline.AirlineId,
				AirlineName = airline.AirlineName,
				AirlineCode = airline.AirlineCode,
				OwnerId = airline.OwnerId,
				Country = airline.Country,
			}).ToList();
		}

		
		private Airline MapAirline(AirlineRequest request)
		{
			return new Airline
			{
				AirlineName = request.AirlineName,
				AirlineCode = request.AirlineCode,
				OwnerId = request.OwnerId,
				Country = request.Country,
			};
		}
	}
}
