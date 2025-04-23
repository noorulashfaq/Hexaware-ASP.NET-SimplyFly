using Microsoft.EntityFrameworkCore;
using SimplyFlyServer.Context;
using SimplyFlyServer.Models;

namespace SimplyFlyServer.Repository
{
    public class BookingRepository : Repository<int, Booking>
    {
        private readonly SimplyFlyContext _context;

        public BookingRepository(SimplyFlyContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<IEnumerable<Booking>> GetAll()
        {
            return await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Flight)
                .Include(b => b.Payment)
                .ToListAsync();
        }

        public override async Task<Booking> GetById(int key)
        {
            return await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Flight)
                .Include(b => b.Payment)
                .FirstOrDefaultAsync(b => b.BookingId == key);
        }
     
    }
}
