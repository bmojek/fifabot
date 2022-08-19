using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace fifabot
{
    internal class Program
    {
        public static List<string> ScanCoins()
        {
            var url = "https://www.mmoga.com/FIFA-Coins/FUT-Coins-Sell/";
            var web = new HtmlWeb();
            var content = web.Load(url);
            var platforms = content.QuerySelectorAll("form p");
            var list = new List<string>();
            foreach (var x in platforms)
            {
                list.Add(x.InnerText.ToString());
            }
            return list;
        }

        public static void BuyCoins(int platform, int amount)
        {
            string paypal = "bartekmojek@gmail.com";

            var driver = new ChromeDriver();
            var url = "https://www.mmoga.com/FIFA-Coins/FUT-Coins-Sell/";
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl(url);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));

            IReadOnlyCollection<IWebElement> platformSelect = wait.Until(i => i.FindElements(By.ClassName("futSellName")));
            platformSelect.ElementAt(platform).Click();

            IReadOnlyCollection<IWebElement> btn = wait.Until(i => i.FindElements(By.XPath("//button[@class = 'commButton commKFRight']")));
            btn.ElementAt(0).Click();
            System.Threading.Thread.Sleep(100);

            IWebElement coinAmount = wait.Until(i => i.FindElement(By.Id("coinSelect")));
            SelectElement selectElement = new SelectElement(coinAmount);
            selectElement.SelectByValue(amount.ToString());
            System.Threading.Thread.Sleep(100);

            IReadOnlyCollection<IWebElement> btn2 = wait.Until(i => i.FindElements(By.XPath("//button[@class = 'commButton']")));
            btn2.ElementAt(1).Click();

            IWebElement payment = wait.Until(i => i.FindElement(By.Name("payment")));
            SelectElement paymentSelect = new SelectElement(payment);
            paymentSelect.SelectByValue("paypal");
            System.Threading.Thread.Sleep(100);

            IReadOnlyCollection<IWebElement> paypalEmail = wait.Until(i => i.FindElements(By.CssSelector("input")));

            Random random = new Random();
            for (int i = 0; i < paypal.Length; i++)
            {
                paypalEmail.ElementAt(5).SendKeys(paypal[i].ToString());
            }
            for (int i = 0; i < paypal.Length; i++)
            {
                paypalEmail.ElementAt(6).SendKeys(paypal[i].ToString());
            }
            IReadOnlyCollection<IWebElement> btn3 = wait.Until(i => i.FindElements(By.XPath("//button[@class = 'commButton']")));
            btn3.ElementAt(1).Click();
        }

        public static void Main(string[] args)
        {
            while (true)
            {
                List<string> result = ScanCoins();
                Console.Clear();
                Console.WriteLine("PS   " + result.ElementAt(1).Substring(29));
                Console.WriteLine("XBOX " + result.ElementAt(3).Substring(29));
                Console.WriteLine("PC   " + result.ElementAt(5).Substring(29));

                if (result.ElementAt(1).Contains("Demand: 0")) ; else Console.WriteLine("|||||||||||| COINSY PS ||||||||||||");
                if (result.ElementAt(3).Contains("Demand: 0")) ; else Console.WriteLine("|||||||||||| COINSY XBOX ||||||||||||");
                if (result.ElementAt(5).Contains("Demand: 0")) ;
                else
                {
                    Console.WriteLine("|||||||||||| COINSY PC ||||||||||||");
                    Console.Beep(370, 500);
                    BuyCoins(2, 150); return;
                }
                Thread.Sleep(6000);
            }
        }
    }
}