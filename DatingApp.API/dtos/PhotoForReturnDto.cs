using System;

namespace DatingApp.API.dtos 
{
    public class PhotoForReturnDto //108
    {
        public string PublicId { get; set; }
        public int Id { get; set; }
        public string  Url { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public bool IsMain { get; set; }
    }
}