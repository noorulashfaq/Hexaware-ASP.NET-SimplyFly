namespace SimplyFlyServer.Exceptions
{
    public class SeatNotFoundException : Exception
    {
        public SeatNotFoundException():base("Seat not found ") { }
        public SeatNotFoundException(string message) : base(message) { }
    }
}
