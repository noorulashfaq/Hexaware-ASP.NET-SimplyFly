using SimplyFlyServer.Interface;
using SimplyFlyServer.Models;
using SimplyFlyServer.Models.DTOs;
using System.Security.Cryptography;
using System.Text;

namespace SimplyFlyServer.Service
{
    public class AuthenticationService : IAuthenticateService
    {
        private readonly IRepository<int, User> _userRepository;
        private readonly ITokenService _tokenService;

        public AuthenticationService(IRepository<int, User> userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public async Task<LoginResponse> Login(LoginRequest loginRequest)
        {
            // Find the user by UserName
            var users = await _userRepository.GetAll();
            var user = users.FirstOrDefault(u => u.UserName == loginRequest.UserName);

            if (user == null)
                throw new UnauthorizedAccessException("User not found");

            using var hmac = new HMACSHA512(user.HashKey);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginRequest.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.Password[i])
                    throw new UnauthorizedAccessException("Invalid password");
            }

            // Generate token after successful password verification
            var token = await _tokenService.GenerateToken(user.UserId, user.UserName);

            return new LoginResponse
            {
                UserId = user.UserId,
                Token = token
            };
        }
    }
}
