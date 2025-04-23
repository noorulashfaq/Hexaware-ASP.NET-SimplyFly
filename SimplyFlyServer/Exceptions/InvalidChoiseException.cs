namespace SimplyFlyServer.Exceptions
{
    public class InvalidChoiseException : Exception
    {
        public InvalidChoiseException() : base("Invalid choice choose correct option") { }
        public InvalidChoiseException(string message) : base(message) { }
    }
}
