using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GoGoShare.Models;

namespace GoGoShare.Controllers
{
    public class SearchController : Controller
    {
        // GET: Search
        public ActionResult Index()
        {
            return View(new Search(Request.Params, (Session["ID"] is int) ? Convert.ToInt32(Session["ID"]) : 0));
        }

        public ActionResult By(FormCollection form)
        {
            string where = "/Search?";
            where += 是空嗎(form["result"]) ? "" : $"result={ form["result"]}&";
            where += 是空嗎(form["active"]) ? "" : $"active={form["active"]}&";
            where += 是空嗎(form["starttime"]) ? "" : $"starttime={form["starttime"]}&";
            where += 是空嗎(form["endtime"]) ? "" : $"endtime={form["endtime"]}&";
            where += 是空嗎(form["point"]) ? "" : $"point={form["point"]}&";
            return Redirect(where.TrimEnd('&') + "#搜尋");

            bool 是空嗎(string input)
            {
                return string.IsNullOrEmpty(input) || (input == "null");
            };
        }

        //用戶新增收藏
        public string 加入收藏(int? id)
        {
            try
            {
                SQL任務 加入收藏任務 = new SQL任務();
                文章 用戶Favorite = 加入收藏任務.文章.Find(id);
                用戶 用戶 = 加入收藏任務.用戶.Find(Session["ID"]);
                if (用戶.文章.Contains(用戶Favorite))
                {
                    用戶.文章.Remove(用戶Favorite);
                }
                else
                {
                    用戶.文章.Add(用戶Favorite);
                }
                加入收藏任務.SaveChanges();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return null;
        }
    }
}