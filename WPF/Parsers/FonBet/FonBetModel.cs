﻿using ApplicationCore.Models;
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
using WPF.Services.Factory;
using System.Diagnostics;

namespace WPF.Parsers.FonBet
{
    public class FonBetModel
    {
        private ServiceProvider _services;
        private IBetService _betservice;

        public FonBetModel()
        {
            _services = ServiceProviderFactory.Get;
            _betservice = _services.GetService<IBetService>()!;
        }
        public Bet CreateModels(IWebElement element)
        {
            try
            {
                var model = new Bet();
                var listCoef = new List<decimal>();
                var time = "00:00";
                model.Name = element.FindElement(By.CssSelector(SearchElements.BetName)).Text;
                model.Score = element.FindElement(By.CssSelector(SearchElements.BetScore)).Text;
                try
                {
                    time = element.FindElement(By.CssSelector(SearchElements.BetTime)).Text;
                }
                catch (Exception ex) { Debug.WriteLine($"------------> У ставки поле ВРЕМЯ было пустое..."); }
                model.BetTime = new TimeSpan().ConvertToTimeSpan(time);
                model.AuxiliaryLocator = element.ToString()!;

                var betCoefficient = element.FindElements(By.CssSelector(SearchElements.BetCoefficient));
                for (int i = 0; i < 3; i++)
                {
                    var coefString = betCoefficient[i].Text != "-" ? betCoefficient[i].Text.Replace(".", ",") : "0,0";
                    var coefNumber = decimal.Parse(coefString);
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

                return model;
            }
            catch
            {
                return new Bet();
            }
        }
    }
}
