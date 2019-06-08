using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tawsCommons.mvc;

namespace taws.Models
{
    public class TestProperty : ITestPropertyModelBase
    {
        public string testURL { get; set; }
        public string testCase { get; set; }
        public string testTerms1 { get; set; }
        public string testTerms2 { get; set; }
        public string testTerms3 { get; set; }
        public string testTerms4 { get; set; }
        public string testTerms5 { get; set; }
        public string testBrowser { get; set; }
        public string testDevice { get; set; }
        public string screenShotType { get; set; }
        public string testUserAgent { get; set; }
        public bool screenCloseFlg { get; set; }
        public HttpPostedFileWrapper testCaseFile { get; set; }

        //テスト実施日時
        public string testDateTime { get; set; }

        //エビデンス出力先パス
        public string evidenceSavePath { get; set; }

        //アップロードファイル保存先パス
        public string uploadFileSavePath { get; set; }
        public bool fileUploadResult { get; set; }

        //スクリーンサイズ
        public int screenWidth { get; set; }
        public int screenHeight { get; set; }

        //ウィンドウ表示位置
        public int positionX { get; set; }
        public int positionY { get; set; }

        //テスト結果
        public string resultMsg { get; set; }
        public string resultErrorMsg { get; set; }
    }
}