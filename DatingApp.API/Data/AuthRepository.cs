using System;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class AuthRepository : IAuthRepository
    {

        private DataContext _context;
        public AuthRepository(DataContext context) //#1 za pristap do context 
        {
            _context = context;
        }



         public async Task<User> Login(string username, string password) // sakame password da bide HASH
        {   
            // vo Login za nav slikata mora da pratam i photo 117 
           var user = await _context.User.Include(p => p.Photos).FirstOrDefaultAsync(x => x.Username == username); // najdi go Userot vo _context koj ima ist Username
           
            if (user == null){
                 return null;
            }
            
            if (!VeryfyPasswordHash(password, user.PasswordHash, user.PasswordSalt)) // ako NE e tocen..ne postoi
            return null;

            return user;
            
        }

        private bool VeryfyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
              using(var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt)) { // passwordSalt e klucot za da go najdam Hash
               var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)); // celoto ComputeHash(System.Text.Encoding.UTF8.GetBytes(password) e za da dobijam password vo Bajti!!!
           // treba => computeHash == passwordHash
                for (int i = 0; i < computeHash.Length; i++){ // moram sekoj bajt posebno bidejki e nele vo Bajti
                    if (computeHash[i] != passwordHash[i]) return false;
                }
           }
           return true;
        }

         public async Task<User> Register(User user, string password)// so ovaa glavno e proces na Sejvnuanje
        {
            byte[] passwordHash, passwordSalt; // vo ovie vrednosti ke stavam hash i salt preku dolnata funk
            CreatePasswordHash(password, out passwordHash, out passwordSalt); // namesto da gi zima vrednostite gi zima kako PRAZNI mesta sto ke im DADE vrednost
       
            user.PasswordHash = passwordHash; // dobieniot password sto go vnesol userot, mu pravam Hash i Salt i mu gi davam na modelot User
            user.PasswordSalt = passwordSalt;

            await _context.User.AddAsync(user); // so ova Asinhrono go dodavam user vo db na User, normalno
            await _context.SaveChangesAsync(); // sekako so sekoja promena na db si sejvnuvam

            return user;       
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
           using (var hmac = new System.Security.Cryptography.HMACSHA512()) { // ova ke izbrise se sto e pisano tuka(ima DISPOSE funkcija vo HMACHA512)
               passwordSalt = hmac.Key; // HMAC   vekje mi dava random cod i go zimam normalen random Key, za razlika od passwordHach
               passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)); // celoto ComputeHash(System.Text.Encoding.UTF8.GetBytes(password) e za da dobijam password vo Bajti!!!
           }
        }

        public async Task<bool> UserExists(string username) // cisto proverka za Username dali postoi
        {
            if (await _context.User.AnyAsync(x => x.Username == username)){
                return true;
            }
            return false;
        }

       
    }
}