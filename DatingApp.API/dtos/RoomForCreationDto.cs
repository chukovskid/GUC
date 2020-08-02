using System;
using System.Collections.Generic;

namespace DatingApp.API.dtos
{
    public class RoomForCreationDto
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int Beds { get; set; }
        public int Floor { get; set; }
        public int occupiedBeds { get; set; }
        public List<int> StudentIds { get; set; }
        public DateTime DateAdded { get; set; }

    }

}