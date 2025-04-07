using Microsoft.EntityFrameworkCore;
using SimplyFlyServer.Context;
using SimplyFlyServer.Models;

namespace SimplyFlyServer.Repository
{
	public class AirlineRepository : Repository<int, Airline>
	{
		public AirlineRepository(SimplyFlyContext context) : base(context)
		{
		}

		public async override Task<IEnumerable<Airline>> GetAll()
		{
			return await _context.Airlines.ToListAsync();
		}

		public async override Task<Airline> GetById(int key)
		{
			var airline = _context.Airlines.FirstOrDefault(f => f.AirlineId == key);
			if (airline == null)
				throw new Exception("Airline not found");
			return airline;
		}
	}
}
