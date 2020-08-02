using System.Collections.Generic;

namespace DatingApp.API.Models
{
    public class NotificationUser
    {
        public int NotificationId { get; set; }
        public Notifications Notification { get; set; }

        public int ReaderId { get; set; }
        public User Reader { get; set; }
    }
}