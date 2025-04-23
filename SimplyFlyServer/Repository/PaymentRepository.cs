using Microsoft.EntityFrameworkCore;
using SimplyFlyServer.Context;
using SimplyFlyServer.Models;

namespace SimplyFlyServer.Repository
{
    public class PaymentRepository : Repository<int, Payment>
    {
        private readonly SimplyFlyContext _context;

        public PaymentRepository(SimplyFlyContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<IEnumerable<Payment>> GetAll()
        {
            return await _context.Payments.ToListAsync();
        }

        public override async Task<Payment> GetById(int key)
        {
            return await _context.Payments.FirstOrDefaultAsync(p => p.PaymentId == key);
        }
    }
}
