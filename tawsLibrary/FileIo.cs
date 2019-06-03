using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using tawsCommons.mvc;

namespace tawsLibrary
{
    public class FileIo
    {
        //エビデンス用のディレクトリ作成
        public string CreateEvidencesDirectory(string testDateTime, ITestPropertyModelBase prop)
        {
            var evidenceSaveDir = $@"{ConfigurationManager.AppSettings["SaveFileRootPath"]}{ prop.testBrowser }_{ prop.testDevice }_{ testDateTime }";
            var evidenceSavePath = evidenceSaveDir + @"\";

            if (!Directory.Exists(evidenceSaveDir))
            {
                Directory.CreateDirectory(evidenceSaveDir);
            }

            return evidenceSavePath;
        }

        //テストケース保存
        public bool SaveTestCaseFiles(ITestPropertyModelBase prop, string testDateTime, HttpPostedFileWrapper testCaseFile)
        {
            if (testCaseFile != null)
            {
                var uploadFileSaveDir = $@"{ConfigurationManager.AppSettings["UploadFileRootPath"]}\{ testDateTime }";
                var uploadFileSavePath = uploadFileSaveDir + @"\";

                //ディレクトリ作成
                if (!Directory.Exists(uploadFileSaveDir))
                {
                    Directory.CreateDirectory(uploadFileSaveDir);
                }

                //ファイル保存
                testCaseFile.SaveAs(uploadFileSavePath + Path.GetFileName(testCaseFile.FileName));

                return true;
            }

            return false;
        }
    }
}