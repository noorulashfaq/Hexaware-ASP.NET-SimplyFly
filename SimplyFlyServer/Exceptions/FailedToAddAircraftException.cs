namespace SimplyFlyServer.Exceptions
{
    public class FailedToAddAircraftException : Exception
    {
        public FailedToAddAircraftException() : base("Failedd to add Aircraft enter correct details")
        {
        }
        public FailedToAddAircraftException(string message) : base(message)
        {
        }
    }
}
