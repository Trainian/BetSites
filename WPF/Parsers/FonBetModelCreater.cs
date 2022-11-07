using ApplicationCore.Models;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using WPF.Static;

namespace WPF.Parsers
{
    public class FonBetModelCreater
    {
        public TextBox? Information { get; set; }
        public Bet CreateModels (IWebElement element)
        {
            var model = new Bet() { Id = 1 };
            model.AuxiliaryLocator = element.ToString()!;
            model.Name = element.FindElement(By.CssSelector(SearchElements.BetName)).Text;
            var betCoefficient = element.FindElements(By.CssSelector(SearchElements.BetCoefficient));
            var listCoef = new List<Coefficient>();
            for(int i=0; i < 3; i++)
            {
                double number = 0.0;
                double.TryParse(betCoefficient[i].Text.Trim(), out number);
                var coef = new Coefficient()
                {
                    Id = i,
                    BetId = model.Id,
                    Time = DateTime.Now,
                    Ratio = number
                };
                listCoef.Add(coef);
            }
            model.Coefficients = listCoef;
            return model;
        }
    }
}
