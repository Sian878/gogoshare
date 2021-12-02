using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GoGoShare.Models;

namespace GoGoShare.Controllers
{
    public class PackController : Controller
    {
        // GET: Pack
        public ActionResult Index()
        {
            if (!(Session["ID"] is int))
                return Redirect("/SignUp");

            return View(new SQL任務().用戶.Find(Session["ID"]));
        }

        public ActionResult New()
        {
            HttpRequestBase post = Request;

            var 新增旅程包 = new SQL任務();

            // 將回傳的東西存進資料庫
            旅程包 新旅程包 = new 旅程包();
            新旅程包.標題 = post["pTitle"];
            新旅程包.作者用戶_FK = Convert.ToInt32(Session["ID"]);
            新旅程包.描述 = post["pContent"];

            新增旅程包.旅程包.Add(新旅程包);
            新增旅程包.SaveChanges();

            var pack = 新增旅程包.用戶.Find(Convert.ToInt32(Session["ID"])).Get最新旅程包();

            string[] postIds = post["packPosts"].Split(',');

            int count = 1;
            foreach (string postId in postIds)
            {
                var 新增旅程連結 = new SQL任務();

                新增旅程連結.旅程包_link.Add(new 旅程包_link()
                {
                    旅程包_FK = pack.Id,
                    文章_FK = Convert.ToInt32(postId),
                    索引 = count
                });

                新增旅程連結.SaveChanges();

                count++;
            }

            return Redirect("/Search?pack=" + pack.Id);
        }
    }
}