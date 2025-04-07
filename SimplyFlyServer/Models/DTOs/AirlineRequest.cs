using System.ComponentModel.DataAnnotations.Schema;

namespace SimplyFlyServer.Models.DTOs
{
	public class AirlineRequest
	{
		public string AirlineName { get; set; }
		public string AirlineCode { get; set; }
		public int OwnerId { get; set; }
		public string Country { get; set; }
	}
}
