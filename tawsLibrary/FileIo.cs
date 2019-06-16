using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using tawsCommons.mvc;

namespace tawsLibrary
{
    public class FileIo
    {
        //エビデンス用のディレクトリ作成
        public string CreateEvidencesDirectory(ITestPropertyModelBase prop)
        {
            var evidenceSaveDir = $@"{ConfigurationManager.AppSettings["SaveFileRootPath"]}{ prop.testBrowser }_{ prop.testDevice }_{ prop.testDateTime }";
            var evidenceSavePath = evidenceSaveDir + @"\";

            if (!Directory.Exists(evidenceSaveDir))
            {
                Directory.CreateDirectory(evidenceSaveDir);
            }

            return evidenceSavePath;
        }

        //テストケース保存
        public string SaveTestCaseFiles(ITestPropertyModelBase prop)
        {
            if (testCaseFile != null)
            {
                var uploadFileSaveDir = $@"{ConfigurationManager.AppSettings["UploadFileRootPath"]}\{ prop.testDateTime }";
                var uploadFileSavePath = uploadFileSaveDir + @"\";

                //ディレクトリ作成
                if (!Directory.Exists(uploadFileSaveDir))
                {
                    Directory.CreateDirectory(uploadFileSaveDir);
                }

                //ファイル保存
                testCaseFile.SaveAs(uploadFileSavePath + Path.GetFileName(testCaseFile.FileName));

                return uploadFileSavePath;
            }

            return null;
        }

        public const string ELEM_NO = "elem_no";
        public const string ELEM_NAME = "elem_name";
        public const string SEND_KEY = "send_key";
        public const string SLEEP_TIME = "sleep_time";

        public List<Dictionary<string, string>> TestCaseFromUploadFile(string uploadFileSavePath, HttpPostedFileWrapper testCaseFile)
        {
            var testElemList = new List<Dictionary<string, string>>();

            var csvFilePath = uploadFileSavePath + testCaseFile.FileName;
            StreamReader reader = new StreamReader(csvFilePath, Encoding.GetEncoding("Shift_JIS"));

            reader.ReadLine(); //タイトルを読み飛ばす
            while (reader.Peek() >= 0)
            {
                string[] cols = reader.ReadLine().Split(',');

                // 項目分繰り返す
                for (int i = 0; i < cols.Length; ++i)
                {
                    //先頭のスペースを除去して、(")ダブルクォーテーションが入っていないか判定する
                    if (cols[i] != string.Empty && cols[i].TrimStart()[0] == '"')
                    {
                        cols[i] = cols[i].Replace("\"", "").Trim();
                    }
                    //先頭のスペースを除去して、(')クォーテーションが入っていないか判定する
                    else if (cols[i] != string.Empty && cols[i].TrimStart()[0] == '\'')
                    {
                        cols[i] = cols[i].Replace("'", "").Trim();
                    }
                }

                //テストケースを格納する
                testElemList.Add(new Dictionary<string, string>() { { ELEM_NO, $"{ cols[0] }" }, { ELEM_NAME, $"{ cols[1] }" }, { SEND_KEY, $"{ cols[2] }" }, { SLEEP_TIME, $"{ cols[3] }" } });
            }
            reader.Close();

            //uploadしたファイルをフォルダごと削除
            Directory.Delete(uploadFileSavePath.Substring(0, uploadFileSavePath.Length - 1), true);

            return testElemList;
        }
    }
}