using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoGoShare
{
    public partial class 文章
    {
        public List<Hashtag> GetHashtag清單()
        {
            return this.Hashtag.ToList();
        }

        public 用戶 Get作者()
        {
            if (this.作者用戶_FK == null)
                return new 用戶() { 名字 = "已刪除的用戶" };

            return new SQL任務().用戶.Find(this.作者用戶_FK);
        }

        public string Get圖片路徑()
        {
            if (this.圖片_FK == 0 || this.圖片_FK == null)
                return "bg_3.jpg";

            return new SQL任務().圖片.Find(this.圖片_FK).路徑;
        }

        public int Get評論數()
        {
            return this.評級.Count;
        }

        public List<評級> Get評級清單()
        {
            return this.評級.ToList();
        }
    }
}