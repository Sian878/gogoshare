using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using 第二組期末專題.Models;

namespace 第二組期末專題
{
    public static partial class 資料庫
    {
        public static List<某類別> 讀取表<某類別>(string 查詢字串, object 欲注入鍵值 = null) where 某類別 : Dictionary<string, object>, new()
        {
            try
            {
                List<某類別> 字典清單 = new List<某類別>();

                new SQL任務(查詢字串, 欲注入鍵值).讀取(delegate (SqlDataReader 資料讀取器)
                {
                    某類別 字典 = new 某類別();
                    for (int i = 0; i < 資料讀取器.FieldCount; i++)
                    {
                        string 欄位標題 = 資料讀取器.GetName(i);
                        object 儲存格值 = 資料讀取器[i];

                        if (儲存格值.GetType() == typeof(DateTime))
                        {
                            儲存格值 = ((DateTime)儲存格值).ToString("yyyy-MM-dd HH:mm:ss");
                        }

                        try // INNER JOIN 排除相同欄位(通常是ID)
                        {
                            字典.Add(欄位標題, 儲存格值);
                        }
                        catch { }
                    }
                    字典清單.Add(字典);
                });

                return 字典清單;
            }
            catch
            {
                return new List<某類別>();
            }
        }

        public static List<Dictionary<string, object>> 讀取(string 查詢字串, object 注入鍵值 = null)
        {
            return 讀取表<Dictionary<string, object>>(查詢字串, 注入鍵值);
        }

        public static List<某類別> 讀取<某類別>(string 查詢條件 = null, object 注入鍵值 = null) where 某類別 : Dictionary<string, object>, new()
        {
            string 查詢字串 = $"SELECT * FROM [{typeof(某類別).Name}]";

            if (!string.IsNullOrEmpty(查詢條件)) 查詢字串 += $" {查詢條件}";

            return 讀取表<某類別>(查詢字串, 注入鍵值);
        }

        public static 某類別 讀取<某類別>(object ID) where 某類別 : Dictionary<string, object>, new()
        {
            string 查詢字串 = $"SELECT TOP 1 * FROM [{typeof(某類別).Name}] WHERE [Id]=@ID";

            return 讀取表<某類別>(查詢字串, new { ID }).FirstOrDefault();
        }

        public static List<某類別> 讀取<某類別>(List<object> 外鍵表, string AAA) where 某類別 : Dictionary<string, object>, new()
        {
            string 查詢字串 = $"SELECT TOP 1 * FROM [{typeof(某類別).Name}] WHERE [Id]=@ID";

            return 讀取表<某類別>(查詢字串);
        }


        public static int 計數<某類別>(string 查詢條件 = null, object 注入鍵值 = null) where 某類別 : Dictionary<string, object>, new()
        {
            string 查詢字串 = $"SELECT COUNT(*) FROM [{typeof(某類別).Name}]";

            if (!string.IsNullOrEmpty(查詢條件)) 查詢字串 += $" {查詢條件}";

            return (int)new SQL任務(查詢字串, 注入鍵值).讀取1格();
        }


        public static List<T> 讀取<T>(this SQL任務 任務) where T : Dictionary<string, object>, new()
        {
            return 讀取表<T>(任務.查詢字串);
        }
    }
}