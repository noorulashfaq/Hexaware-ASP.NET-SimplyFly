using System.ComponentModel.DataAnnotations.Schema;

namespace SimplyFlyServer.Models.DTOs
{
	public class AirlineResponse
	{
		public int AirlineId { get; set; }
		public string AirlineName { get; set; } = string.Empty;
		public string AirlineCode { get; set; } = string.Empty;
		public int OwnerId { get; set; }
		public string Country { get; set; } = string.Empty;
	}
}
