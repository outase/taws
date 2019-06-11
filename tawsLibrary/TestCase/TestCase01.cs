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

            testElemList.Add(new Dictionary<string, string>() { { "elem_no", "1" }, { "elem_name", "" }, { "send_key", "" }, { "sleep_time", "" } } );
            testElemList.Add(new Dictionary<string, string>() { { "elem_no", "205" }, { "elem_name", "その他の「ヘッドライン」" }, { "send_key", "" }, { "sleep_time", "" } });
            testElemList.Add(new Dictionary<string, string>() { { "elem_no", "1" }, { "elem_name", "" }, { "send_key", "" }, { "sleep_time", "2000" } });
            testElemList.Add(new Dictionary<string, string>() { { "elem_no", "401" }, { "elem_name", "select * from test_case_t" }, { "send_key", "test_case_t" }, { "sleep_time", "" } });

            return testElemList;
        }
    }
}
