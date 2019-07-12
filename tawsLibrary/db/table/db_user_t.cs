using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tawsLibrary.db.table
{
    [Table("user_t")] //テーブル名
    public class db_user_t : db_base_t
    {
        [Key] //主キー設定
        [Column("id")] //DB上のカラム名
        public int id { get; set; }

        [Column("user_name")]
        public string user_name { get; set; }

        [Column("login_id")]
        public string login_id { get; set; }

        [Column("password")]
        public string password { get; set; }

        [Column("email")]
        public string email { get; set; }
    }
}
