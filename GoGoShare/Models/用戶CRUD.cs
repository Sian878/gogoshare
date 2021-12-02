using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoGoShare.Models
{
    public class 用戶CRUD
    {
        public void 註冊(用戶 x)
        {
            string sql = "INSERT INTO 用戶 (帳號, 密碼, 名字, 手機, 註冊日期, 點數) VALUES (";
            sql += "N'" + x["帳號"] + "', ";
            sql += "'" + x["密碼"] + "', ";
            sql += "N'" + x["名字"] + "', ";
            sql += "'" + x["手機"] + "', ";
            sql += "N'" + x["註冊日期"] + "', ";
            sql += x["點數"] + ")";

            new 資料庫任務(sql).Set();
        }

        //用戶表格讀取字串
        private List<用戶> queryBySql(string sql)
        {
            List<用戶> list = new List<用戶>();

            new 資料庫任務(sql)
            {
                When讀取到一筆資料 = (reader) =>
                {
                    list.Add(new 用戶()
                    {
                        Id = (int)reader["Id"],
                        帳號 = reader["帳號"].ToString(),
                        密碼 = reader["密碼"].ToString(),
                        名字 = reader["名字"].ToString(),
                        手機 = reader["手機"].ToString(),
                        註冊日期 = (string)reader["註冊日期"],
                        大頭貼 = reader["大頭貼"].ToString(),
                        點數 =(int)reader["點數"]
                    }); 
                }
            }.讀取資料庫();

            return list;
        }

        //登入比對
        public List<用戶> 登入(string 帳號,string 密碼)
        {
            return queryBySql("SELECT *  FROM 用戶 WHERE 帳號 LIKE"+" '"+帳號 +"' " +"AND 密碼  LIKE"+" '"+密碼+"'");
        }

        //利用註冊時名字搜尋ID
        public List<用戶> 取ID(string name)
        {
            string sql = "SELECT * FROM 用戶 WHERE 名字 = N" + "'" + name + "'";
            return queryBySql(sql);
        }

        //登入中會員資料抓取
        public 用戶 queryById(int fid)
        {
            string sql = "SELECT * FROM 用戶 WHERE Id = " + fid;
            List<用戶> x = queryBySql(sql);
            if (x.Count == 0)
                return null;
            return x[0];
        }

        //更新會員資料
        public void 更新(用戶 x)
        {
            string sql = "UPDATE 用戶 SET ";
            if (!string.IsNullOrEmpty((string)x["帳號"]))
            {
                sql += "帳號='" + x["帳號"] + "',";
            }
            if (!string.IsNullOrEmpty((string)x["密碼"]))
            {
                sql += "密碼='" + x["密碼"] + "',";
            }
            if (!string.IsNullOrEmpty((string)x["名字"]))
            {
                sql += "名字=N'" + x["名字"] + "',";
            }
            if (!string.IsNullOrEmpty((string)x["手機"]))
            {
                sql += "手機='" + x["手機"] + "',";
            }
            if (!string.IsNullOrEmpty((string)x["大頭貼"]))
            {
                sql += "大頭貼='" + x["大頭貼"] + "',";
            }
            if (sql.Trim().Substring(sql.Trim().Length - 1, 1) == ",")
                sql = sql.Trim().Substring(0,sql.Trim().Length-1);
            sql += " WHERE Id=" + x["Id"];
            new 資料庫任務(sql).Set();
        }
    }
}