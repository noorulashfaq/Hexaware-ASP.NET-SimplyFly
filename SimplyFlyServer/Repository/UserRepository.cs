using Microsoft.EntityFrameworkCore;
using SimplyFlyServer.Context;
using SimplyFlyServer.Models;

namespace SimplyFlyServer.Repository
{
    public class UserRepository : Repository<int, User>
    {
        public UserRepository(SimplyFlyContext context) : base(context)
        {
        }

        public async override Task<IEnumerable<User>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }

        public async override Task<User> GetById(int key)
        {
            var users = _context.Users.FirstOrDefault(u => u.UserId == key);
            if (users == null)
                throw new Exception("Users not found");
            return users;
        }
    }
}
