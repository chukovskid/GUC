using System.Collections.Generic;
using System.Linq;
using DatingApp.API.Models;
using Newtonsoft.Json;

namespace DatingApp.API.Data
{
    public class Seed
    {
        public static void SeedUsers(DataContext context){

            if (!context.User.Any()){
                var userData = System.IO.File.ReadAllText("Data/UserSeedData.json");
                var users = JsonConvert.DeserializeObject<List<User>>(userData);
                foreach (var user in users)
                {
                    byte[] passwordHash, passwordSalt;
                    CreatePasswordHash("password", out passwordHash, out passwordSalt);
                    
                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;
                    user.Username = user.Username.ToLower();
                    context.User.Add(user);

                }
                context.SaveChanges();
            }
        }

     private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
           using (var hmac = new System.Security.Cryptography.HMACSHA512()) { // ova ke izbrise se sto e pisano tuka(ima DISPOSE funkcija vo HMACHA512)
               passwordSalt = hmac.Key; // HMAC   vekje mi dava random cod i go zimam normalen random Key, za razlika od passwordHach
               passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)); // celoto ComputeHash(System.Text.Encoding.UTF8.GetBytes(password) e za da dobijam password vo Bajti!!!
           }
        }


    }
}