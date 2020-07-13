using System;

namespace DatingApp.API.dtos
{
    public class MessageForCreationDto // pri Send ova go prakja userot 
    {
        public int SenderId { get; set; }
        public int RecipientId { get; set; }
        public DateTime MessageSent { get; set; } // created
        public string Content { get; set; }

        public MessageForCreationDto()
        {
            MessageSent = DateTime.Now;
        }


    }
}