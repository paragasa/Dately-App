using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Helpers
{
    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        /*
        Divide our total results by our pagesize
         */
        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int) Math.Ceiling(count/(double)pageSize);
            this.AddRange(items);
        }

        /**
        Defer execution of our query to items
        1.query count of items aysnc
        2.skip page number amount of pageSize, get next pageSize. to a list of async
        3.!-- return the page list amount of items
         */
        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source,
        int pageNumber, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(); // Skip bypasses certain amount of elements by a specified amount
            return new PagedList<T>(items ,count, pageNumber, pageSize);
        }
    }
}