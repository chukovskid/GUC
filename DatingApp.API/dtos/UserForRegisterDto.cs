using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.dtos
{// DTO se koristi privremeno
    public class UserForRegisterDto // ova mi treba za register vo AuthController
    {
        [Required]
        public string Username {get; set;}
        
        [Required]
        [StringLength(8, MinimumLength = 4, ErrorMessage = "The password must consist 4 to 8 char")]
        public string Password { get; set; }
        
    }
}