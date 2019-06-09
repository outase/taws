using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tawsLibrary;
using tawsCommons.mvc;

namespace tawsLibrary
{
    public class TestCaseFactory
    {
        public string insertTestCase(ITestCaseModelBase testCase)
        {
            var dbIo = new DataBaseIo();

            //test_case_tのインサート文作成
            string insTestCase = "INSERT INTO test_case_t(test_case_no, name, test_url, description) VALUES ";

            int testCaseCount = dbIo.ExeSql("");

            insTestCase += $"('{ testCase.projectNo }', '{ testCase.testCaseName }', '{ testCase.testURL }', '{ testCase.testDescription }');";

            //test_case_detail_tのインサート文作成
            string insTestCaseDetail = "INSERT INTO test_case_detail_t(test_case_no, elem_no, elem_name, sendkey, sleep_time) VALUES ";

            foreach (Dictionary<string, string> tlist in testCase.testElemList)
            {
                insTestCaseDetail += $"('','{ tlist[FileIo.ELEM_NO] }', '{ tlist[FileIo.ELEM_NAME] }', '{ tlist[FileIo.SEND_KEY] }', '{ tlist[FileIo.SLEEP_TIME] }'),";
            }

            //最後の不要な","を削除してセミコロンで閉じる
            insTestCaseDetail = insTestCaseDetail.Substring(0, insTestCaseDetail.Length - 1) + ";";

            dbIo.ExeSql(insTestCase);
            dbIo.ExeSql(insTestCaseDetail);

            return "";
        }

        public int TestCaseCountWithTestCaseNo(string testCaseNo)
        {
            return 0;
        }
    }
}
