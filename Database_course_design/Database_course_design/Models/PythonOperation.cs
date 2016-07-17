﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IronPython.Hosting;
using IronPython;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using System.Drawing.Imaging;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using System.Threading;
using Database_course_design.Models.ItemModel;


namespace Database_course_design.Models
{
    public class PythonOperation
    {
        private const string URL = "http://urp.tongji.edu.cn/index.portal";
        private const string IE_DRIVER_PATH = @"C:\Path";

        public UserInfo spider(string userid,string userkey)
        {
            var userInfo = new UserInfo();
            var options = new InternetExplorerOptions()
            {
                InitialBrowserUrl = URL,
                IntroduceInstabilityByIgnoringProtectedModeSettings = true,
                IgnoreZoomLevel = true
            };

            //爬虫门户网站 获取个人信息
            var driver = new InternetExplorerDriver(IE_DRIVER_PATH, options);
            driver.Navigate();
            var userIdElement = driver.FindElement(By.Id("username"));
            userIdElement.SendKeys(userid);
            var passwordElement = driver.FindElement(By.Id("password"));
            passwordElement.SendKeys(userkey);
            var submitElement = driver.FindElement(By.Name("submit"));
            submitElement.Submit();
            Thread.Sleep(5000);
            var pageHtml = driver.PageSource;

            //正则表达式匹配
            Regex reg = new Regex("<LI style=\"MARGIN-TOP: 1px; COLOR: #3e82c3\" _extended=\"true\">(.+)，您好！ </LI>");
            Match match = reg.Match(pageHtml);
            userInfo.name = match.Groups[1].Value;
            userInfo.number = userid;
            reg = new Regex("<LI style=\"MARGIN-TOP: 2px\" _extended=\"true\">身份： <SPAN>(.+)</SPAN> </LI>");
            match = reg.Match(pageHtml);
            userInfo.identity = match.Groups[1].Value;
            reg = new Regex("<LI style=\"MARGIN-TOP: 2px\" _extended=\"true\">部门：(.+)</LI>");
            match = reg.Match(pageHtml);
            userInfo.department = match.Groups[1].Value;

            return userInfo;
            //关闭浏览器
            driver.Close(); // closes browser
            driver.Quit(); //
        }
    }
}