namespace DatingApp.API.Helpers
{
    public class MessageParams
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

        public string MessageContainer { get; set; } = "Unread";

    }
}