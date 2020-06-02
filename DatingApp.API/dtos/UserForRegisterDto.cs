using System;
using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.dtos
{// DTO se koristi privremeno
    public class UserForRegisterDto // ova mi treba za register vo AuthController
    {
        [Required]
        public string Gender {get; set;}

        [Required]
        public string Username {get; set;}

        [Required]
        public string KnownAs {get; set;}

        [Required]
        public DateTime DateOfBirth {get; set;}

        [Required]
        public string City {get; set;}

        [Required]
        public string Country {get; set;}
        
        [Required]
        [StringLength(8, MinimumLength = 4, ErrorMessage = "The password must consist 4 to 8 char")]
        public string Password { get; set; }

        private DateTime Created {get; set;}
        private DateTime LastActive {get; set;}

        private UserForRegisterDto(){
            Created = DateTime.Now;
            LastActive = DateTime.Now;
        }
        
    }
}