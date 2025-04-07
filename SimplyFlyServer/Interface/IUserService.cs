using SimplyFlyServer.Models;
using SimplyFlyServer.Models.DTOs;

namespace SimplyFlyServer.Interface
{
    public interface IUserService
    {
        Task<UserResponse> AddUser(UserRequest user);
        Task<UserResponse> UpdateUser(int id, UserRequest user);
        Task<bool> DeleteUser(int id);
    }
}
