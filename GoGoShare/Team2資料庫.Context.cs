﻿//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace GoGoShare
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class SQL任務 : DbContext
    {
        public SQL任務()
            : base("name=SQL任務")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Hashtag> Hashtag { get; set; }
        public virtual DbSet<文章> 文章 { get; set; }
        public virtual DbSet<用戶> 用戶 { get; set; }
        public virtual DbSet<旅程包> 旅程包 { get; set; }
        public virtual DbSet<旅程包_link> 旅程包_link { get; set; }
        public virtual DbSet<提問> 提問 { get; set; }
        public virtual DbSet<評級> 評級 { get; set; }
        public virtual DbSet<圖片> 圖片 { get; set; }
    }
}
