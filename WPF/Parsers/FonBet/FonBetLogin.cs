using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Parsers.Base;
using WPF.Static;

namespace WPF.Parsers.FonBet
{
    public class FonBetLogin : BetLogin
    {
        public override bool TryToLogin(IWebDriver driver, string loginPhone, string loginPassword)
        {
            var login = driver.FindElement(By.CssSelector(SearchElements.CheckNotLoged));
            Task.Delay(1500);
            login.Click();
            driver.FindElement(By.CssSelector(SearchElements.LoginPhoneForm)).SendKeys(loginPhone);
            driver.FindElement(By.CssSelector(SearchElements.LoginPasswordForm)).SendKeys(loginPassword);
            driver.FindElement(By.CssSelector(SearchElements.LoginButton)).Click();
            Task.Delay(2000);
            var balance = driver.FindElement(By.CssSelector(SearchElements.Balance));
            return balance != null ? true : false;
        }
    }
}
