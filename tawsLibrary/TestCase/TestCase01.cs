using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using tawsLibrary.TestCase;
using tawsCommons.mvc;

namespace tawsLibrary.TestCase
{
    class TestCase01 : ITestCase
    {
        public virtual List<Dictionary<string, string>> TestElement(ITestPropertyModelBase prop)
        {
            List<Dictionary<string, string>> testElemList = new List<Dictionary<string, string>>();

            testElemList.Add(new Dictionary<string, string>() { { "elemNo", "1" }, { "elemName", "" }, { "sendKey", "" } } );
            testElemList.Add(new Dictionary<string, string>() { { "elemNo", "205" }, { "elemName", "その他の「ヘッドライン」" }, { "sendKey", "" } });
            testElemList.Add(new Dictionary<string, string>() { { "elemNo", "1" }, { "elemName", "" }, { "sendKey", "" } });
            //testElemList.Add(new Dictionary<string, string>() { { "elemNo", "401" }, { "elemName", "select * from user_t" }, { "sendKey", "user_t" } });

            return testElemList;
        }
    }
}
