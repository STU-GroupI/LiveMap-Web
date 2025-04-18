using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveMap.Domain.Pagination
{
    public class PaginatedResult<T>
    {
        public PaginatedResult(List<T> items, int? take, int? skip, int totalCount)
        {
            Items = items;
            TotalCount = totalCount;
            TotalPages = (int)(totalCount / (take == null ? 1 : take)) + 1;
            if (skip != null && take != null)
            {
                CurrentPage = (int)(skip / take) + 1;
            }
            else
            {
                CurrentPage = 1;
            }
        }

        public PaginatedResult()
        {
            Items = [];
            TotalPages = 1;
            TotalCount = 0;
            CurrentPage = 1;
        }

        public List<T> Items { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public int CurrentPage { get; set; }

        public static PaginatedResult<T> Default => new();
    }
}
