using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Transactions;
using Npgsql;
using tawsCommons.mvc;

namespace tawsLibrary
{
    public class DataBaseIo
    {
        /// <summary>
        /// CSV出力用
        /// ※別サーバーにDBがあるときは、COPYコマンドでEXPORTできないので、ExportCsv2を利用のこと
        /// </summary>
        public void ExportCsv(string savePath, string fileName, string exportSql)
        {
            var csvPath = savePath + fileName + ".csv";
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            //出力前にスリープ
            Thread.Sleep(2000);

            using (TransactionScope ts = new TransactionScope())
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    NpgsqlCommand command = new NpgsqlCommand($@"COPY ({ exportSql }) TO '{ csvPath }' CSV HEADER", conn);
                    command.ExecuteNonQuery();
                    conn.Close();
                }

                ts.Complete();
            }
        }

        ///<summary>
        ///CSV出力用
        ///別サーバーにDBがあってもEXPORT可能
        ///※ただし、テーブル結合の場合のタイトル出力には未対応
        ///</summary>
        public void ExportCsv2(string savePath, string fileName, string exportSql, string exportTable = null)
        {
            var csvPath = savePath + fileName + ".csv";
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string csvTitle = null;
            string exportTitle = $@"SELECT column_name FROM information_schema.columns WHERE table_name = '{ exportTable }' ORDER BY ordinal_position;";

            //出力前にスリープ
            Thread.Sleep(2000);

            using (TransactionScope ts = new TransactionScope())
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();

                    //CSVタイトル出力
                    if (exportTable != null)
                    {
                        var cmd = new NpgsqlCommand(exportTitle, conn);
                        using (var da = new NpgsqlDataAdapter(cmd))
                        {
                            var dt = new DataTable();
                            da.Fill(dt);

                            int length = 0;
                            foreach (DataRow row in dt.Rows)
                            {
                                csvTitle += $@"{row["column_name"]},";
                            }
                            length = csvTitle.Length;
                            csvTitle = csvTitle.Substring(0, length - 1);
                        }
                    }

                    //CSVレコード出力
                    var cmd2 = new NpgsqlCommand(exportSql, conn);
                    using (var da = new NpgsqlDataAdapter(cmd2))
                    {
                        var dt = new DataTable();
                        da.Fill(dt);

                        using (var exportCsv = new StreamWriter(csvPath))
                        {
                            string csvRecode = "";
                            int length = 0;

                            if (csvTitle != null)
                            {
                                exportCsv.WriteLine(csvTitle);
                            }

                            foreach (DataRow row in dt.Rows)
                            {
                                foreach (var item in row.ItemArray)
                                {
                                    csvRecode += $@"{item},";
                                }

                                length = csvRecode.Length;
                                exportCsv.WriteLine(csvRecode.Substring(0, length - 1));
                                csvRecode = "";
                            }
                        }
                    }
                    conn.Close();
                }
                ts.Complete();
            }
        }

        /// <summary>
        /// 単一のクエリの実行
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public int ExeSql(string query)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            int result = 0;

            using (TransactionScope ts = new TransactionScope())
            {
                using (SqlConnection sqlCon = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    sqlCon.Open();

                    SqlCommand comm = new SqlCommand(query, sqlCon);
                    result = comm.ExecuteNonQuery();

                    sqlCon.Close();
                }

                ts.Complete();
            }

            return result;
        }

        /// <summary>
        /// 売上伝票ヘッダと明細のようにセットで同一トランザクション内で実行する場合に利用
        /// </summary>
        /// <param name="queryHeader"></param>
        /// <param name="queryDetail"></param>
        /// <returns></returns>
        public int[] ExeSql2(string queryHeader, string queryDetail)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            int[] result = new int[2];

            using (TransactionScope ts = new TransactionScope())
            {
                using (SqlConnection sqlCon = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    sqlCon.Open();

                    SqlCommand comm = new SqlCommand(queryHeader, sqlCon);
                    result[0] = comm.ExecuteNonQuery();

                    SqlCommand comm2 = new SqlCommand(queryDetail, sqlCon);
                    result[1] = comm2.ExecuteNonQuery();

                    sqlCon.Close();
                }

                //どちらかが更新が０件の場合は失敗とみなしてコンプリートさせない
                if (result[0] > 0 && result[0] > 0)
                {
                    ts.Complete();
                }
            }

            return result;
        }

        /// <summary>
        /// 単一のクエリの実行
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public int ExeSqlUseNpgsql(string query)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            int result = 0;

            using (TransactionScope ts = new TransactionScope())
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();

                    NpgsqlCommand comm = new NpgsqlCommand(query, conn);
                    result = comm.ExecuteNonQuery();

                    conn.Close();
                }

                ts.Complete();
            }

            return result;
        }

        /// <summary>
        /// 売上伝票ヘッダと明細のようにセットで同一トランザクション内で実行する場合に利用
        /// </summary>
        /// <param name="queryHeader"></param>
        /// <param name="queryDetail"></param>
        /// <returns></returns>
        public int[] ExeSql2UseNpgsql(string queryHeader, string queryDetail)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            int[] result = new int[2] {0, 0};

            using (TransactionScope ts = new TransactionScope())
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();

                    NpgsqlCommand comm = new NpgsqlCommand(queryHeader, conn);
                    result[0] = comm.ExecuteNonQuery();

                    NpgsqlCommand comm2 = new NpgsqlCommand(queryDetail, conn);
                    result[1] = comm2.ExecuteNonQuery();

                    conn.Close();
                }

                //どちらかが更新が０件の場合は失敗とみなしてコンプリートさせない
                if(result[0] > 0 && result[0] > 0)
                {
                    ts.Complete();
                }
            }

            return result;
        }


        ///<summary>
        ///複数のクエリを同一トランザクション内で処理したい場合に利用(用途に合わせてロールバックさせる処理の追加が必要）
        ///</summary>
        public virtual List<int> ExeSql3UseNpgsql(string[] query)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            var result = new List<int>();

            using (TransactionScope ts = new TransactionScope())
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();

                    foreach(string s in query)
                    {
                        NpgsqlCommand comm = new NpgsqlCommand(s, conn);
                        result.Add(comm.ExecuteNonQuery());
                    }

                    conn.Close();
                }

                ts.Complete();
            }

            return result;
        }
    }
}
