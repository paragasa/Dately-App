using System;
using System.Linq;
using DatingApp.API.Models;

namespace DatingApp.API.Helpers
{
    public class UserParams
    {
        private const int MaxPageSize = 30;
        public int PageNumber { get; set; } =1; //unlsess requested set to 1
        private int pageSize = 10;

        //if Page size requested is less than max set page size to this value
        public int PageSize { 
            get{return pageSize;}
            set{pageSize = (value > MaxPageSize)? MaxPageSize : value;}
            }
        
        public int UserId {get;set;}


        public string Gender {get;set;}

        public int MinAge { get; set; }= 18;

        public int MaxAge { get; set; } = 99;


        public string OrderBy { get; set; }

        public bool Likees { get; set; } = false;

        public bool Likers { get; set; } = false;

        internal IQueryable<User> OrderByDescending(Func<object, object> p)
        {
            throw new NotImplementedException();
        }
    }

}