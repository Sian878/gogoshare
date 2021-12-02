using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace GoGoShare.Models
{
    public class HashtagCRUD
    {
        //新增Hashtag類別方法
        public void Create(Hashtag p)
        {
            string sql = "INSERT INTO Hashtag (名稱, 類別) VALUES (";
            sql += "N'" + p["名稱"] + "',";
            sql += " N'" + p["類別"] + "')";
            
            new 資料庫任務(sql).Set();
        }

        //資料庫讀取字串
        private List<Hashtag> queryBySql(string sql)
        {
            List<Hashtag> list = new List<Hashtag>();

            new 資料庫任務(sql)
            {
                When讀取到一筆資料 = (reader) =>
                {
                    list.Add(new Hashtag()
                    {
                        Id = (int)reader["id"],
                        名稱 = reader["名稱"].ToString(),
                        類別 = reader["類別"].ToString()
                    });
                }
            }.讀取資料庫();

            return list;
        }

        //印出全部
        public List<Hashtag> queryAll()
        {
            return queryBySql("SELECT * FROM Hashtag");
        }
    }
}