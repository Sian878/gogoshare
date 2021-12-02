using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GoGoShare.Models;

namespace GoGoShare.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            List<文章> 精選文章 = new SQL任務().文章.Where(x => x.作者用戶_FK != null ).OrderByDescending(x => x.Id).Take(10).ToList();
            return View(精選文章);
        }

    }
}