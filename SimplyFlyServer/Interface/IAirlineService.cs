using SimplyFlyServer.Models.DTOs;

namespace SimplyFlyServer.Interface
{
	public interface IAirlineService
	{
		Task<AirlineResponse> AddAirline(AirlineRequest airline);
		Task<AirlineResponse> UpdateAirline(int id, AirlineRequest airline);
		Task<bool> DeleteAirline(int id);
		Task<AirlineResponse> GetAirlineById(int id);
		Task<IEnumerable<AirlineResponse>> GetAllAirlines();
	}
}
