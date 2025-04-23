namespace SimplyFlyServer.Models
{
	public class Payment
	{
        public enum Paymentmethod
        {
            CreditCard,
            DebitCard,
            NetBanking,
            UPI
        }
        public int PaymentId { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public Paymentmethod PaymentMethod { get; set; }
        public string PaymentStatus { get; set; } = string.Empty;
    }
}
