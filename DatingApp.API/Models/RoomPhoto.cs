using System;

namespace DatingApp.API.Models
{
    public class RoomPhoto
    {
        public int Id { get; set; }
        public string  Url { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public string PhotoId { get; set; }
        public Room Room { get; set; }
        public int RoomId { get; set; }
        
    }
}