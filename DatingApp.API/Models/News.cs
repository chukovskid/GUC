using System;
using System.Collections.Generic;

namespace DatingApp.API.Models
{////////// Ne se dodadeni vo Data Context
    public class News
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public DateTime Created { get; set; }
        public ICollection<User> ReadBy { get; set; }
        public string Content { get; set; }
        public int ReadByCount { get; set; }

    }
}