using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using tawsLibrary.db.table;

namespace tawsLibrary.db
{
    public class Connection : DbContext
    {
        //接続プロパティ

        /// <summary>
        /// テーブル接続情報。
        /// </summary>
        public DbSet<db_base_t>  Table { get; set; }

        /// <summary>
        /// 参照するスキーマ。
        /// </summary>
        public string DefaultSchema { get; private set; }


        //イベント

        /// <summary>
        /// スキーマを変更したい場合はここで変更。
        /// 指定が無いとスキーマ名は『dbo』に設定される。
        /// </summary>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
            => modelBuilder.HasDefaultSchema(DefaultSchema);


        //メソッド

        /// <summary>
        /// コネクションの取得。
        /// </summary>
        static NpgsqlConnection GetConnecting()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            return new NpgsqlConnection(connectionString);
        }


        //コンストラクタ

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="defaultSchema">接続先スキーマ。</param>
        public Connection(string defaultSchema) : base(GetConnecting(), true)
        {
            DefaultSchema = defaultSchema;
        }
    }
}
