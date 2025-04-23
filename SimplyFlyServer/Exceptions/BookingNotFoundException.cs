namespace SimplyFlyServer.Exceptions
{
    public class BookingNotFoundException : Exception
    {
        public BookingNotFoundException() : base("Booking not found") { }
        public BookingNotFoundException(string message) : base(message) { }
    }
}
