using System;
using System.Collections.Generic;

namespace DatingApp.API.Models
{////////// Ne se dodadeni vo Data Context
    public class Notifications
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public string Content { get; set; }
        public int ReadByCount { get; set; }
        public int CreatedById { get; set; }
        public ICollection<NotificationUser> notificationUsers { get; set; }
        
        // public User CreatedBy { get; set; }
        // public ICollection<int> ReadByUserIds { get; set; } // site Ids od Userite koi go videle
        // public ICollection<User> ReadBy { get; set; } // site useri koi go videle

    }
}