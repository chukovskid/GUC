using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace DatingApp.API.Models
{ 
    public class User : IdentityUser<int> // polesno e ako e <int> 
    {
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public string Introduction  { get; set; }
        public string LookingFor { get; set; }
        public string ZaUcenikot { get; set; }
        public string City { get; set; }
        public string Sredno { get; set; }
        public string Godina { get; set; } // I, II, III, IV
        public string FaceBook { get; set; }
        public string Instagram { get; set; }

        // public int PhoneNumber { get; set; } // go imas u inheritence
        public ICollection<Photo> Photos { get; set; }
        public ICollection<Like> Likees { get; set; } // Followers
        public ICollection<Like> Likers { get; set; } // Folowing
        public ICollection<Message> MessagesSent { get; set; } 
        public ICollection<Message> MessagesReceived { get; set; } 
        public ICollection<NotificationUser> notificationUsers { get; set; }  // created
        public virtual  ICollection<UserRole> UserRoles { get; set; }
        public Room Room { get; set; }
        public int? RoomId { get; set; }

    }
}