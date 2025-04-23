using Microsoft.EntityFrameworkCore;
using SimplyFlyServer.Context;
using SimplyFlyServer.Models;

namespace SimplyFlyServer.Repository
{
    public class SeatRepository : Repository<int, Seat>
    {
        private readonly SimplyFlyContext _context;

        public SeatRepository(SimplyFlyContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<IEnumerable<Seat>> GetAll()
        {
            return await _context.Seats.ToListAsync();
        }

        public override async Task<Seat> GetById(int key)
        {
            return await _context.Seats.FirstOrDefaultAsync(s => s.SeatId == key);
        }
    }
}
