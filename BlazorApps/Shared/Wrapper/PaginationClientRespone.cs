using System;
using System.Collections.Generic;

namespace Shared.Wrapper
{
    public class PaginationClientRespone<T>
    {

        public PaginationClientRespone(List<T> data, int count, int page, int pageSize)
        {
            DataList = data;
            CurrentPage = page;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalCount = count;
        }

        public List<T> DataList { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public int TotalCount { get; set; }

        public int PageSize { get; set; }

        public bool HasPreviousPage => CurrentPage > 1;

        public bool HasNextPage => CurrentPage < TotalPages;
    }
}