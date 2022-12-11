using ApplicationCore.Models;
using Microsoft.Extensions.DependencyInjection;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using WPF.Interfaces;
using WPF.Services;
using WPF.Static;
using WPF.Converters;

namespace WPF.Parsers
{
    public class FonBetModelCreater
    {
        private ServiceProvider _services;
        private IBetService _betservice;

        public FonBetModelCreater()
        {
            _services = ServiceProviderFactory.Get;
            _betservice = _services.GetService<IBetService>()!;
        }
        public Bet CreateModels (IWebElement element)
        {
            try
            {
                var model = new Bet();
                var listCoef = new List<double>();
                model.Name = element.FindElement(By.CssSelector(SearchElements.BetName)).Text;
                model.Score = element.FindElement(By.CssSelector(SearchElements.BetScore)).Text;
                var time = element.FindElement(By.CssSelector(SearchElements.BetTime)).Text;
                model.BetTime = new TimeSpan().ConvertToTimeSpan(time);
                model.AuxiliaryLocator = element.ToString()!;

                var betCoefficient = element.FindElements(By.CssSelector(SearchElements.BetCoefficient));
                for (int i = 0; i < 3; i++)
                {
                    var coefString = betCoefficient[i].Text != "-" ? betCoefficient[i].Text.Replace(".", ",") : "0,0";
                    var coefNumber = double.Parse(coefString);
                    listCoef.Add(coefNumber);
                }
                var coefficient = new Coefficient() 
                {
                    RatioFirst = listCoef[0],
                    RatioSecond = listCoef[1],
                    RatioThird = listCoef[2],
                    Score = model.Score,
                    BetTime = model.BetTime
                };
                
                model.Coefficients.Add(coefficient);

                lock (MainWindow.locker)
                {
                    _betservice.AddBetAsync(model);
                }

                return model;
            }
            catch
            {
                return new Bet();
            }
        }
    }
}
