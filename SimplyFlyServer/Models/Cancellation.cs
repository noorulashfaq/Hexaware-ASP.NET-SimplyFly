using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimplyFlyServer.Models
{
	public class Cancellation
	{
        public enum RefundStatusEnum
        {
            Processed,
            Pending
        }

        [Key]
        public int CancelId { get; set; }

        [ForeignKey("Booking")]
        public int BookingId { get; set; }
        public Booking Booking { get; set; }

        public DateTime CancellationDate { get; set; }

        [Column(TypeName = "text")]
        public string Reason { get; set; }

        public decimal RefundAmount { get; set; }

        public RefundStatusEnum RefundStatus { get; set; }
    }
}
