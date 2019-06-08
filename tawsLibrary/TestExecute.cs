﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;
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

            //画面サイズ設定
            var so = new ScreenOprations();
            so.ReSize(driver, prop.screenWidth, prop.screenHeight);

            //テストケースの取得
            //カスタムテストケースから
            List<Dictionary<string, string>> testElemList = null;
            if (prop.testCase == "000")
            {
                testElemList = new TestCase01().TestElement(prop);
            }
            //CSVファイルから
            else if (prop.testCase == "901")
            {
                testElemList = this.TestCaseFromUploadFile(prop);
                //uploadしたファイルをフォルダごと削除
                Directory.Delete(prop.uploadFileSavePath.Substring(0, prop.uploadFileSavePath.Length - 1), true); 
            }
            // データベースから
            else if (prop.testCase == "902")
            {

            }

            //テストケースの実行
            this.ExeTestCase<T>(driver, prop, testElemList);

            if (prop.screenCloseFlg)
            {
                //ブラウザを閉じる
                driver.Close();
            }
        }

        public const string ELEM_NO = "elem_no";
        public const string ELEM_NAME = "elem_name";
        public const string SEND_KEY = "send_key";
        public const string SLEEP_TIME = "sleep_time";

        public List<Dictionary<string, string>> TestCaseFromUploadFile(ITestPropertyModelBase prop)
        {
            var testElemList = new List<Dictionary<string, string>>();

            var csvFilePath = prop.uploadFileSavePath + prop.testCaseFile.FileName;
            StreamReader reader = new StreamReader(csvFilePath, Encoding.GetEncoding("Shift_JIS"));

            reader.ReadLine(); //タイトルを読み飛ばす
            while (reader.Peek() >= 0)
            {
                string[] cols = reader.ReadLine().Split(',');

                // 項目分繰り返す
                for (int i = 0; i < cols.Length; ++i)
                {
                    //先頭のスペースを除去して、(")ダブルクォーテーションが入っていないか判定する
                    if (cols[i] != string.Empty && cols[i].TrimStart()[0] == '"')
                    {
                        cols[i] = cols[i].Replace("\"","").Trim();
                    }
                    //先頭のスペースを除去して、(')クォーテーションが入っていないか判定する
                    else if (cols[i] != string.Empty && cols[i].TrimStart()[0] == '\'')
                    {
                        cols[i] = cols[i].Replace("'", "").Trim();
                    }
                }

                //テストケースを格納する
                testElemList.Add(new Dictionary<string, string>() { { ELEM_NO, $"{ cols[0] }" }, { ELEM_NAME, $"{ cols[1] }" }, { SEND_KEY, $"{ cols[2] }" }, { SLEEP_TIME, $"{ cols[3] }" } });
            }
            reader.Close();

            return testElemList;
        }

        public void ExeTestCase<T>(T driver, ITestPropertyModelBase prop, List<Dictionary<string, string>> testElemList) where T : RemoteWebDriver
        {
            var dbIo = new DataBaseIo();
            int fileNameNo = 0; //ファイル名No
            bool result = true; //テスト結果格納

            //テスト要素の実行
            foreach (Dictionary<string, string> tlist in testElemList)
            {
                //スクリーンショットの取得、エビデンスのファイル名のNoをカウントアップ
                if ((int)EnumTestElem.getScreen == Convert.ToInt32(tlist[ELEM_NO]))
                {
                    result = this.TestElementExecution(driver, Convert.ToInt32(tlist[ELEM_NO]), prop, fileNameNo.ToString("00"), "", tlist[SLEEP_TIME]);
                    fileNameNo++;
                }
                //csvの取得
                else if ((int)EnumTestElem.Export_Csv == Convert.ToInt32(tlist[ELEM_NO]))
                {
                    dbIo.ExportCsv2(prop.evidenceSavePath, fileNameNo.ToString("00"), tlist[ELEM_NAME], tlist[SEND_KEY]);
                }
                //その他画面要素の操作
                else
                {
                    result = this.TestElementExecution(driver, Convert.ToInt32(tlist[ELEM_NO]), prop, tlist[ELEM_NAME], tlist[SEND_KEY], tlist[SLEEP_TIME]);
                }

                if (!result) break;
            }
        }

        public bool TestElementExecution<T>(T driver, int elemNo, ITestPropertyModelBase prop, string elemName, string sendKey = "", string sleep = "") where T : RemoteWebDriver
        {
            bool result = false;

            //実行前に少し待つ
            if(sleep == "")
            {
                Thread.Sleep(500);
            }
            else
            {
                Thread.Sleep(Convert.ToInt32(sleep));
            }

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