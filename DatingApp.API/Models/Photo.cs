using System;

namespace DatingApp.API.Models
{
    public class Photo
    {
        public int Id { get; set; }

        public string Url { get; set; }

        public string Description { get; set; }

        public DateTime DateAdded { get; set; }

        public bool isMain { get; set; }

        //ref to each other
        public User User { get; set; }

        //ref user to photo/ if deleted will delte photo/ for referencial-cascade
        public int UserId { get; set; }
    }
}