using SimplyFlyServer.Models.DTOs;

namespace SimplyFlyServer.Interface
{
    public interface IAuthenticateService
    {
        Task<LoginResponse> Login(LoginRequest loginRequest);
    }
}
