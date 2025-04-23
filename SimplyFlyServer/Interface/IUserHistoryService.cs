using SimplyFlyServer.Models.DTOs;

namespace SimplyFlyServer.Interface
{
    public interface IUserHistoryService
    {
        Task<IEnumerable<UserHistoryResponse>> GetUserHistory(int userId);
    }
}
