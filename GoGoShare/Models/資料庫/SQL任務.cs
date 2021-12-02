using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace GoGoShare
{
    public partial class SQL任務
    {
        //建構函式 開始
        public SQL任務(string 查詢字串 = null, object Sql指令參數 = null)
        {
            this.連線字串 = @"Data Source=.;Initial Catalog=team2;Integrated Security=True";

            this.查詢字串 = 查詢字串;

            this.Sql指令參數 = Sql指令參數;

            this.操作流程 = delegate (SqlCommand 指令)
            {
                指令.ExecuteNonQuery();
            };
        }
        //建構函式 結束

        //物件屬性 開始
        public string 連線字串 { get; set; }

        public string 查詢字串 { get; set; }

        public object Sql指令參數 { get; set; }

        public Action<SqlCommand> 操作流程 { get; set; }
        //物件屬性 結束

        //物件方法 開始
        public void 執行()
        {
            using (SqlConnection 連線 = new SqlConnection(連線字串))
            {
                連線.Open();

                SqlCommand 指令 = new SqlCommand(查詢字串, 連線);

                if (Sql指令參數 != null)
                {
                    指令 = (Sql指令參數 is Dictionary<string, object>) ? 注入參數字典(指令) : 注入參數物件(指令);
                }

                this.操作流程(指令);

                連線.Close();
            }
        }

        private SqlCommand 注入參數字典(SqlCommand 指令)
        {
            foreach (var 欄位 in Sql指令參數 as Dictionary<string, object>)
            {
                string 注入鍵 = 欄位.Key;
                object 注入值 = 欄位.Value;

                if (注入值 == null) continue;

                if (注入值.GetType() == typeof(DateTime))
                {
                    注入值 = ((DateTime)注入值).ToString("yyyy-MM-dd HH:mm:ss");
                }

                指令.Parameters.AddWithValue(注入鍵, 注入值);
            }
            return 指令;
        }

        private SqlCommand 注入參數物件(SqlCommand 指令)
        {
            foreach (PropertyInfo 屬性 in Sql指令參數.GetType().GetProperties())
            {
                string 注入鍵 = 屬性.Name;
                object 注入值 = 屬性.GetValue(Sql指令參數);

                if (注入值 == null) continue;

                if (注入值.GetType() == typeof(DateTime))
                {
                    注入值 = ((DateTime)注入值).ToString("yyyy-MM-dd HH:mm:ss");
                }

                指令.Parameters.AddWithValue(注入鍵, 注入值);
            }
            return 指令;
        }

        public SQL任務 加入參數(object 參數鍵值)
        {
            this.Sql指令參數 = 參數鍵值;

            return this;
        }

        public List<Dictionary<string, object>> 讀取()
        {
            List<Dictionary<string, object>> 字典清單 = new List<Dictionary<string, object>>();

            this.讀取(delegate (SqlDataReader 資料讀取器)
            {
                Dictionary<string, object> 字典 = new Dictionary<string, object>();
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

        public object 讀取1格()
        {
            object 查詢結果 = new object();

            this.操作流程 = delegate (SqlCommand 指令)
            {
                查詢結果 = 指令.ExecuteScalar();
            };

            this.執行();

            return 查詢結果;
        }

        public void 讀取(Action<SqlDataReader> When讀取到一筆資料)
        {
            this.操作流程 = delegate (SqlCommand 指令)
            {
                SqlDataReader 資料讀取器 = 指令.ExecuteReader();

                while (資料讀取器.Read())
                {
                    When讀取到一筆資料(資料讀取器);
                }
            };

            this.執行();
        }
    }
}