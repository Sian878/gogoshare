using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoGoShare.Models
{
    public class 用戶收藏CRUD
    {
        //用戶新增收藏
        public void 新增收藏(用戶Favorite x)
        {
            string sql = "INSERT INTO 用戶Favorite VALUES("+x["收藏文章_FK"] +","+x["用戶_FK"] +")";
            new 資料庫任務(sql).Set();
        }

        //檢查有無重複
        public void 檢查重複(用戶Favorite x)
        {
            string sql = "INSERT INTO 用戶Favorite VALUES(" + x["收藏文章_FK"] + "," + x["用戶_FK"] + ")";
            new 資料庫任務(sql).Set();
        }
    }
}