using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public int PostId { get; set; }
        public String UserName { get; set; }
        public DateTime TimeStamp { get; set; }
    }

}