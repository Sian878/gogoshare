using GoGoShare.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GoGoShare.Controllers
{
    public class PostController : Controller
    {
        // GET: Post
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                文章 新文章 = new 文章();
                新文章.標題 = "";
                新文章.內容 = "";
                新文章.日期起始 = DateTime.Now;
                新文章.日期結束 = DateTime.Now;
                新文章.地點 = "";
                新文章.接待人數 = 1;
                新文章.類型 = "";
                新文章.點數 = 0;

                return View(new PostPage() { 文章 = 新文章 });
            }

            var post = new SQL任務().文章.Find(id);

            return View(new PostPage() { 文章 = post });
        }

        public string New()
        {
            try
            {
                圖片 image = new 圖片();

                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[i];
                    var 副檔名點索引 = file.FileName.LastIndexOf('.');
                    if (副檔名點索引 == -1) continue;
                    var 檔名 = DateTime.Now.ToString("yyyyMMddhhmmss") + file.FileName.Substring(副檔名點索引);

                    file.SaveAs(Server.MapPath("~/images/uploads/") + 檔名);

                    SQL任務 新增圖片任務 = new SQL任務();
                    新增圖片任務.圖片.Add(new 圖片()
                    {
                        上傳用戶_FK = Convert.ToInt32(Session["ID"]),
                        路徑 = "uploads/" + 檔名
                    });
                    新增圖片任務.SaveChanges();

                    image = 新增圖片任務.用戶.Find(Session["ID"]).Get最新圖片();
                }

                SQL任務 新增文章任務 = new SQL任務();
                HttpRequestBase post = Request;
                // 將回傳的東西存進資料庫
                文章 新文章 = new 文章();
                新文章.標題 = post["pTitle"];
                新文章.作者用戶_FK = Convert.ToInt32(Session["ID"]);
                新文章.內容 = post["pContent"];
                新文章.日期起始 = Convert.ToDateTime(post["pStartDate"]);
                新文章.日期結束 = Convert.ToDateTime(post["pEndDate"]);
                新文章.圖片_FK = image.Id;
                新文章.地點 = post["pAddress"];
                新文章.接待人數 = Convert.ToInt32(post["pplNumber"]);
                新文章.類型 = string.IsNullOrEmpty(post["ptype"]) ? "" : post["ptype"];
                新文章.點數 = Convert.ToInt32(post["pPoint"]);
                新文章.文章註冊時間 = DateTime.Now;
                新增文章任務.文章.Add(新文章);
                新增文章任務.SaveChanges();

                string[] hashtagIds = post["hashtags"].Split(',');
                foreach (string hashtagId in hashtagIds)
                {
                    新文章.Hashtag.Add(新增文章任務.Hashtag.Find(Convert.ToInt32(hashtagId)));
                }
                新增文章任務.SaveChanges();

                var 新增點數任務 = new SQL任務();
                新增點數任務.用戶.Find(Session["ID"]).點數 += 1000;
                新增點數任務.SaveChanges();
            }
            catch (Exception e)
            {
                return e.Message;
            }

            return null;
        }

        public string Update()
        {
            //try
            //{
                圖片 image = null;

                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[i];
                    var 副檔名點索引 = file.FileName.LastIndexOf('.');
                    if (副檔名點索引 == -1) continue;
                    var 檔名 = DateTime.Now.ToString("yyyyMMddhhmmss") + file.FileName.Substring(副檔名點索引);

                    file.SaveAs(Server.MapPath("~/images/uploads/") + 檔名);

                    SQL任務 新增圖片任務 = new SQL任務();
                    新增圖片任務.圖片.Add(new 圖片()
                    {
                        上傳用戶_FK = Convert.ToInt32(Session["ID"]),
                        路徑 = "uploads/" + 檔名
                    });
                    新增圖片任務.SaveChanges();

                    image = 新增圖片任務.用戶.Find(Session["ID"]).Get最新圖片();
                }

                SQL任務 修改文章任務 = new SQL任務();
                HttpRequestBase post = Request;
                // 將回傳的東西存進資料庫
                文章 舊文章 = 修改文章任務.文章.Find(Convert.ToInt32(post["id"]));
                修改文章任務.文章.Attach(舊文章);
                舊文章.標題 = post["pTitle"];
                舊文章.內容 = post["pContent"];
                舊文章.日期起始 = Convert.ToDateTime(post["pStartDate"]);
                舊文章.日期結束 = Convert.ToDateTime(post["pEndDate"]);
                舊文章.圖片_FK = (image != null) ? image.Id : 舊文章.圖片_FK;
                舊文章.地點 = post["pAddress"];
                舊文章.接待人數 = Convert.ToInt32(post["pplNumber"]);
                舊文章.類型 = string.IsNullOrEmpty(post["ptype"]) ? "" : post["ptype"];
                舊文章.點數 = Convert.ToInt32(post["pPoint"]);

                舊文章.Hashtag.Clear();

            if (post["hashtags"] != "")
            {
                string[] hashtagIds = post["hashtags"].Split(',');
                foreach (string hashtagId in hashtagIds)
                {
                    舊文章.Hashtag.Add(修改文章任務.Hashtag.Find(Convert.ToInt32(hashtagId)));
                }
            }

                修改文章任務.SaveChanges();
            //}
            //catch (Exception e)
            //{
            //    return e.Message;
            //}

            return null;
        }

        public JsonResult GetHashtags()
        {
            return Json(new SQL任務("SELECT * FROM [HASHTAG]").讀取(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddHashtag()
        {
            Dictionary<string, object> result;
            try
            {
                HttpRequestBase post = Request;
                // 將回傳的東西存進資料庫
                Hashtag 新Hashtag = new Hashtag()
                {
                    名稱 = post["名稱"],
                    類別 = post["類別"]
                };

                SQL任務 新增任務 = new SQL任務();
                新增任務.Hashtag.Add(新Hashtag);
                新增任務.SaveChanges();

                result = new SQL任務("SELECT TOP 1 [Hashtag].* FROM [Hashtag] ORDER BY [Id] DESC").讀取()[0];
            }
            catch (Exception e)
            {
                return Json(e, JsonRequestBehavior.AllowGet);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}