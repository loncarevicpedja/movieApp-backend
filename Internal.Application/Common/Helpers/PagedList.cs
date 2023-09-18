using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internal.Application.Common.Helpers
{
    public class PagedList<T>
    {
        public PagedList(List<T> items, int page, int pageSize, int totalCount)
        {
            Items = items;
            Page = page;
            PageSize = pageSize;
            TotalCount = totalCount;
        }

        public List<T> Items { get; }
        public int Page { get; }
        public int PageSize { get; }
        public int TotalCount { get; }
        public bool HasNextPage => Page * PageSize < TotalCount;
        public bool HasPreviousPage => Page > 1;

        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> query, int page, int size)
        {
            int totalCount = await query.CountAsync();
            var items = await query.Skip((page - 1) * size)
                                    .Take(size)
                                    .ToListAsync();

            return new(items, page, size, totalCount);
        }
    }
}
