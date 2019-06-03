using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Windows;
using System.Threading;
using System.Transactions;
using System.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using Npgsql;
using tawsCommons.mvc;

namespace tawsLibrary
{
    public class ScreenOprations
    {
        public void ReSize(IWebDriver driver, int reWidth, int reHeight)
        {
            var size = driver.Manage().Window.Size;
            size.Width = reWidth;
            size.Height = reHeight;
            driver.Manage().Window.Size = size;
            Thread.Sleep(2000);
        }

        public void setPostion(IWebDriver driver, int x, int y)
        {
            var potion = driver.Manage().Window.Position;
            potion.X = x;
            potion.Y = y;
            driver.Manage().Window.Position = potion;
            Thread.Sleep(2000);
        }

        public void GetScreenCommon<T>(T driver, string savePath, string fileName, string browserName, string mode = null) where T :RemoteWebDriver
        {
            Thread.Sleep(2000);

            if (mode == "full")
            {
                try
                {
                    this.GetFullScreen(driver, savePath, fileName + ".png");
                }
                //ブラウザによってJavascriptが動作しなかったときの対応
                catch
                {
                    driver.GetScreenshot().SaveAsFile(savePath + fileName + ".png");

                }
            }
            else
            {
                driver.GetScreenshot().SaveAsFile(savePath + fileName + ".png");
            }
        }

        public virtual void GetFullScreen<T>(T driver, string savePath, string fileName) where T : RemoteWebDriver
        {
            // ページサイズ取得
            var totalWidth = Convert.ToInt32(driver.ExecuteScript("return document.body.scrollWidth"));
            var totalHeight = Convert.ToInt32(driver.ExecuteScript("return document.body.scrollHeight"));

            // 画面サイズ取得
            var viewWidth = Convert.ToInt32(driver.ExecuteScript("return document.documentElement.clientWidth"));
            var viewHeight = Convert.ToInt32(driver.ExecuteScript("return document.documentElement.clientHeight"));

            // ブラウザのヘッダのHeight取得
            var bodyY = Convert.ToInt32(driver.ExecuteScript("return window.outerHeight")) - Convert.ToInt32(driver.ExecuteScript("return window.innerHeight"));

            // スクロール操作用
            var scrollWidth = 0;
            var scrollHeight = 0;

            //Bitmapの作成
            Bitmap tmpDmp = new Bitmap(totalWidth, totalHeight);
            var screenSplitCount = Math.Ceiling((double)totalHeight / viewHeight);

            // 縦スクロールの処理
            for (int count = 1; count <= screenSplitCount; count++)
            {
                //Graphicsの作成
                Graphics tmpGrp = Graphics.FromImage(tmpDmp);

                //描写前にSleep
                Thread.Sleep(750);

                //画面全体をコピーする
                if (count == screenSplitCount)
                {
                    var extraHeight = viewHeight * count - totalHeight;
                    tmpGrp.CopyFromScreen(new Point(0, bodyY + extraHeight), new Point(0, scrollHeight), tmpDmp.Size);
                }
                else
                {
                    tmpGrp.CopyFromScreen(new Point(0, bodyY), new Point(0, scrollHeight), tmpDmp.Size);
                }

                //imageファイル出力
                tmpDmp.Save(savePath + fileName, System.Drawing.Imaging.ImageFormat.Png);

                tmpGrp.Dispose();

                scrollHeight += viewHeight;
                driver.ExecuteScript($"window.scrollTo({scrollWidth}, {scrollHeight})");
            }

            //画面をもとの位置に戻す
            driver.ExecuteScript($"window.scrollTo(0, 0)");
        }
    }
}