using SimplyFlyServer.Models;
using SimplyFlyServer.Models.DTOs;

namespace SimplyFlyServer.Interface
{
    public interface IRouteservice
    {
        Task<FlightRouteResponse> AddRoute(FlightRouteRequest flightRoute);
        Task<FlightRouteResponse> UpdateRoute(int id, FlightRouteRequest flightRoute);
        Task<bool> DeleteRoute(int id);
        Task<FlightRouteResponse> GetRouteById(int id);
        Task<IEnumerable<FlightRouteResponse>> GetAllRoutes();
    }
}
