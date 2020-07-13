using System;

namespace DatingApp.API.dtos
{
    public class MessageForReturnDto // pri Send ova go prakja userot 
    {
        public int Id { get; set; }

        // sender
        public int SenderId { get; set; }
        public string SenderKnownAs { get; set; }
        public string SenderPhotoUrl { get; set; }


        // Recipient
        public int RecipientId { get; set; }
        public string RecipientKnownAs { get; set; }
        public string RecipientPhotoUrl { get; set; }


        // Message
        public string Content { get; set; }
        public bool IsRead { get; set; }
        public DateTime? DateRead { get; set; }
        public DateTime MessageSent { get; set; } 

    }
}