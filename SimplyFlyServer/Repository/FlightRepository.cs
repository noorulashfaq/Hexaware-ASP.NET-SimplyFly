using Microsoft.EntityFrameworkCore;
using SimplyFlyServer.Context;
using SimplyFlyServer.Models;

namespace SimplyFlyServer.Repository
{
    public class FlightRepository : Repository<int, Flight>
    {
        public FlightRepository(SimplyFlyContext context) : base(context)
        {
        }

        public async override Task<IEnumerable<Flight>> GetAll()
        {
            return await _context.Flights
                .Include(f => f.Aircraft)
                    .ThenInclude(a => a.Airline)
                .Include(f => f.FlightRoute)
                .ToListAsync();
        }

        public async override Task<Flight> GetById(int key)
        {
            var flyings = _context.Flights.FirstOrDefault(f => f.FlightId == key);
            if (flyings == null)
                throw new Exception("Flying routes not found");
            return flyings;
        }
    }
}
