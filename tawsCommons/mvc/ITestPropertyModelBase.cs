using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace tawsCommons.mvc
{
    public interface ITestPropertyModelBase
    {
        string testURL { get; set; }
        string testCase { get; set; }
        string testTerms1 { get; set; }
        string testTerms2 { get; set; }
        string testTerms3 { get; set; }
        string testTerms4 { get; set; }
        string testTerms5 { get; set; }
        string testBrowser { get; set; }
        string testDevice { get; set; }
        string screenShotType { get; set; }
        string testUserAgent { get; set; }
        bool screenCloseFlg { get; set; }
        HttpPostedFileWrapper testCaseFile { get; set; }

        //テスト実施日時
        string testDateTime { get; set; }

        //エビデンス出力先パス
        string evidenceSavePath { get; set; }

        //アップロードファイル保存先パス
        string uploadFileSavePath { get; set; }
        bool fileUploadResult { get; set; }

        //スクリーンサイズ
        int screenWidth { get; set; }
        int screenHeight { get; set; }

        //ウィンドウ表示位置
        int positionX { get; set; }
        int positionY { get; set; }

        //テスト結果
        string resultMsg { get; set; }
        string resultErrorMsg { get; set; }
    }
}
