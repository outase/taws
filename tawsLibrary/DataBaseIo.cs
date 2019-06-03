using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Transactions;
using Npgsql;

namespace tawsLibrary
{
    class DataBaseIo
    {
        //別サーバーにDBがあるときは、COPYコマンドでEXPORTできないので、localhostのみ利用可能
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

        //別サーバーにDBがあってもEXPORT可能
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

        public string ExecutSQL(string savePath, string fileName, string executSql)
        {

            var csvPath = savePath + fileName + ".csv";

            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            int result = 0;

            using (TransactionScope ts = new TransactionScope())
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    NpgsqlCommand command = new NpgsqlCommand(executSql, conn);
                    //SQLの実行と実行結果の格納
                    result = command.ExecuteNonQuery();
                    conn.Close();
                }

                ts.Complete();
            }

            return Convert.ToString(result) + "件";
        }
    }
}
