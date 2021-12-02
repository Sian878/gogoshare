using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using GoGoShare;

namespace GoGoShare.Models
{
    public class Search
    {
        public Search(NameValueCollection name, int id = 0)
        {
            this.name = name;
            this.id = id;
            輪播文章 = new SQL任務().文章.Where(x => x.Id == 1 || x.Id == 6 || x.Id == 7).ToList();
        }

        NameValueCollection name { get; set; }
        int id { get; set; }

        public List<文章> 輪播文章 { get; set; }

        public List<文章> 收藏文章 { 
            get
            {
                return (id == 0) ? new List<文章>() : new SQL任務().用戶.Find(id).文章.ToList();
            }
        }

        public List<文章> 搜尋結果 {
            get
            {
                if (name["pack"] != null)
                {
                    var 旅程包_link = new SQL任務().旅程包.Find(Convert.ToInt32(name["pack"])).旅程包_link.OrderBy(x => x.索引);
                    var result = new List<文章>();
                    foreach (var link in 旅程包_link)
                    {
                        result.Add(link.文章);
                    }
                    return result;
                }

                List<文章> 全部文章 = new SQL任務().文章.ToList();

                if (name["user"] != null)
                    全部文章.RemoveAll(文章 => {
                        return 文章.作者用戶_FK.ToString() != name["user"];
                    });

                if (name["hashtag"] != null)
                    全部文章.RemoveAll(文章 => {
                        List<Hashtag> Hashtag清單 = 文章.Hashtag.ToList();
                        return !Hashtag清單.Exists(Hashtag => Hashtag.Id.ToString() == name["hashtag"]);
                    });

                if (name["favorite"] != null)
                    全部文章.RemoveAll(文章 => {
                        List<用戶> 收藏用戶清單 = 文章.用戶.ToList();
                        return !收藏用戶清單.Exists(收藏用戶 => 收藏用戶.Id.ToString() == name["favorite"]);
                    });


                if (name["result"] != null)
                    全部文章.RemoveAll(文章 => {
                        bool 關鍵字符合 = false;
                        關鍵字符合 = 文章.標題.Contains(name["result"])
                                  || 文章.內容.Contains(name["result"]);
                        return !關鍵字符合;
                    });

                if (name["active"] != null)
                    全部文章.RemoveAll(文章 => {
                        var AAA = 文章.類型;
                        return !文章.類型.Contains(name["active"]);
                    });

                if (name["starttime"] != null)
                    全部文章.RemoveAll(文章 => {
                        return !(文章.日期起始 > Convert.ToDateTime(name["starttime"].ToString()));
                    });

                if (name["endtime"] != null)
                    全部文章.RemoveAll(文章 => {
                        return !(文章.日期結束 < Convert.ToDateTime(name["endtime"].ToString()));
                    });

                if (name["point"] != null)
                    全部文章.RemoveAll(文章 => {
                        return !(文章.點數 <= Convert.ToInt32(name["point"]));
                    });

                return 全部文章;
            }
        }

        public string 標題
        {
            get {
                if (name["user"] != null)
                    return "\"" + new SQL任務().用戶.Find(Convert.ToInt32(name["user"])).名字 + "\"創作的文章";

                if (name["hashtag"] != null)
                    return "包含Hashtag: \"" + new SQL任務().Hashtag.Find(Convert.ToInt32(name["hashtag"])).名稱 + "\"的文章";

                if (name["favorite"] != null)
                    return "\"" + new SQL任務().用戶.Find(Convert.ToInt32(name["favorite"])).名字 + "\"收藏的文章";

                if (name["active"] != null)
                    return (name["trip"] == "room") ? "出租房間"
                        : (name["trip"] == "food") ? "共享美食"
                        : (name["trip"] == "trip") ? "分享旅程" : "主題旅遊";

                return "主題旅遊";
            }
        }
    }
}