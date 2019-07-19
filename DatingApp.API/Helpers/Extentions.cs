using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DatingApp.API.Helpers
{
    public static class Extentions
    {
        
        public static void AddApplicationError(this HttpResponse response, string message){
            response.Headers.Add("Application-Error", message); //Error Message as value
            response.Headers.Add("Access-Control-Expose-Headers", "ApplicationError"); //display this too
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        }

        public static int CalculateAge(this DateTime theDateTime){
            var age = DateTime.Today.Year - theDateTime.Year;
        
            if (theDateTime.AddYears(age) > DateTime.Today){ //check if user had their birthday
                age--;
            }
            return age;
        }

        //Helper to help pass pagination headers to json formating
        public static void AddPagination(this HttpResponse response, int currentPage, int itemsPerPage,
            int totalItems, int totalPages){
                //Fix to cammel case in sending request
                var paginationHeader = new PaginationHeader(currentPage, itemsPerPage, totalItems, totalPages);
                var cammelCaseFormatter = new JsonSerializerSettings();
                cammelCaseFormatter.ContractResolver = new CamelCasePropertyNamesContractResolver();
                response.Headers.Add("Pagination", JsonConvert.SerializeObject(paginationHeader, cammelCaseFormatter));
                response.Headers.Add("Access-Control-Expose-Headers", "Pagination"); //display this too

            }
    }
}