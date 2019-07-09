using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;

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
    }
}