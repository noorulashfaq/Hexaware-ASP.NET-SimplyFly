using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimplyFlyServer.Models
{
	public class Price
	{
        [Key]
        public int PriceId { get; set; }

        [ForeignKey("Flight")]
        public int FlightId { get; set; }
        public Flight Flight { get; set; }

        public decimal PremiumClass { get; set; }
        public decimal EconomicClass { get; set; }
        public decimal BusinessClass { get; set; }
    }
}
