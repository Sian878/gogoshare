using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace GoGoShare.Models
{
    public class 用戶HashtagCRUD
    {
        //UserHashtag 新增
        public void Create(用戶Hashtag a)
        {
            string sql = "INSERT INTO 用戶Hashtag (用戶_FK, Hashtag_FK) VALUES (";
            sql += "N'" + a["用戶_FK"] + "',";
            sql += " N'" + a["Hashtag_FK"] + "')";

            new 資料庫任務(sql).Set();
        }

        //刪除用戶標籤
        public void del(用戶Hashtag a)
        {
            string sql = "DELETE FROM 用戶Hashtag WHERE 用戶_FK="+ a["用戶_FK"] + "and Hashtag_FK="+ a["Hashtag_FK"];
            new 資料庫任務(sql).Set();
        }
    }
}
