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
using OpenQA.Selenium.DevTools;
using WPF.Services.Factory;
using Newtonsoft.Json.Linq;

namespace WPF.Parsers
{
    public class FonBetParser : IBaseParser
    {
        private IBetService _betservice;
        private IWebDriver _driver;
        private int _scrolls = 1;
        private int _curScroll = 1;
        private string _curDer = Directory.GetCurrentDirectory();
        private Random _random = new Random();
        private string _loginPhone;
        private string _loginPassword;

        public FonBetParser()
        {
            // Настраиваем Сервисы
            var services = ServiceProviderFactory.Get;
            _betservice = services.GetService<IBetService>()!;

            // Настраиваем Драйвер
            _driver = new ChromeDriver(_curDer);
            _driver.Manage().Window.Size = new System.Drawing.Size(1152, 864);
            _driver.Manage().Timeouts().ImplicitWait = new TimeSpan(0, 0, 3);
        }

        public async Task Run(CancellationToken token, string loginPhone, string loginPassword, int scrolls = 1)
        {
            _scrolls = scrolls;
            _loginPhone = loginPhone;
            _loginPassword = loginPassword;
            _driver.Navigate().GoToUrl(@"https://www.fon.bet/live/");

            // Регистрируем Токен остановки потока
            token.Register(() =>
            {
                CloseDriver("Парсинг остановлен");
                return;
            });

            try
            {
                await Task.Delay(3000);
                // Пытаемся загрузить сайт, иначе закрываем Драйвер и выводим сообщение
                var waiterLoading = WaiterLoading(_driver);
                if (waiterLoading == false)
                {
                    CloseDriver("Проблема с загрузкой сайта");
                    return;
                }

                // Проверяем что был выполнен вход, иначе закрываем Драйвер и выводим сообщение
                var loged = TryToLogin(_driver, _loginPhone, _loginPassword);
                if (loged != true)
                {
                    CloseDriver("Вход не выполнен");
                    return;
                }

                // Начинаем парсинг сайта
                do
                {
                    await ParsSite(_driver);
                    Scroll();
                } while (true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void CloseDriver(string message)
        {
            MessageBox.Show(message);
            _driver.Close();
            _driver.Dispose();
        }

        private bool WaiterLoading (IWebDriver driver, string searchElement = SearchElements.SportSection)
        {
            IWebElement? loader = null;
            var iteration = 0;
            do
            {
                try
                {
                    loader = _driver.FindElement(By.CssSelector(SearchElements.SportSection));
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Не получилось загрузиться лоадеру, {ex.Message}");
                }
                finally
                {
                    iteration++;
                    Task.Delay(1500);
                }
            } while (loader == null && iteration < 10);
            if (iteration >= 10)
                return false;
            return true;
        }

        private bool TryToLogin (IWebDriver driver, string loginPhone, string loginPassword)
        {
            var login = _driver.FindElement(By.CssSelector(SearchElements.CheckNotLoged));
            Task.Delay(1500);
            login.Click();
            _driver.FindElement(By.CssSelector(SearchElements.LoginPhoneForm)).SendKeys(loginPhone);
            _driver.FindElement(By.CssSelector(SearchElements.LoginPasswordForm)).SendKeys(loginPassword);
            _driver.FindElement(By.CssSelector(SearchElements.LoginButton)).Click();
            Task.Delay(2000);
            var balance = _driver.FindElement(By.CssSelector(SearchElements.Balance));
            return balance != null ? true : false;
        }

        private async Task ParsSite(IWebDriver driver)
        {
            IReadOnlyCollection<IWebElement> sports = new List<IWebElement>();
            do
            {
                sports = _driver.FindElements(By.CssSelector(SearchElements.Bet));
            }
            while (sports.Count == 0);


            var parsModels = new FonBetModelCreater();
            var debtService = new FonbetDebtService();
            foreach (var bet in sports)
            {
                try
                {
                    var name = bet.Text;
                    if (name.Contains("—"))
                    {
                        var newBet = parsModels.CreateModels(bet);
                        var debt = await debtService.CheckAndDebt(bet, newBet);
                        if (debt == true)
                        {
                            newBet.Coefficients.Last().IsMadeBet = true;
                            await Task.Delay(_random.Next(1500,5000));
                        }

                        lock (MainWindow.locker)
                        {
                            _betservice.AddBetAsync(newBet).Wait();
                        }
                    }
                }
                catch (StaleElementReferenceException){ }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
            }
            
        }
        /// Скроллинг
        public void Scroll()
        {
            var heightScroll = 0;

            if (_curScroll < _scrolls)
            {
                _curScroll++;
                heightScroll = 600;
            }
            else
            {
                _curScroll = 1;
                heightScroll =  -1000 * _scrolls;
            }

            var scrollElement = _driver.FindElement(By.CssSelector(SearchElements.ScrollElement));
            var scrollOrigin = new WheelInputDevice.ScrollOrigin
            {
                Element = scrollElement,
                XOffset = 0,
                YOffset = 0
            };

            new Actions(_driver)
                .ScrollFromOrigin(scrollOrigin, 0, heightScroll)
                .Perform();

            Task.Delay(500);
        }

    }
}