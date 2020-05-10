using System;
using DatingApp.API.Data;
using Microsoft.AspNetCore.Hosting;
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
            {
                var context = services.GetRequiredService<DataContext>();
                context.Database.Migrate(); // ova e mnogu Cool // procitaj so pisuva u migrate() ja puni Db sekoj pat koga ke se uklucime pa duri i ako ja nema nova ke naprai
                Seed.SeedUsers(context); // ja puni bazata ako e prazna (neli takva e funk.)

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
                    webBuilder.UseStartup<Startup>(); // UseStartup(izgleda dadena e) <Startup> mi e classa 
                });
    }
}
