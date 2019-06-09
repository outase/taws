using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace tawsCommons.mvc
{
    public interface ITestCaseModelBase
    {
        string projectNo { get; set; }
        string testCaseName { get; set; }
        string testURL { get; set; }
        HttpPostedFileWrapper testCaseFile { get; set; }
        List<Dictionary<string, string>> testElemList { get; set; }
        string testDescription { get; set; }

        //テスト結果
        string resultMsg { get; set; }
        string resultErrorMsg { get; set; }
    }
}
