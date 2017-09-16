using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class Pagination<T>
    {
        public IEnumerable<T> Data { get; private set; }
   
        public int PageSize { get; private set; }

        public int Page { get; set; }
   
        public int Pages { get; private set; }

        public bool HasPrev { get { return Page > 1; } }

        public bool HasNext { get { return Page < Pages; } }
      
        public int NextNum
        {
            get
            {
                if (this.HasNext)
                    return (Page + 1);
                else
                    return Pages;
            }
        }

        public int PrevNum
        {
            get
            {
                if (this.HasPrev)
                    return (Page - 1);
                else
                    return 1;
            }
        }

        public Pagination(IEnumerable<T> data, int pageSize, int page = 1)
        {
            this.PageSize = pageSize > 1 ? pageSize : 1;
            this.Data = data;
            this.Page = page;
            Pages = (int)Math.Ceiling(data.Count() / (double)pageSize);
        }

        public IEnumerable<T> Items()
        {
            return Data.Skip((Page - 1) * PageSize).Take(PageSize);
        }


    }
}