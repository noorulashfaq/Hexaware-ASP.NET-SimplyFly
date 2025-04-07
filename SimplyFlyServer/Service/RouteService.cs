using SimplyFlyServer.Interface;
using SimplyFlyServer.Models;
using SimplyFlyServer.Models.DTOs;

namespace SimplyFlyServer.Service
{
    public class RouteService : IRouteservice
    {
        private readonly IRepository<int, FlightRoute> _routeRepository;

        public RouteService(IRepository<int, FlightRoute> routeRepository)
        {
            _routeRepository = routeRepository;
        }

        public async Task<FlightRouteResponse> AddRoute(FlightRouteRequest routeRequest)
        {
            if (routeRequest == null)
                throw new ArgumentNullException(nameof(routeRequest));

            var route = MapToFlightRoute(routeRequest);
            var result = await _routeRepository.Add(route);

            return new FlightRouteResponse
            {
                RouteId = result.RouteId,
                Source = result.Source,
                Destination = result.Destination
            };
        }

        public async Task<FlightRouteResponse> UpdateRoute(int id, FlightRouteRequest routeRequest)
        {
            var existingRoute = await _routeRepository.GetById(id);
            if (existingRoute == null)
                throw new Exception("Route not found");

            if (!string.IsNullOrEmpty(routeRequest.Source))
                existingRoute.Source = routeRequest.Source;

            if (!string.IsNullOrEmpty(routeRequest.Destination))
                existingRoute.Destination = routeRequest.Destination;

            var result = await _routeRepository.Update(id, existingRoute);

            return new FlightRouteResponse
            {
                RouteId = result.RouteId,
                Source = result.Source,
                Destination = result.Destination
            };
        }

        public async Task<bool> DeleteRoute(int id)
        {
            var existingRoute = await _routeRepository.GetById(id);
            if (existingRoute == null)
                return false;

            await _routeRepository.Delete(id);
            return true;
        }

        public async Task<IEnumerable<FlightRouteResponse>> GetAllRoutes()
        {
            var routes = await _routeRepository.GetAll();

            return routes.Select(route => new FlightRouteResponse
            {
                RouteId = route.RouteId,
                Source = route.Source,
                Destination = route.Destination
            });
        }

        public async Task<FlightRouteResponse> GetRouteById(int id)
        {
            var route = await _routeRepository.GetById(id);
            if (route == null)
                throw new Exception("Route not found");

            return new FlightRouteResponse
            {
                RouteId = route.RouteId,
                Source = route.Source,
                Destination = route.Destination
            };
        }

        private FlightRoute MapToFlightRoute(FlightRouteRequest request)
        {
            return new FlightRoute
            {
                Source = request.Source,
                Destination = request.Destination
            };
        }

        
    }
}
