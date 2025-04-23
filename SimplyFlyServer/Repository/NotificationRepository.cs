using Microsoft.EntityFrameworkCore;
using SimplyFlyServer.Context;
using SimplyFlyServer.Models;

namespace SimplyFlyServer.Repository
{
    public class NotificationRepository : Repository<int, Notification>
    {
        public NotificationRepository(SimplyFlyContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Notification>> GetAll()
        {
            return await _context.Notifications.ToListAsync();
        }

        public override async Task<Notification> GetById(int key)
        {
            return await _context.Notifications.FirstOrDefaultAsync(n => n.NotificationId == key);
        }

    }
}
