namespace SimplyFlyServer.Exceptions
{
    public class FailedToAddFlightException : Exception
    {
        public FailedToAddFlightException() : base("Failed to dd the Flight please Enter the correct details")
        { 
        }
        public FailedToAddFlightException(string message) : base(message) 
        {
        }
    }
}
