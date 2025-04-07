using SimplyFlyServer.Interface;
using SimplyFlyServer.Models;
using SimplyFlyServer.Models.DTOs;

namespace SimplyFlyServer.Service
{
	public class AirlineService : IAirlineService
	{
		private readonly IRepository<int, Airline> _airlineRepository;

		public AirlineService(IRepository<int, Airline> airlineRepository)
		{
			_airlineRepository = airlineRepository;
		}
		public async Task<AirlineResponse> AddAirline(AirlineRequest airline)
		{
			var airlineEntity = MapAirline(airline);
			var result = await _airlineRepository.Add(airlineEntity);
			if (result == null)
				throw new Exception("Failed to create airline");
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
				throw new Exception("Airline not found");

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
				throw new Exception("Airline not found");

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
