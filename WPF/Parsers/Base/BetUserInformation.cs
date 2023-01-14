using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF.Parsers.Base
{
    public abstract class BetUserInformation
    {
        public abstract void UpdateUserInformation(IWebDriver driver);
    }
}
