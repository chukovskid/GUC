using System;
using DatingApp.API.Data;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DatingApp.API
{
    public class Program
    {
        public static void Main(string[] args) // ako go nema Main ne pocnuva app
        {
           var host =  CreateHostBuilder(args).Build();
            // .Run() go trgnav za da ne se pusti odma
            using(var scope = host.Services.CreateScope()){ // ?
                
                var services = scope.ServiceProvider; //?
             try
            { // Seeding the DB
                var context = services.GetRequiredService<DataContext>();
                var userManager = services.GetRequiredService<UserManager<User>>(); // ova e novo, staveno zaedno so Identity
                var roleManager = services.GetRequiredService<RoleManager<Role>>(); // 205
                context.Database.Migrate(); // ova e mnogu Cool // procitaj so pisuva u migrate() ja puni Db sekoj pat koga ke se uklucime pa duri i ako ja nema nova ke naprai
                Seed.SeedUsers(userManager, roleManager); // ja puni bazata ako e prazna (neli takva e funk.)

            }
            catch (Exception ex)
            {   
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occured in Migration Seeding");
            }                
            }
            host.Run(); // sea neka se pusti 
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>(); 
                });
    }
}
