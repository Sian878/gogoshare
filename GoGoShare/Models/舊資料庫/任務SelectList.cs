using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace 第二組期末專題.Models
{
    public class 任務SelectList<某類別> : 資料庫任務 where 某類別 : Dictionary<string, object>, new()
    {
        public 任務SelectList() : base()
        {
            查詢字串 = "SELECT * FROM [" + typeof(某類別).Name + "];";
        }

        public 任務SelectList(string 查詢字串) : base(查詢字串) {}

        new public List<某類別> Get()
        {
            List<某類別> 字典清單 = new List<某類別>();

            When讀取到一筆資料 = (資料讀取器) =>
            {
                某類別 字典 = new 某類別();
                for (int i = 0; i < 資料讀取器.FieldCount; i++)
                {
                    string 欄位標題 = 資料讀取器.GetName(i);
                    object 儲存格值 = 資料讀取器[i];
                    字典.Add(欄位標題, 儲存格值);
                }
                字典清單.Add(字典);
            };

            讀取資料庫();

            return 字典清單;
        }

        public List<某類別> GetBy(string 外鍵欄位名稱)
        {
            List<某類別> 某類別清單 = new List<某類別>();

            When讀取到一筆資料 = (資料讀取器) =>
            {
                某類別清單.Add(new 任務SelectById<某類別>((int)資料讀取器[外鍵欄位名稱]).Get());
            };

            讀取資料庫();

            return 某類別清單;
        }
    }
}