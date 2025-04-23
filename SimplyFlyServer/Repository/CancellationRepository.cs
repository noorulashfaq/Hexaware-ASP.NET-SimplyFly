using Microsoft.EntityFrameworkCore;
using SimplyFlyServer.Context;
using SimplyFlyServer.Models;

namespace SimplyFlyServer.Repository
{
    public class CancellationRepository : Repository<int, Cancellation>
    {
        private readonly SimplyFlyContext _context;

        public CancellationRepository(SimplyFlyContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<IEnumerable<Cancellation>> GetAll()
        {
            return await _context.Cancellations
                .Include(c => c.Booking)
                .ToListAsync();
        }

        public override async Task<Cancellation> GetById(int key)
        {
            return await _context.Cancellations
                .Include(c => c.Booking)
                .FirstOrDefaultAsync(c => c.CancelId == key);
        }

       
    }
}
