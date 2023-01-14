using ApplicationCore.Models;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF.Parsers.Base
{
    public abstract class BetDebt
    {
        public abstract Task<bool> CheckAndDebt(IWebElement webElement, Bet bet);
    }
}
