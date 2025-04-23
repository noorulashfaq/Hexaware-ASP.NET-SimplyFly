namespace SimplyFlyServer.Exceptions
{
    public class FlightNotFoundException : Exception
    {
        public FlightNotFoundException()
             : base("Flight not found.")
        {
        }

        public FlightNotFoundException(string message = "Flight not found")
            : base(message)
        {
        }

        

    }
}
