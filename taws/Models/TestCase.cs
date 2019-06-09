using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tawsCommons.mvc;

namespace taws.Models
{
    public class TestCase : ITestCaseModelBase
    {
        public string projectNo { get; set; }
        public string testCaseName { get; set; }
        public string testURL { get; set; }
        public HttpPostedFileWrapper testCaseFile { get; set; }
        public List<Dictionary<string, string>> testElemList { get; set; }
        public string testDescription { get; set; }

        //テスト結果
        public string resultMsg { get; set; }
        public string resultErrorMsg { get; set; }
    }
}