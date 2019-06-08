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
            if (prop.testCaseFile != null)
            {
                var uploadFileSaveDir = $@"{ConfigurationManager.AppSettings["UploadFileRootPath"]}\{ prop.testDateTime }";
                var uploadFileSavePath = uploadFileSaveDir + @"\";

                //ディレクトリ作成
                if (!Directory.Exists(uploadFileSaveDir))
                {
                    Directory.CreateDirectory(uploadFileSaveDir);
                }

                //ファイル保存
                prop.testCaseFile.SaveAs(uploadFileSavePath + Path.GetFileName(prop.testCaseFile.FileName));

                return uploadFileSavePath;
            }

            return null;
        }
    }
}