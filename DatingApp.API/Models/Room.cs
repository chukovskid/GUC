using System;
using System.Collections.Generic;

namespace DatingApp.API.Models
{
    public class Room
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int Floor { get; set; } 
        public int Beds { get; set; }
        public int occupiedBeds { get; set; }
        public ICollection<User> Student { get; set; }
        public ICollection<RoomPhoto> RoomPhotos { get; set; }
    }
}