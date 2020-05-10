using System;
using Microsoft.AspNetCore.Http;

namespace DatingApp.API.Helpers
{
    public static class Extensions
    {
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            response.Headers.Add("Application-Error", message); // ova e porakata
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error"); // ova i dolnovo imaat cel da ja pustat gornava 
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        } // ova za alertify mislam bese poraki da moes da prakjas messsages
        
        public static int CalculateAge(this DateTime theDateTime){ // Kako ja zima ovaa funkcija u AuthMapperProfile.cs bez da ja povika clasava i kako e povrzano so DateOfBirth
            var age = DateTime.Today.Year - theDateTime.Year;
            if (theDateTime.AddYears(age) > DateTime.Today)
                age--;

            return age;
        }

    }
}