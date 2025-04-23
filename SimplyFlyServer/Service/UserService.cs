using SimplyFlyServer.Exceptions;
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
            var allUsers = await _userRepository.GetAll(); // Get all users

            // Check if username or phone number already exists
            var existingUser = allUsers.FirstOrDefault(u =>
                u.UserName == user.UserName || u.PhoneNumber == user.PhoneNumber);

            if (existingUser != null)
            {
                throw new Exception("User with the same username or phone number already exists.");
            }
            HMACSHA512 hmac = new HMACSHA512();
            byte[] passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(user.Password));

            var users = MapUser(user, passwordHash, hmac.Key);
            var userResult = await _userRepository.Add(users);
          

            if (userResult == null)
                throw new Exception("Failed to create user");

            return MapToUserResponse(userResult);
        }

        public async Task<UserResponse> UpdateUser(int id, UserRequest user)
        {
            var existingUser = await _userRepository.GetById(id);
            if (existingUser == null)
                throw new UserNotFoundException($"user with ID {id} not found.");

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
            return MapToUserResponse(result);
        }

        public async Task<bool> DeleteUser(int id)
        {
            var existingUser = await _userRepository.GetById(id);
            if (existingUser == null)
                throw new UserNotFoundException($"user with ID {id} not found.");

            await _userRepository.Delete(id);
            return true;
        }

        public async Task<IEnumerable<UserResponse>> GetAllUsers()
        {
            var users = await _userRepository.GetAll();
            return users.Select(MapToUserResponse);
        }

        public async Task<UserResponse> GetUserById(int id)
        {
            var user = await _userRepository.GetById(id);
            if (user == null)
                throw new UserNotFoundException($"User with ID {id} not found.");

            return MapToUserResponse(user);
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

        private UserResponse MapToUserResponse(User user)
        {
            return new UserResponse
            {
                UserId = user.UserId,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address
            };

        }
    }
}