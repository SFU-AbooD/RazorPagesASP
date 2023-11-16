using Efcorelearningrazorpages.Models;
using Microsoft.EntityFrameworkCore;

namespace Efcorelearningrazorpages
{
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }
        public PaginatedList(List<T> items, int count, int pageIndex, int pagesize)
        {
            PageIndex = pageIndex; 
            TotalPages = (int)Math.Ceiling((double)count / pagesize);
            this.AddRange(items); 
        }
        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;
        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageindex, int pagesize)
        {
            int count = await source.CountAsync();
            var items = await source.Skip((pageindex - 1) * pagesize).Take(pagesize).ToListAsync();
            return new PaginatedList<T>(items, count, pageindex, pagesize);
        }
    }

}
