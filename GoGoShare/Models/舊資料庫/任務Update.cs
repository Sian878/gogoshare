using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace 第二組期末專題.Models
{
    public class 任務Update<某類別> : 資料庫任務 where 某類別 : Dictionary<string, object>
    {
        private int 目標Id { get; set; }
        string 資料表名稱 { get; set; }
        Dictionary<string, object> 欲修改欄位 { get; set; }

        public 任務Update(某類別 物件)
        {
            資料表名稱 = typeof(某類別).Name;
            欲修改欄位 = new Dictionary<string, object>();
            Dictionary<string, object> 注入參數鍵值 = new Dictionary<string, object>();

            foreach (var 欄位 in 物件)
            {
                string 屬性鍵 = 欄位.Key;
                object 屬性值 = 欄位.Value;

                if (屬性鍵.Equals("Id"))
                {
                    目標Id = (int)屬性值;
                    continue;
                }

                if (屬性值 == null) {
                    欲修改欄位.Add(屬性鍵, "NULL");
                    //"[" + 屬性鍵 + "]=NULL"
                    continue;
                }

                欲修改欄位.Add(屬性鍵, "@" + 屬性鍵);
                //"[" + 屬性鍵 + "]=@" + 屬性鍵

                注入參數鍵值.Add("@" + 屬性鍵, 屬性值);
            }

            查詢字串 = "UPDATE [" + 資料表名稱 + "] SET " + Get欄位更新值By(欲修改欄位) + " WHERE Id=" + 目標Id + ";";

            注入參數by(注入參數鍵值);
        }

        public void SetBy欄位(string[] 指定欄位)
        {
            更新欲修改欄位By(指定欄位);

            查詢字串 = "UPDATE [" + 資料表名稱 + "] SET " + Get欄位更新值By(欲修改欄位) + " WHERE Id=" + 目標Id + ";";

            Set();
        }

        private string Get欄位更新值By(Dictionary<string, object> 欲修改欄位)
        {
            List<string> 欄位更新值 = new List<string>();

            foreach (KeyValuePair<string, object> 欄位鍵值對 in 欲修改欄位)
            {
                欄位更新值.Add("[" + 欄位鍵值對.Key + "]=" + 欄位鍵值對.Value);
            }

            return string.Join(", ", 欄位更新值);
        }

        private void 更新欲修改欄位By(string[] 指定欄位)
        {
            Dictionary<string, object> 指定更新欄位 = new Dictionary<string, object>();

            foreach (KeyValuePair<string, object> 欄位 in 欲修改欄位)
            {
                if (指定欄位.Contains(欄位.Key))
                {
                    指定更新欄位.Add(欄位.Key, 欄位.Value);
                }
            }
            欲修改欄位 = 指定更新欄位;
        }
    }
}