namespace SimplyFlyServer.Exceptions
{
    public class AircraftNotFoundException : Exception
    {
        public AircraftNotFoundException() : base("Aircraft Not Found")
        {
        }
        public AircraftNotFoundException(string message) : base(message)
        {
        }

    }
}
