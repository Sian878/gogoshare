using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;

namespace 第二組期末專題.Models
{
    public class 任務SelectById<某類別> : 資料庫任務 where 某類別 : Dictionary<string, object>, new()
    {
        public 任務SelectById(object Id)
        {
            查詢字串 = "SELECT * FROM [" + typeof(某類別).Name + "] WHERE Id=@ID;";

            注入參數 = delegate (SqlCommand 指令)
            {
                指令.Parameters.AddWithValue("@ID", Id);
                return 指令;
            };
        }


        new public 某類別 Get()
        {
            某類別 字典 = new 某類別();

            When讀取到一筆資料 = delegate (SqlDataReader 資料讀取器)
            {
                for (int i = 0; i < 資料讀取器.FieldCount; i++)
                {
                    string 欄位標題 = 資料讀取器.GetName(i);
                    object 儲存格值 = 資料讀取器[i];
                    字典.Add(欄位標題, 儲存格值);
                }
            };

            讀取資料庫();

            return 字典;
        }
    }
}