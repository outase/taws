using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tawsLibrary
{
    public enum EnumTestElem
    {
        //スクリーンショット取得
        getScreen = 1,

        //エレメントにsendkeyする（id指定）主にテキストボックスなど
        Id_SendKey = 101,

        //エレメントにsendkeyする（id指定）主にセレクトボックスなど
        Id_SendKeyForSelectEtc = 102,

        //エレメントにsendkeyする（Name指定）主にテキストボックスなど
        Name_SendKey = 103,

        //エレメントにsendkeyする（Name指定）主にセレクトボックスなど
        Name_SendKeyForSelectEtc = 104,

        //エレメントをクリックする（id指定）
        Id_Click = 201,

        //エレメントをクリックする（Name指定）
        Name_Click = 202,

        //エレメントをクリックする（ClassName指定）
        ClassName_Click = 203,

        //エレメントをクリックする（XPath指定）
        XPath_Click = 204,

        //エレメントをクリックする（LinkText指定）
        LinkText_Click = 205,

        //JavaScript実行
        Execute_Script = 301,

        //JavaScript実行
        Export_Csv = 401,
    }
}