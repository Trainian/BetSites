using ApplicationCore.Interfaces;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Static;
using OpenQA.Selenium.Support.UI;
using System.Windows.Controls;

namespace WPF.Parsers
{
    public class FonBetParser : IBaseParser
    {
        public TextBox? Information { get; set; }
        public async Task Run()
        {
            var curDer = Directory.GetCurrentDirectory();
            IWebDriver driver = new ChromeDriver(curDer);
            var waiter = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            driver.Navigate().GoToUrl(@"https://www.fon.bet/live/");
            await Task.Delay(40000);

            var sports = waiter.Until(e => e.FindElements(By.CssSelector(SearchElements.Bet)));

            Information?.AppendText(sports[0].Text);

            var parsModels = new FonBetModelCreater() { Information = this.Information };
            var el = parsModels.CreateModels(sports[0]);

            //var ss = listEl[3];
            //foreach (var item in sports)
            //{
            //    var enb = (string)((WebElement)item).Coordinates.AuxiliaryLocator;
            //    if (enb == ss)
            //    {
            //        waiter.Until(e => item.FindElement(By.CssSelector(SearchElements.BetCoefficient))).Click();
            //        break;
            //    }
            //}
            Console.WriteLine();
        }
    }
}

/// Скроллинг
//var scrollElement = driver.FindElement(By.CssSelector(SearchElements.Scroll));
//var scrollOrigin = new WheelInputDevice.ScrollOrigin
//{
//    Element = scrollElement,
//    XOffset = 0,
//    YOffset = 0
//};

//new Actions(driver)
//    .ScrollFromOrigin(scrollOrigin, 0, 500)
//    .Perform();

//await Task.Delay(1000);

//new Actions(driver)
//    .ScrollFromOrigin(scrollOrigin, 0, -500)
//    .Perform();