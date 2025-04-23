namespace SimplyFlyServer.Interface
{
    public interface ITokenService
    {
        public Task<string> GenerateToken(int id, string name, string role);
    }
}
