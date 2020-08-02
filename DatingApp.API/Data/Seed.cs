using System.Collections.Generic;
using System.Linq;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace DatingApp.API.Data
{
    public class Seed
    {
        public static void SeedUsers(UserManager<User> userManager, RoleManager<Role> roleManager)
        {

            if (!userManager.Users.Any())
            {
                var userData = System.IO.File.ReadAllText("Data/UserSeedData.json");
                var users = JsonConvert.DeserializeObject<List<User>>(userData);

                // creating Roles // 205
                var roles = new List<Role>
                {
                    new Role{Name = "Member"},
                    new Role{Name = "Admin"},
                    new Role{Name = "Moderator"}
                };

                foreach (var role in roles)
                {
                    roleManager.CreateAsync(role).Wait(); // gi zimam site useri i im stavam Roles
                }

                foreach (var user in users)
                {
                    userManager.CreateAsync(user, "password").Wait(); // Wait() e bidejki imam createASYNC vo static 
                    userManager.AddToRoleAsync(user, "Member"); //  kako (Member)
                }

                // Admin // 205
                var adminUser = new User
                {
                    UserName = "Admin"
                };
                var result = userManager.CreateAsync(adminUser, "adminpassword").Result;

                if (result.Succeeded)
                {
                    var admin = userManager.FindByNameAsync("Admin").Result;
                    userManager.AddToRolesAsync(admin, new[] {"Admin", "Moderator"});
                }
            }
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            { // ova ke izbrise se sto e pisano tuka(ima DISPOSE funkcija vo HMACHA512)
                passwordSalt = hmac.Key; // HMAC   vekje mi dava random cod i go zimam normalen random Key, za razlika od passwordHach
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)); // celoto ComputeHash(System.Text.Encoding.UTF8.GetBytes(password) e za da dobijam password vo Bajti!!!
            }
        }


    }
}