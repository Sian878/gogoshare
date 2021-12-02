using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace 第二組期末專題.Models
{
    public class 任務InsertInto<某類別> : 資料庫任務 where 某類別 : Dictionary<string, object>
    {
        public 任務InsertInto(某類別 物件)
        {
            string 資料表名稱 = typeof(某類別).Name;
            List<string> 屬性鍵清單 = new List<string>();
            List<string> 屬性值清單 = new List<string>();
            Dictionary<string, object> 注入鍵值字典 = new Dictionary<string, object>();

            foreach (var 欄位 in 物件)
            {
                string 屬性鍵 = 欄位.Key;
                object 屬性值 = 欄位.Value;

                if (屬性鍵 == "Id") continue;

                屬性鍵清單.Add("[" + 屬性鍵 + "]");
                屬性值清單.Add("@" + 屬性鍵);

                注入鍵值字典.Add("@" + 屬性鍵, 屬性值);
            }

            查詢字串 = "INSERT INTO [" + 資料表名稱 + "] (" + string.Join(", ", 屬性鍵清單) + ") VALUES(" + string.Join(", ", 屬性值清單) + ");";

            注入參數by(注入鍵值字典);
        }
    }
}