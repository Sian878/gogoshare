using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GoGoShare;

namespace GoGoShare.Models
{
    public class PostPage
    {
        public 文章 文章 { get; set; }

        public string actionUrl { 
            get
            {
                if (文章.Id != 0) return "../Post/Update";
                return "../Post/New";
            }
        }
    }
}