using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GoGoShare.Models;


namespace GoGoShare.Controllers
{
    public class ArticleController : Controller
    {
        // GET: Article
        public ActionResult Index(int? id, int? pack)
        {
            if (id == null)
                return Redirect("/Search");

            Article 貼文ViewModel = new Article
            {
                文章 = new SQL任務().文章.Find(id)
            };

            if (pack != null)
                貼文ViewModel.旅程包 = new SQL任務().旅程包.Find(pack);

            return View(貼文ViewModel);
        }

        public ActionResult NewComment(FormCollection form)
        {
            try
            {
                var 新增評論 = new SQL任務();

                // 將回傳的東西存進資料庫
                評級 新評級 = new 評級();
                新評級.分數 = Convert.ToInt32(form["分數"]);
                新評級.評分用戶_FK = Convert.ToInt32(Session["ID"]);
                新評級.文章_FK = Convert.ToInt32(form["文章_FK"]);
                新評級.評論 = form["評論"];

                新增評論.評級.Add(新評級);
                新增評論.SaveChanges();
            }
            catch {}

            return Redirect("/Article?id=" + form["文章_FK"]);
        }
    }
}