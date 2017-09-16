using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class AjaxForm
    {
        public string action { get; set; }
        public string pageNo { get; set; }
    }

    public class CommentForm
    {
        public string comment { get; set; }
        public string postId { get; set; }
    }


}