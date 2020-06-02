using System;
using System.Security.Claims;
using System.Threading.Tasks;
using DatingApp.API.Data;
using Microsoft.AspNetCore.Mvc.Filters; 
using Microsoft.Extensions.DependencyInjection;

namespace DatingApp.API.Helpers
{
    public class LastUserActivity : IAsyncActionFilter  // 137
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next){
            // next ?
            var resultContext = await next();


            var userIdProba = resultContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            // Koga ke pobaram nekoja od Metodite fo http req kaj AuthControlerot, httpContext.User go vrakja userot koj do naprail istoto
            var userId = int.Parse(resultContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);  // vo Login gi imam metodive
            // Repo za GetUser() / na koj ke mu ja smenam vrednosta za lastActive
            var repo = resultContext.HttpContext.RequestServices.GetService<IDatingRepository>(); // za GetService // using Microsoft.Extensions.DependencyInjection;
           
           
            var user = await repo.GetUser(userId);
            user.LastActive = DateTime.Now;
            await repo.SaveAll();
       
       
        }
        
    }
}