namespace SimplyFlyServer.Exceptions
{
    public class AvailableSeatException : Exception
    {
        public AvailableSeatException() : base("Not enough available seats for your seat count") { }
        public AvailableSeatException(string message) : base(message) { }
    }
}
