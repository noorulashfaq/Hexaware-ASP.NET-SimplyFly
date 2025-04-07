namespace SimplyFlyServer.Interface
{
    public interface ITokenService
    {
        Task<string> GenerateToken(int id, string name);
    }
}
