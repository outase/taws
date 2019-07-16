namespace tawsLibrary.db
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using tawsLibrary.db.table;

    public class edm_user_t : DbContext
    {
        // コンテキストは、アプリケーションの構成ファイル (App.config または Web.config) から 'edm_user_t' 
        // 接続文字列を使用するように構成されています。既定では、この接続文字列は LocalDb インスタンス上
        // の 'tawsLibrary.db.edm_user_t' データベースを対象としています。 
        // 
        // 別のデータベースとデータベース プロバイダーまたはそのいずれかを対象とする場合は、
        // アプリケーション構成ファイルで 'edm_user_t' 接続文字列を変更してください。
        public edm_user_t()
            : base("name=ConnectionString2")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");
            base.OnModelCreating(modelBuilder);
        }

        // モデルに含めるエンティティ型ごとに DbSet を追加します。Code First モデルの構成および使用の
        // 詳細については、http://go.microsoft.com/fwlink/?LinkId=390109 を参照してください。

        public virtual DbSet<db_user_t> userEntities { get; set; }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}