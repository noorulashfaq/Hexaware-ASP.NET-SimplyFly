using SimplyFlyServer.Models.DTOs;

namespace SimplyFlyServer.Interface
{
    public interface INotificationService
    {
        Task<NotificationResponse> AddNotification(NotificatinRequest request);
        Task<IEnumerable<NotificationResponse>> GetAllNotifications();
        Task<NotificationResponse> MarkAsRead(int id);
        Task<bool> DeleteNotification(int id);
    }
}
