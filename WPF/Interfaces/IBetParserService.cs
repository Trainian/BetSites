using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace WPF.Interfaces
{
    public interface IBetParserService
    {
        public abstract Task Run(CancellationToken token);
        public void CloseDriver(string message);
    }
}
