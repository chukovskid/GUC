using System;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DatingApp.API.Helpers
{
    public static class Extensions
    {
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            response.Headers.Add("Application-Error", message); // ova e porakata
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error"); // ova i dolnovo imaat cel da ja pustat gornava 
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        } // ova alertify mislam bese za poraki da prakjas messsages


        public static void AddPagination(this HttpResponse response,
         int currentPage, int itemsPerPage, int totalItems, int totalPages) // 141
        { 
            // za camel case da gi napravam infrmacii te vo PaginationHeader
                var camelCaseFormater = new JsonSerializerSettings(); 
                camelCaseFormater.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    // dole kaj SerialieObject dodavam vo zagradite (paginationHeader, *camelCaseFormater*)
                    // bez ova ke imas problem vo SPA pri vlecenje informaciive (itemsPerPage, totalPages.. itn)
            // do tuka e za camel


            var paginationHeader = new PaginationHeader(currentPage, itemsPerPage, totalItems, totalPages); // ova KE go vratam u HEADER
            response.Headers.Add("Pagination", JsonConvert.SerializeObject(paginationHeader, camelCaseFormater)); // Informaciite za Pageing gi pustam u HEADEROT
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination"); // za da nemam COUSE ERROR
        }


        public static int CalculateAge(this DateTime theDateTime)
        { // Kako ja zima ovaa funkcija u AuthMapperProfile.cs bez da ja povika clasava i kako e povrzano so DateOfBirth
            var age = DateTime.Today.Year - theDateTime.Year;
            if (theDateTime.AddYears(age) > DateTime.Today)
                age--;

            return age;
        }

    }
}