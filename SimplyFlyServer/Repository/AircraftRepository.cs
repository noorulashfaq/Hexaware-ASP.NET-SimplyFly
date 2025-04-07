using Microsoft.EntityFrameworkCore;
using SimplyFlyServer.Context;
using SimplyFlyServer.Models;

namespace SimplyFlyServer.Repository
{
    public class AircraftRepository : Repository<int, Aircraft>
    {
        public AircraftRepository(SimplyFlyContext context) : base(context)
        {
        }

        public async override Task<IEnumerable<Aircraft>> GetAll()
        {
            return await _context.Aircrafts.ToListAsync();
        }

        public async override Task<Aircraft> GetById(int key)
        {
            var aircraft = _context.Aircrafts.FirstOrDefault(f => f.AircraftId == key);
            if (aircraft == null)
                throw new Exception("Flight not found");
            return aircraft;
        }


    }
}
