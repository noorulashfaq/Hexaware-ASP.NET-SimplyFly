using Microsoft.EntityFrameworkCore;
using SimplyFlyServer.Context;
using SimplyFlyServer.Models;

namespace SimplyFlyServer.Repository
{
    public class PriceRepository : Repository<int, Price>
    {
        public PriceRepository(SimplyFlyContext context) : base(context)
        {
        }

        public async override Task<IEnumerable<Price>> GetAll()
        {
            return await _context.Prices
                .Include(p => p.Flight)
                .ToListAsync();
        }

        public async override Task<Price> GetById(int key)
        {
            return await _context.Prices
                .Include(p => p.Flight)
                .FirstOrDefaultAsync(p => p.PriceId == key);
        }
    }
}
