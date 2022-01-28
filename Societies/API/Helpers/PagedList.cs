using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Helpers
{
    public class PagedList<T>: List<T>
    {
        //Another nonsense class for database we might not need

        public PagedList(IEnumerable<T> items, int pagenumber, int count, int pagesize)
        {
            CurrentPage = pagenumber;
            TotalPages = (int) Math.Ceiling(count/(double)pagesize);
            Pagesize = pagesize;
            TotalCount = count;
            AddRange(items);
        }

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int Pagesize { get; set; }
        public int TotalCount { get; set; }

        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PagedList<T>(items, count, pageNumber, pageSize); 
        }
    }
}
