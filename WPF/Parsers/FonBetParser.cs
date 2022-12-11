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
using Infrastructure.Data;
using ApplicationCore.Repositories;
using WPF.Interfaces;
using System.Windows.Threading;
using Microsoft.Extensions.DependencyInjection;
using WPF.Services;
using System.Windows;
using ApplicationCore.Models;
using System.Diagnostics;
using System.Threading;

namespace WPF.Parsers
{
    public class FonBetParser : IBaseParser
    {
        private IServiceProvider _services;
        private IBetService _betservice;

        public FonBetParser()
        {

        }

        public async Task Run(CancellationToken token, int scrolls = 0)
        {
            int curScroll = 0;
            var curDer = Directory.GetCurrentDirectory();
            IWebDriver driver = new ChromeDriver(curDer);
            driver.Manage().Timeouts().ImplicitWait = new TimeSpan(0, 0, 3);
            var waiter = new WebDriverWait(driver, TimeSpan.FromSeconds(1));
            waiter.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
            driver.Navigate().GoToUrl(@"https://www.fon.bet/live/");

            do
            {
                await ParsSite(driver, waiter);
                ++curScroll;

                if(curScroll < scrolls)
                {
                    Scroll(driver, 600);
                }
                else
                {
                    curScroll = 0;
                    Scroll(driver, -1000 * scrolls);
                }
            } while (!token.IsCancellationRequested);
            
            driver.Close();

            await Task.CompletedTask;

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
        }

        private async Task ParsSite(IWebDriver driver, WebDriverWait waiter)
        {
            IReadOnlyCollection<IWebElement> sports = new List<IWebElement>();
            do
            {
                sports = waiter.Until(e => e.FindElements(By.CssSelector(SearchElements.Bet)));
            }
            while (sports.Count == 0);


            var parsModels = new FonBetModelCreater();
            foreach (var bet in sports)
            {
                try
                {

                    var name = bet.Text; //bet.FindElement(By.CssSelector(SearchElements.BetName)).Text;
                    if (name.Contains("—"))
                    {
                        parsModels.CreateModels(bet);
                    }

                }
                catch (StaleElementReferenceException){ }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
            
        }
        /// Скроллинг
        public void Scroll(IWebDriver driver, int scrollAmount)
        {
            var scrollElement = driver.FindElement(By.CssSelector(SearchElements.Scroll));
            var scrollOrigin = new WheelInputDevice.ScrollOrigin
            {
                Element = scrollElement,
                XOffset = 0,
                YOffset = 0
            };

            new Actions(driver)
                .ScrollFromOrigin(scrollOrigin, 0, scrollAmount)
                .Perform();

            Task.Delay(500);
        }

    }
}




