using SimplyFlyServer.Models;
using SimplyFlyServer.Models.DTOs;

namespace SimplyFlyServer.Interface
{
    public interface IFlightService
	{
        Task<FlightResponse> AddFlight(FlightRequest flying);
        Task<FlightResponse> UpdateFlight(int id, FlightRequest flying);
        Task<bool> DeleteFlight(int id);
        Task<IEnumerable<FlightResponse>> GetAllFlights();
        Task<FlightResponse> GetFlightById(int id);
        Task<IEnumerable<FlightResponse>> GetFlightsByFilter(FlightFilterRequest request);

    }
}
