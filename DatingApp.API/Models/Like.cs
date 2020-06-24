namespace DatingApp.API.Models
{
    public class Like
    {
        public int LikerId { get; set; } // Userot Lajknal nekoj DRUG user(LIKER ID) // jas sakam // Following
        public int LikeeId { get; set; } // nekoj Nas ne Lajknal/ drugite sakaat // Follower
        public User Liker { get; set; } // Following i like
        public User Likee { get; set; } // Follower, Who Lime Me
          
    }
}