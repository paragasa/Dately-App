using System;
using Microsoft.AspNetCore.Http;

namespace DatingApp.API.Dtos
{
    public class PhotoForCreationDto
    {
        

        public string Url { get; set; }

        public IFormFile File { get; set; }  // Get File From system

        public string Description { get; set; }

        public DateTime DateAdded { get; set; }
        public string PublicId { get; set; }

        public bool IsMain { get; set; }


        public PhotoForCreationDto(){
            DateAdded = DateTime.Now;
        }
    }
}