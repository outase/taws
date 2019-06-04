using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Text;
using Npgsql;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using tawsCommons.mvc;
using tawsLibrary;
using tawsLibrary.TestCase;

namespace tawsLibrary
{
    public class TestExecute
    {
        public virtual void ExeTest<T>(T driver, ITestPropertyModelBase prop) where T : RemoteWebDriver
        {
            //ブラウザを開く
            driver.Navigate().GoToUrl(prop.testURL);

            var dbIo = new DataBaseIo();
            int fileNameNo = 0; //ファイル名No

            //テストケースの呼び出し
            List<Dictionary<string, string>> testElemList = null;
            if (prop.testCase == "000")
            {
                testElemList = new TestCase01().TestElement(prop);
            }
            //CSVファイルから
            else if (prop.testCase == "999")
            {
                testElemList = this.TestCaseFromUploadFile(prop);
                //uploadしたファイルをフォルダごと削除
                Directory.Delete(prop.uploadFileSavePath.Substring(0, prop.uploadFileSavePath.Length - 1), true); 
            }

            //テスト結果格納
            bool result = true;

            //テスト要素の実行
            foreach (Dictionary<string,string> tlist in testElemList)
            {
                //スクリーンショットの取得、エビデンスのファイル名のNoをカウントアップ
                if ((int)EnumTestElem.getScreen == Convert.ToInt32(tlist["elemNo"]))
                {
                    result = this.TestElementExecution(driver, Convert.ToInt32(tlist["elemNo"]), prop, fileNameNo.ToString("00"));
                    fileNameNo++;
                }
                //csvの取得
                else if ((int)EnumTestElem.Export_Csv == Convert.ToInt32(tlist["elemNo"]))
                {
                    dbIo.ExportCsv2(prop.evidenceSavePath, fileNameNo.ToString("00"), tlist["elemName"], tlist["sendKey"]);
                }
                //その他画面要素の操作
                else
                {
                    result = this.TestElementExecution(driver, Convert.ToInt32(tlist["elemNo"]), prop, tlist["elemName"], tlist["sendKey"]);
                }

                if (!result) break;
            }

            if (prop.screenCloseFlg)
            {
                //ブラウザを閉じる
                driver.Close();
            }
        }

        public List<Dictionary<string, string>> TestCaseFromUploadFile(ITestPropertyModelBase prop)
        {
            var testElemList = new List<Dictionary<string, string>>();

            var csvFilePath = prop.uploadFileSavePath + prop.testCaseFile.FileName;
            StreamReader reader = new StreamReader(csvFilePath, Encoding.GetEncoding("Shift_JIS"));
            reader.ReadLine(); //タイトルを読み飛ばす
            while (reader.Peek() >= 0)
            {
                string[] cols = reader.ReadLine().Split(',');

                // 配列からリストに格納する
                List<string> lists = new List<string>();
                lists.AddRange(cols);

                // 項目分繰り返す
                for (int i = 0; i < lists.Count; ++i)
                {
                    //先頭のスペースを除去して、(")ダブルクォーテーションが入っていないか判定する
                    if (lists[i] != string.Empty && lists[i].TrimStart()[0] == '"')
                    {
                        lists[i] = lists[i].Replace("\"", "");
                    }
                    //先頭のスペースを除去して、(')クォーテーションが入っていないか判定する
                    else if (lists[i] != string.Empty && lists[i].TrimStart()[0] == '\'')
                    {
                        lists[i] = lists[i].Replace("\'", "");
                    }
                }

                //テストケースを格納する
                testElemList.Add(new Dictionary<string, string>() { { "elemNo", $"{ lists[0] }" }, { "elemName", $"{ lists[1] }" }, { "sendKey", $"{ lists[2] }" } });
            }
            reader.Close();

            return testElemList;
        }

        public bool TestElementExecution<T>(T driver, int elemNo, ITestPropertyModelBase prop, string elemName, string sendKey = "") where T : RemoteWebDriver
        {
            bool result = false;

            //実行前に少し待つ
            Thread.Sleep(500);

            try
            {
                //スクリーンショット取得
                if (elemNo == (int)EnumTestElem.getScreen)
                {
                    var scrOpt = new ScreenOprations();
                    scrOpt.GetScreenCommon<T>(driver, prop.evidenceSavePath, elemName, prop.testBrowser, prop.screenShotType);
                    result = true;
                }
                //sendkey id 主にテキストボックスなど
                else if (elemNo == (int)EnumTestElem.Id_SendKey)
                {
                    if (driver.FindElements(By.Id(elemName)).Count > 0)
                    {
                        driver.FindElementById(elemName).SendKeys(sendKey);
                        result = true;
                    }
                }
                //sendkey id 主にセレクトボックスなど
                else if (elemNo == (int)EnumTestElem.Id_SendKeyForSelectEtc)
                {
                    if (driver.FindElements(By.Id(elemName)).Count > 0)
                    {
                        driver.FindElement(By.Id(elemName)).SendKeys(sendKey);
                        result = true;
                    }
                }
                //sendkey Name 主にテキストボックスなど
                else if (elemNo == (int)EnumTestElem.Name_SendKey)
                {
                    if (driver.FindElements(By.Name(elemName)).Count > 0)
                    {
                        driver.FindElementByName(elemName).SendKeys(sendKey);
                        result = true;
                    }
                }
                //sendkey Name 主にセレクトボックスなど
                else if (elemNo == (int)EnumTestElem.Name_SendKeyForSelectEtc)
                {
                    if (driver.FindElements(By.Name(elemName)).Count > 0)
                    {
                        driver.FindElement(By.Name(elemName)).SendKeys(sendKey);
                        result = true;
                    }
                }
                //クリック
                else if (elemNo == (int)EnumTestElem.Id_Click)
                {
                    if (driver.FindElements(By.Id(elemName)).Count > 0)
                    {
                        driver.FindElementById(elemName).Click();
                        result = true;
                    }
                }
                else if (elemNo == (int)EnumTestElem.Name_Click)
                {
                    if (driver.FindElements(By.Name(elemName)).Count > 0)
                    {
                        driver.FindElementByName(elemName).Click();
                        result = true;
                    }
                }
                else if (elemNo == (int)EnumTestElem.ClassName_Click)
                {
                    if (driver.FindElements(By.ClassName(elemName)).Count > 0)
                    {
                        driver.FindElementByClassName(elemName).Click();
                        result = true;
                    }
                }
                else if (elemNo == (int)EnumTestElem.XPath_Click)
                {
                    if (driver.FindElements(By.XPath(elemName)).Count > 0)
                    {
                        driver.FindElementByXPath(elemName).Click();
                        result = true;
                    }
                }
                else if (elemNo == (int)EnumTestElem.LinkText_Click)
                {
                    if (driver.FindElements(By.LinkText(elemName)).Count > 0)
                    {
                        driver.FindElementByLinkText(elemName).Click();
                        result = true;
                    }
                }
                //javascript実行
                else if (elemNo == (int)EnumTestElem.Execute_Script)
                {
                    driver.ExecuteScript(elemName);
                    result = true;
                }
            }
            catch (Exception e) { prop.resultErrorMsg = e.ToString(); }

            return result;
        }
    }
}