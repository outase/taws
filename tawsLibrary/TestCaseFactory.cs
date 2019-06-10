using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
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

            //すでに登録済みのprojectNoのテストケース件数を取得
            int projectCount = dbIo.ExeSql($"SELECT COUNT(*) FROM test_case_t WHERE test_case_no like '{ testCase.projectNo }%';");

            //新しい連番を取得
            string newSerial = (projectCount + 1).ToString($"D{ ConfigurationManager.AppSettings["ProjectNoCountDigits"] }");
            string newTestCaseNo = $"{ testCase.projectNo }-{ newSerial }";

            //インサートするレコード作成
            insTestCase += $"('{ newTestCaseNo }', '{ testCase.testCaseName }', '{ testCase.testURL }', '{ testCase.testDescription }');";

            //test_case_detail_tのインサート文作成
            string insTestCaseDetail = "INSERT INTO test_case_detail_t(test_case_no, elem_no, elem_name, sendkey, sleep_time) VALUES ";

            //インサートするレコード作成
            foreach (Dictionary<string, string> tlist in testCase.testElemList)
            {
                insTestCaseDetail += $"('{ newTestCaseNo }','{ tlist[FileIo.ELEM_NO] }', '{ tlist[FileIo.ELEM_NAME] }', '{ tlist[FileIo.SEND_KEY] }', '{ tlist[FileIo.SLEEP_TIME] }'),";
            }

            //最後の不要な","を削除してセミコロンで閉じる
            insTestCaseDetail = insTestCaseDetail.Substring(0, insTestCaseDetail.Length - 1) + ";";

            //データベースにINSERTする
            int[] result = dbIo.ExeSql2(insTestCase, insTestCaseDetail);

            if (result[0] == 0 || result[1] == 0)
            {
                return $"テストケースのINSERTに失敗しました。test_case_t:{ result[0] } test_case_detail_t：{ result[1] }";
            }

            return $"テストケースのINSERTが完了しました。test_case_t:{ result[0] } test_case_detail_t：{ result[1] }";
        }
    }
}
