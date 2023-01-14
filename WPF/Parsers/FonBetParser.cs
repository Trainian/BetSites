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
using WPF.ViewModels;
using WPF.Parsers.FonBet;

namespace WPF.Parsers
{
    public class FonBetParser : IBetParserService
    {
        private IWebDriver _driver;
        private WebDriverWait _driverWaiter;
        private IBetService _betService;
        private int _scrolls = 1;
        private int _curScroll = 1;
        private int _betRate = 50;
        private bool _betIsDisabled = false;
        private string _curDer = Directory.GetCurrentDirectory();
        private string _loginPhone;
        private string _loginPassword;
        private MainViewModel _mainViewModel;

        private FonBetPars pars;
        private FonBetLogin login;
        private FonBetUserInformation userInfo;

        public FonBetParser()
        {
            // Настраиваем Сервисы
            var services = ServiceProviderFactory.Get;
            _betService = services.GetService<IBetService>()!;

            // Настраиваем Драйвер
            _driver = new ChromeDriver(_curDer);
            _driver.Manage().Window.Size = new System.Drawing.Size(1152, 864);
            _driver.Manage().Timeouts().ImplicitWait = new TimeSpan(0, 0, 3);
            _driverWaiter = new WebDriverWait(_driver, TimeSpan.FromSeconds(3));
            _driverWaiter.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));

            // Загружаем и устанавливаем настройки
            _mainViewModel = MainWindow.MainViewModel;
            _loginPhone = _mainViewModel.Settings.PhoneNumber;
            _loginPassword = _mainViewModel.Settings.Password;
            _scrolls = _mainViewModel.Settings.Scrolls;
            _betRate = _mainViewModel.Settings.BetRate;
            _betIsDisabled = _mainViewModel.Settings.BetIsDisabled;

            pars = new FonBetPars();
            login = new FonBetLogin();
            userInfo = new FonBetUserInformation();
        }

        public async Task Run(CancellationToken token)
        {
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
                var loged = login.TryToLogin(_driver, _loginPhone, _loginPassword);
                if (loged != true)
                {
                    CloseDriver("Вход не выполнен");
                    return;
                }

                // Настройка для ставок, если не отключена
                if(!_betIsDisabled)
                    SetupBet(_driver, _betRate);

                // Начинаем парсинг сайта
                do
                {
                    userInfo.UpdateUserInformation(_driver);

                    var bets = await pars.ParsSite(_driver, _betIsDisabled);

                    foreach (var bet in bets)
                        await _betService.AddBetAsync(bet);

                    Scroll();
                } while (!token.IsCancellationRequested);
                Debug.WriteLine("Закрытие Парсинга");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public void CloseDriver(string message)
        {
            MessageBox.Show(message);
            _driver.Close();
            _driver.Dispose();
        }

        private bool WaiterLoading(IWebDriver driver, string searchElement = SearchElements.SportSection)
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

        private void SetupBet(IWebDriver driver, int betRate)
        {
            string betRateString = betRate.ToString();
            driver.FindElement(By.CssSelector(SearchElements.FastBetSwitcher)).Click();
            var oldRate = driver.FindElement(By.CssSelector(SearchElements.FastBetRate)).GetAttribute("value");
            for (int i = 0; i < oldRate.Length; i++)
            {
                driver.FindElement(By.CssSelector(SearchElements.FastBetRate)).SendKeys(Keys.Backspace);
            }
            driver.FindElement(By.CssSelector(SearchElements.FastBetRate)).SendKeys(betRateString);
        }

        private void Scroll()
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
                heightScroll = -1000 * _scrolls;
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

            Task.Delay(1000);
        }

    }
}