﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using taws.Models;
using tawsLibrary;

namespace taws.Controllers
{
    public class TestCaseController : Controller
    {
        // GET: TestCase
        public ActionResult index()
        {
            return View();
        }
        public ActionResult Insert()
        {
            return View();
        }

        public ActionResult Insert2(TestCaseProperty testCase)
        {
            var fileIo = new FileIo();

            //テストケース登録日時取得
            string testDateTime = DateTime.Now.ToString("yyyyMMdd_HHmmss");

            //テストケースファイル保存
            string uploadFileSavePath = fileIo.SaveTestCaseFiles(testCase.testCaseFile, testDateTime);

            //ファイルからテストケース取得
            testCase.testElemList = new FileIo().TestCaseFromUploadFile(uploadFileSavePath, testCase.testCaseFile);

            //DataBaseにInsertする
            var testCaseFac = new TestCaseFactory();
            testCase.resultMsg = testCaseFac.insertTestCase(testCase);

            return View("complete", testCase);
        }
    }
}