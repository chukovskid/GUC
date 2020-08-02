using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DatingApp.API.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Diagnostics;
using DatingApp.API.Helpers;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AutoMapper;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace DatingApp.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration) // ova configuration treba da vodi kon AppSetings i appsetings.Developer kade sto e LOGIRANJETO
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) // 200
        {
            IdentityBuilder builder = services.AddIdentityCore<User>(opt =>
            {
                opt.Password.RequireDigit = false;
                opt.Password.RequiredLength = 4;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;
            });
            builder = new IdentityBuilder(builder.UserType, typeof(Role), builder.Services);
            builder.AddEntityFrameworkStores<DataContext>(); // Add and create Userts and it creates Tables in DB
            builder.AddRoleValidator<RoleValidator<Role>>();
            builder.AddRoleManager<RoleManager<Role>>();
            builder.AddSignInManager<SignInManager<User>>(); // 200

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
         .AddJwtBearer(options =>
         {
             options.TokenValidationParameters = new TokenValidationParameters
             {
                 ValidateIssuerSigningKey = true,
                 IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII
                       .GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                 ValidateIssuer = false,
                 ValidateAudience = false
             };  // 36
         });


              services.AddAuthorization( options => { // 207
                options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin")); // za pogolema sigurnost kaj [Authorization = "" vo Controlerite]
                options.AddPolicy("ModeratePhotoRole", policy => policy.RequireRole("Admin", "Moderator"));
            });
            
            // servisi se raboti koi sakame da gi dodademe na Applikacijata, kako sto e preku Link, localhost:4500.Images neka ti tekne vo c# Interfaces go koristevme
            services.AddDbContext<DataContext>(x => x.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
            // mora da mu dademe DB a taa ke bide SQLite so pristap Configuration.GetConnectionString("DefaultConnection") koj ke go zeme od **appsetings.json**
            // i MORA DA INSTALIRAS Mc.EntityFramework.SQLite!


            services.AddControllers(
                options =>
                {
                    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

                    options.Filters.Add(new AuthorizeFilter(policy));
                }
            ) // (Authorisation Policy) // now ALL users have to Authenticate every single method in API //200
            .AddNewtonsoftJson(opt =>
            {
                opt.SerializerSettings.ReferenceLoopHandling =
                Newtonsoft.Json.ReferenceLoopHandling.Ignore; // 76
            }
            );
            services.AddCors(); // ova go stavam za localhost:5000 da ne go cita kako virus ili hak. ti pisit na googleChrome
            services.Configure<CloudinarySettings>(Configuration.GetSection("CloudinarySettings")); // so ova vrednostite od ClaudnarySetings.cs ke bidat isto so fajlovite u appSettings.json file
            services.AddAutoMapper(typeof(DatingRepository).Assembly); // DatingRepository e dodadena kako Klasakoja ke go KORISTI AutoMapper
            // services.AddScoped<IAuthRepository, AuthRepository>(); // sega raboti so UserManager // 204
            services.AddScoped<IDatingRepository, DatingRepository>(); // 72
            services.AddScoped<LastUserActivity>(); // 137


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) // se racuna za MIDDLEWARE
        { // ova e kako tece programata, niz sto sve treba da pomine, biten e redot
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(); // nesto pravi so catching Exeptions
            }
            else
            { // od tua nadole e za GLOBAL EXCEPTIONS
                app.UseExceptionHandler(builder =>
                { // samo ova     app.UseExceptionHandler()   bilo dovolno ama dodadovme povekje
                    builder.Run(async context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                        var error = context.Features.Get<IExceptionHandlerFeature>();
                        if (error != null)
                        {
                            context.Response.AddApplicationError(error.Error.Message);
                            await context.Response.WriteAsync(error.Error.Message);
                        }


                    });


                });


            }

            // app.UseHttpsRedirection(); // go iskomentirav ova za da ne slusa https

            // ovie dole fun, routing, auth, endpoint se od MVC(porano bile zaedno vo .net 2.0)
            app.UseRouting();

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()); // ova e sto dozvoluvame, koj bilo Metod i koj bilo Header
                                                                                    // vo 112 spomnuvame deka ima problem co acces vo 5000 od 4200 poradi credentials. toa se resava ako dodademe AllowAnyCredential() ama ke dozvoli svasta
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
