using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoGoShare.Models
{
    public class ListPage
    {
        用戶 User { get; set; }

        public ListPage(用戶 user = null) {
            this.User = user;
        }

        public List<文章> 所有文章 {
            get
            {
                if (this.User == null)
                    return new SQL任務().文章.OrderByDescending(x => x.Id).ToList();

                return User.Get創作文章清單();
            }
        }

        public List<旅程包> 所有旅程包
        {
            get
            {
                if (this.User == null)
                    return new SQL任務().旅程包.OrderByDescending(x => x.Id).ToList();

                return User.Get旅程包清單();
            }
        }

        public List<圖片> 所有圖片
        {
            get
            {
                if (this.User == null)
                    return new SQL任務().圖片.OrderByDescending(x => x.Id).ToList();

                return User.圖片.ToList();
            }
        }

        public List<Hashtag> 所有Hashtag
        {
            get
            {
                if (this.User == null)
                    return new SQL任務().Hashtag.OrderByDescending(x => x.Id).ToList();

                return User.Hashtag.ToList();
            }
        }

        public List<評級> 所有評級
        {
            get
            {
                if (this.User == null)
                    return new SQL任務().評級.OrderByDescending(x => x.Id).ToList();

                return User.評級.ToList();
            }
        }

        public List<提問> 所有提問
        {
            get
            {
                if (this.User == null)
                    return new SQL任務().提問.OrderByDescending(x => x.Id).ToList();

                return User.提問.ToList();
            }
        }

        public List<用戶> 所有用戶
        {
            get
            {
                return new SQL任務().用戶.OrderByDescending(x => x.Id).ToList();
            }
        }
    }
}