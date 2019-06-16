using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using taws.Models;

namespace taws.Validate
{
    public class tawsValidate
    {
        public void Validate(TestProperty prop)
        {
            //Validation
            if (prop.testCase == "801")
            {
                if (prop.testURL == null)
                {
                    prop.validationMsg.Add("テストURLが未入力です。");
                    
                }
            }
            else if (prop.testCase == "901")
            {
                if (prop.testURL == null)
                {
                    prop.validationMsg.Add("テストURLが未入力です。");
                }

                if (prop.testCaseFile == null)
                {
                    prop.validationMsg.Add("テストケースファイルが未選択です。");
                }
            }
        }
    }
}