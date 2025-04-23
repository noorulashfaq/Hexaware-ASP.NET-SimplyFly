using SimplyFlyServer.Interface;
using SimplyFlyServer.Models.DTOs;
using SimplyFlyServer.Models;

namespace SimplyFlyServer.Service
{
    public class NotificationService : INotificationService
    {
        private readonly IRepository<int, Notification> _notificationRepo;

        public NotificationService(IRepository<int, Notification> notificationRepo)
        {
            _notificationRepo = notificationRepo;
        }

        public async Task<NotificationResponse> AddNotification(NotificatinRequest request)
        {
            var notification = new Notification
            {
                Message = request.Message,
                Status = "Unread"
            };

            var result = await _notificationRepo.Add(notification);

            return new NotificationResponse
            {
                NotificationId = result.NotificationId,
                Message = result.Message,
                Status = result.Status
            };
        }

        public async Task<IEnumerable<NotificationResponse>> GetAllNotifications()
        {
            var notifications = await _notificationRepo.GetAll();
            return notifications.Select(n => new NotificationResponse
            {
                NotificationId = n.NotificationId,
                Message = n.Message,
                Status = n.Status
            });
        }

        public async Task<NotificationResponse> MarkAsRead(int id)
        {
            var notification = await _notificationRepo.GetById(id);
            if (notification == null)
                throw new Exception($"Notification with ID {id} not found.");

            notification.Status = "Read";
            var updated = await _notificationRepo.Update(id, notification);

            return new NotificationResponse
            {
                NotificationId = updated.NotificationId,
                Message = updated.Message,
                Status = updated.Status
            };
        }
        public async Task<bool> DeleteNotification(int id) 
        {
            var notification = await _notificationRepo.GetById(id);
            if (notification == null)
                return false;

            await _notificationRepo.Delete(id);
            return true;
        }
    }
}
