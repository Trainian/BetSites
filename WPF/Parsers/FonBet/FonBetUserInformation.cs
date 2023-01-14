using ApplicationCore.Models;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Static;
using WPF.ViewModels;

namespace WPF.Parsers.FonBet
{
    public class FonBetUserInformation
    {
        private UserInformation userInfo;

        public FonBetUserInformation()
        {
            userInfo = MainWindow.MainViewModel.UserInformation;
        }
        public void UpdateUserInformation(IWebDriver driver)
        {
            userInfo.Balance = Int32.Parse(driver.FindElement(By.CssSelector(SearchElements.Balance)).Text);
        }
    }
}
