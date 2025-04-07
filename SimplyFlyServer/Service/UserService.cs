using SimplyFlyServer.Interface;
using SimplyFlyServer.Models;
using SimplyFlyServer.Models.DTOs;
using System.Security.Cryptography;
using System.Text;


namespace SimplyFlyServer.Service
{
    public class UserService : IUserService
    {
        private readonly IRepository<int, User> _userRepository;

        public UserService(IRepository<int, User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserResponse> AddUser(UserRequest user)
        {
            HMACSHA512 hmac = new HMACSHA512();
            byte[] passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(user.Password));

            var users = MapUser(user, passwordHash, hmac.Key);
            var userResult = await _userRepository.Add(users);

            if (userResult == null)
                throw new Exception("Failed to create user");

            return new UserResponse { UserId = userResult.UserId };
        }

        public async Task<UserResponse> UpdateUser(int id, UserRequest user)
        {
            var existingUser = await _userRepository.GetById(id);
            if (existingUser == null)
                throw new Exception("User not found");

            if (!string.IsNullOrEmpty(user.UserName))
                existingUser.UserName = user.UserName;

            if (!string.IsNullOrEmpty(user.FirstName))
                existingUser.FirstName = user.FirstName;

            if (!string.IsNullOrEmpty(user.LastName))
                existingUser.LastName = user.LastName;

            if (!string.IsNullOrEmpty(user.PhoneNumber))
                existingUser.PhoneNumber = user.PhoneNumber;

            if (!string.IsNullOrEmpty(user.Address))
                existingUser.Address = user.Address;

            if (!string.IsNullOrEmpty(user.Password))
            {
                HMACSHA512 hmac = new HMACSHA512();
                existingUser.Password = hmac.ComputeHash(Encoding.UTF8.GetBytes(user.Password));
                existingUser.HashKey = hmac.Key;
            }

            var result = await _userRepository.Update(id, existingUser);
            return new UserResponse { UserId = result.UserId };
        }

        public async Task<bool> DeleteUser(int id)
        {
            var existingUser = await _userRepository.GetById(id);
            if (existingUser == null)
                throw new Exception("User not found");

            await _userRepository.Delete(id);
            return true;
        }

        private User MapUser(UserRequest request, byte[] passwordHash, byte[] key)
        {
            return new User
            {
                UserName = request.UserName,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                Password = passwordHash,
                HashKey = key,
                CreatedDate = DateTime.UtcNow
            };
        }
    }
}
