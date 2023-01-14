using OpenQA.Selenium.Interactions;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Static;
using ApplicationCore.Models;

namespace WPF.Parsers.Base
{
    public abstract class BetPars
    {
        public abstract Task<ICollection<Bet>> ParsSite(IWebDriver driver, bool betIsDisabled);
        public virtual void Scroll(IWebDriver driver, int currentScroll, int setScrolls)
        {
            var heightScroll = 0;

            if (currentScroll < setScrolls)
            {
                currentScroll++;
                heightScroll = 600;
            }
            else
            {
                currentScroll = 1;
                heightScroll = -1000 * setScrolls;
            }

            var scrollElement = driver.FindElement(By.CssSelector(SearchElements.ScrollElement));
            var scrollOrigin = new WheelInputDevice.ScrollOrigin
            {
                Element = scrollElement,
                XOffset = 0,
                YOffset = 0
            };

            new Actions(driver)
                .ScrollFromOrigin(scrollOrigin, 0, heightScroll)
                .Perform();

            Task.Delay(500);
        }
    }
}
