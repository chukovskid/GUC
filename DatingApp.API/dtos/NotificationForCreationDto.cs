using System;
using System.Collections.Generic;

namespace DatingApp.API.dtos
{
    public class NotificationForCreationDto
    {
         public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public List<int> ReadBy { get; set; }
        public string Content { get; set; }
        public int ReadByCount { get; set; }
        
    }
}