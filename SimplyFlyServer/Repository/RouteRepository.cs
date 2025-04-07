using Microsoft.EntityFrameworkCore;
using SimplyFlyServer.Context;
using SimplyFlyServer.Models;

namespace SimplyFlyServer.Repository
{
    public class RouteRepository : Repository<int, FlightRoute>
    {
        public RouteRepository(SimplyFlyContext context) : base(context)
        {
        }

        public async override Task<IEnumerable<FlightRoute>> GetAll()
        {
            return await _context.Routes.ToListAsync();
        }

        public async override Task<FlightRoute> GetById(int key)
        {
            var routes = _context.Routes.FirstOrDefault(f => f.RouteId == key);
            if (routes == null)
                throw new Exception("Flight not found");
            return routes;
        }
    }
}
