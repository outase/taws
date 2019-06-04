using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows;
using System.Threading;
using System.Transactions;
using System.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.IE;
using Npgsql;
using taws.Models;
using tawsCommons;
using tawsLibrary;

namespace taws.Controllers
{
    public class tawsController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TestAction(TestProperty prop)
        {
           //var browerName = ConfigurationManager.AppSettings["BrowserName"];
            var browerName = prop.testBrowser;

            //テスト実施日時取得
            string testDateTime = DateTime.Now.ToString("yyyyMMdd_HHmmss");

            //エビデンス用ディレクトリ作成
            var fileIo = new FileIo();
            prop.evidenceSavePath = fileIo.CreateEvidencesDirectory(testDateTime, prop);
            //テストケースファイル保存
            prop.uploadFileSavePath = fileIo.SaveTestCaseFiles(prop, testDateTime);

            //URL設定(configから)
            //var prop.testURL = $"{ConfigurationManager.AppSettings["Url"]}member";

            //ウィンドウ表示位置設定
            prop.positionX = 0;
            prop.positionY = 0;

            //スクリーンサイズ取得
            if (prop.testDevice == "pc")
            {
                prop.screenWidth = Convert.ToInt32(ConfigurationManager.AppSettings["BrowerWidth"]);
                prop.screenHeight = Convert.ToInt32(ConfigurationManager.AppSettings["BrowerHeight"]);
            }
            else if (prop.testDevice == "sp_iphone")
            {
                prop.screenWidth = Convert.ToInt32(ConfigurationManager.AppSettings["BrowerWidthiPhone"]);
                prop.screenHeight = Convert.ToInt32(ConfigurationManager.AppSettings["BrowerHeightiPhone"]);
            }
            else if (prop.testDevice == "sp_android")
            {
                prop.screenWidth = Convert.ToInt32(ConfigurationManager.AppSettings["BrowerWidthAndroid"]);
                prop.screenHeight = Convert.ToInt32(ConfigurationManager.AppSettings["BrowerHeightAndroid"]);
            }


            //テスト実施---------------------------------------

            var excuteTest = new TestExecute();

            //Google Chromeのテスト
            if (browerName == "chrome")
            {
                //Web Driverの設定
                var option = new ChromeOptions();
                if (prop.testDevice == "sp_iphone")
                {
                    option.AddArgument($"--user-agent=\"{ ConfigurationManager.AppSettings["UserAgentiPhone"] }\"");
                }
                else if (prop.testDevice == "sp_android")
                {
                    option.AddArgument($"--user-agent=\"{ ConfigurationManager.AppSettings["UserAgentAndroid"] }\"");
                }

                excuteTest.ExeTest<ChromeDriver>(new ChromeDriver(option), prop);
            }
            //FireFoxのテスト
            else if (browerName == "firefox")
            {
                //Web Driverの設定
                var option = new FirefoxOptions();
                if (prop.testDevice == "sp_iphone")
                {
                    option.SetPreference("general.useragent.override", ConfigurationManager.AppSettings["UserAgentiPhone"]);
                }
                else if (prop.testDevice == "sp_android")
                {
                    option.SetPreference("general.useragent.override", ConfigurationManager.AppSettings["UserAgentAndroid"]);
                }
                //Proxyの設定
                if (Convert.ToBoolean(ConfigurationManager.AppSettings["SetProxy"]))
                {
                    var fireComm = new FireFoxTestCommons();
                    option = fireComm.SetProxy(option, ConfigurationManager.AppSettings["NonProxyString"]);
                }

                excuteTest.ExeTest<FirefoxDriver>(new FirefoxDriver(option), prop);
            }
            //Edgeのテスト
            else if (browerName == "edge")
            {
                excuteTest.ExeTest<EdgeDriver>(new EdgeDriver(), prop);
            }

            prop.resultMsg = $@"{ prop.evidenceSavePath } にテスト結果が保存されました。";

            return View("complete", prop);
        }

        public ActionResult complete()
        {
            return View("complete");
        }
    }
}