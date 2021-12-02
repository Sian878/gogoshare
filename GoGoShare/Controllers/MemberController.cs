using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GoGoShare.Models;

namespace GoGoShare.Controllers
{
    public class MemberController : Controller
    {
        // GET: Member
        //會員編輯資料顯示
        public ActionResult Index()
        {
            if (Session["ID"] is int)
            {
                用戶 x = new SQL任務().用戶.Find(Session["ID"]);
                ViewBag.kk = x.大頭貼;
                return View(x);
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Update()
        {
            var 用戶更新 = new SQL任務();
            用戶 x = 用戶更新.用戶.Find(Session["ID"]);

            用戶更新.用戶.Attach(x);
            x.帳號 = Request.Form["Email"];
            x.名字 = Request.Form["Name"];
            x.手機 = Request.Form["Phone"];
            x.Id = Convert.ToInt32(Request.Form["Id"]);

            HttpPostedFileBase f = Request.Files["myfile"];
            if (Request.Files.Count > 0)
            {
                try
                {
                    int index = f.FileName.LastIndexOf('.');
                    if (index == -1) throw new Exception();
                    string fileName = "uploads/" + DateTime.Now.ToString("yyyyMMddhhmmss") + f.FileName.Substring(index);
                    f.SaveAs(Server.MapPath("~/images/" + fileName));

                    SQL任務 增加圖片 = new SQL任務();
                    增加圖片.圖片.Add(new 圖片() {
                        上傳用戶_FK = Convert.ToInt32(Session["ID"]),
                        路徑 = fileName
                    });
                    增加圖片.SaveChanges();

                    x.大頭貼 = fileName;
                }
                catch { };
            }
            用戶更新.SaveChanges();

            Session["Name"] = x.名字;
            return RedirectToAction("Index", "Home");
        }

        //登出
        public ActionResult 登出()
        {
            Session["ID"] = "";
            Session["Name"] = "";
            return RedirectToAction("Index", "Home");
        }
    }
}