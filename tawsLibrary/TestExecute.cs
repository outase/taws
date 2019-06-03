﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using Npgsql;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using tawsCommons.mvc;
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
            var testElemList = new TestCase01().TestElement(prop);

            //テスト要素の実行
            foreach (Dictionary<string,string> tlist in testElemList)
            {
                //スクリーンショットの取得、エビデンスのファイル名のNoをカウントアップ
                if ((int)EnumTestElem.getScreen == Convert.ToInt32(tlist["elemNo"]))
                {
                    this.TestElementExecution(driver, Convert.ToInt32(tlist["elemNo"]), prop, fileNameNo.ToString("00"));
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
                    this.TestElementExecution(driver, Convert.ToInt32(tlist["elemNo"]), prop, tlist["elemName"], tlist["sendKey"]);
                }
            }

            if (prop.screenCloseFlg)
            {
                //ブラウザを閉じる
                driver.Close();
            }
        }

        public bool TestElementExecution<T>(T driver, int elemNo, ITestPropertyModelBase prop, string elemName, string sendKey = "") where T : RemoteWebDriver
        {
            bool result = false;

            //実行前に少し待つ
            Thread.Sleep(500);

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
                    //driver.FindElement(By.Id(elemName)).SendKeys(sendKey);
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
                    //driver.FindElement(By.Name(elemName)).SendKeys(sendKey);
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
                    //driver.FindElement(By.Id(elemName)).Click();
                    driver.FindElementById(elemName).Click();
                    result = true;
                }
            }
            else if (elemNo == (int)EnumTestElem.Name_Click)
            {
                if (driver.FindElements(By.Name(elemName)).Count > 0)
                {
                    //driver.FindElement(By.Name(elemName)).Click();
                    driver.FindElementByName(elemName).Click();
                    result = true;
                }
            }
            else if (elemNo == (int)EnumTestElem.ClassName_Click)
            {
                if (driver.FindElements(By.ClassName(elemName)).Count > 0)
                {
                    //driver.FindElement(By.ClassName(elemName)).Click();
                    driver.FindElementByClassName(elemName).Click();
                    result = true;
                }
            }
            else if (elemNo == (int)EnumTestElem.XPath_Click)
            {
                if (driver.FindElements(By.XPath(elemName)).Count > 0)
                {
                    //driver.FindElement(By.XPath(elemName)).Click();
                    driver.FindElementByXPath(elemName).Click();
                    result = true;
                }
            }
            else if (elemNo == (int)EnumTestElem.LinkText_Click)
            {
                if (driver.FindElements(By.LinkText(elemName)).Count > 0)
                {
                    //driver.FindElement(By.LinkText(elemName)).Click();
                    driver.FindElementByLinkText(elemName).Click();
                    result = true;
                }
            }
            //javascript実行
            else if (elemNo == (int)EnumTestElem.Execute_Script)
            {
                try
                {
                    driver.ExecuteScript(elemName);
                    result = true;
                }
                catch (Exception e) { prop.resultErrorMsg = e.ToString(); }
            }

            return result;
        }
    }
}