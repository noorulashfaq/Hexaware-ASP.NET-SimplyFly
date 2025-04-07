using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimplyFlyServer.Models
{
	public class Airline
	{
		[Key]
		public int AirlineId { get; set; }
		public string AirlineName { get; set; } = string.Empty;
		public string AirlineCode { get; set; } = string.Empty;
		[ForeignKey("UserId")]
		public int OwnerId { get; set; }
		public string Country { get; set; } = string.Empty;
		public User Owner { get; set; }
		public List<Aircraft> Aircrafts { get; set; } = new List<Aircraft>();
	}
}
