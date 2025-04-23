using System.ComponentModel.DataAnnotations;

namespace SimplyFlyServer.Models
{
    public class Notification
    {
        [Key]
        public int NotificationId { get; set; }

       
        public string Message { get; set; }

        
        public string Status { get; set; } = "Unread";
    }
}
