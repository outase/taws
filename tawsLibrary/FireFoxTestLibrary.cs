using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;


namespace tawsCommons
{
    public class FireFoxTestCommons
    {
        //FireFox SSL警告対応(こちらの理由で導入保留：profileがアクセス拒否で読み込み不可、自己証明書の受け入れ設定がバグで動作せず）
        public virtual FirefoxDriver SetAcceptUntrustedCertificates(FirefoxOptions option, string mode = null)
        {
            var profileDir = ConfigurationManager.AppSettings["ProfileDir"];
            FirefoxDriver driver = null;

            using (var fs = new FileStream(profileDir, FileMode.Open, FileAccess.Read))
            {
                var profile = new FirefoxProfile(profileDir);
                profile.AcceptUntrustedCertificates = true;
                profile.AssumeUntrustedCertificateIssuer = false;
                option.Profile = profile;

                option = this.SetProxy(option, ConfigurationManager.AppSettings["LineNonProxy"]);
                if (mode == "sp")
                {
                    option.SetPreference("general.useragent.override", ConfigurationManager.AppSettings["UserAgent"]);
                }

                driver = new FirefoxDriver(option);
            }

            return driver;
        }

        //proxyの値をセット
        public virtual FirefoxOptions SetProxy(FirefoxOptions option, string noProxy)
        {
            string proxy = ConfigurationManager.AppSettings["Proxy"];
            int port = Convert.ToInt32(ConfigurationManager.AppSettings["ProxyPort"]);

            option.SetPreference("network.proxy.type", 1);
            option.SetPreference("network.proxy.http", proxy);
            option.SetPreference("network.proxy.http_port", port);
            option.SetPreference("network.proxy.ftp", proxy);
            option.SetPreference("network.proxy.ftp_port", port);
            option.SetPreference("network.proxy.ssl", proxy);
            option.SetPreference("network.proxy.ssl_port", port);
            option.SetPreference("network.proxy.socks", proxy);
            option.SetPreference("network.proxy.socks_port", port);
            option.SetPreference("network.proxy.no_proxies_on", $"localhost, 127.0.0.1{ noProxy }");

            return option;
        }
    }
}
