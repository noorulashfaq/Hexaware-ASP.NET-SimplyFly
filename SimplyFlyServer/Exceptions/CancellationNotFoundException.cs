namespace SimplyFlyServer.Exceptions
{
    public class CancellationNotFoundException : Exception
    {
        public CancellationNotFoundException(string message) : base(message) { }
    }
}
