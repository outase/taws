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

            //Create ヘッダ

            //すでに登録済みのprojectNoのテストケース件数を取得
            int projectCount = dbIo.ExeRecodeCount($"SELECT COUNT(*) FROM test_case_t WHERE test_case_no like '{ testCase.projectNo }%';");

            //新しい連番を取得
            string newSerial = (projectCount + 1).ToString($"D{ ConfigurationManager.AppSettings["ProjectNoCountDigits"] }");
            string newTestCaseNo = $"{ testCase.projectNo }-{ newSerial }";

            //インサート文作成
            string insTestCase = "INSERT INTO test_case_t(test_case_no, name, test_url, description) VALUES ";

            //インサートするレコード作成
            insTestCase += $"('{ newTestCaseNo }', '{ testCase.testCaseName }', '{ testCase.testURL }', '{ testCase.testDescription }');";

            //Create ボディー

            //インサート文作成
            string insTestCaseDetail = "INSERT INTO test_case_detail_t(test_case_no, execute_order, elem_no, elem_name, sendkey, sleep_time) VALUES ";

            //インサートするレコード作成
            int index = 1;
            foreach (Dictionary<string, string> tlist in testCase.testElemList)
            {
                insTestCaseDetail += $@"('{ newTestCaseNo }', 
                                        '{ index.ToString("00000") }', 
                                        '{ tlist[FileIo.ELEM_NO] }', 
                                        '{ tlist[FileIo.ELEM_NAME] }', 
                                        '{ tlist[FileIo.SEND_KEY] }', 
                                        '{ tlist[FileIo.SLEEP_TIME] }'),";
                index++;
            }

            //最後の不要な","を削除してセミコロンで閉じる
            insTestCaseDetail = insTestCaseDetail.Substring(0, insTestCaseDetail.Length - 1) + ";";

            //データベースにINSERTする
            int[] result = dbIo.ExeDml2(insTestCase, insTestCaseDetail);

            if (result[0] == 0 || result[1] == 0)
            {
                return $"テストケースのINSERTに失敗しました。Test case table Add：{ result[0] }件 Test case detail table Add：{ result[1] }件";
            }

            return $"テストケースのINSERTが完了しました。Test case table Add：{ result[0] }件 Test case detail table Add：{ result[1] }件";
        }
    }
}
