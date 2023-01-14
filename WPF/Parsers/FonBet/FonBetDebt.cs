using ApplicationCore.Models;
using Microsoft.Extensions.DependencyInjection;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Interfaces;
using WPF.Parsers.Base;
using WPF.Services.Factory;
using WPF.Static;

namespace WPF.Parsers.FonBet
{
    public class FonBetDebt : BetDebt
    {
        private readonly IBetService _betService;

        public FonBetDebt()
        {
            var services = ServiceProviderFactory.Get;
            _betService = services.GetService<IBetService>()!;
        }
        public override async Task<bool> CheckAndDebt(IWebElement webElement, Bet bet)
        {
            var betDb = await _betService.GetBetByName(bet.Name);
            if (betDb == null)
            {
                Debug.WriteLine($"------------> Добавление новой ставки в базу \'{bet.Name}\'");
                return false;
            }

            // Проверяем что ещё ставка не совершена, иначе выходим
            var coefficients = betDb.Coefficients.FirstOrDefault(c => c.IsMadeBet == true);
            if (coefficients != null)
                return false;

            // Проверяем что счет не равный, иначе выходим
            var result = bet.Score.Split(':')?.Select(int.Parse)?.ToList();
            if (result![0] == result![1])
                return false;

            // Выбираем команду победителя и делаем ставку
            var betCoefficients = webElement.FindElements(By.CssSelector(SearchElements.BetCoefficient));
            var betTo = result[0] > result[1] ? 0 : 2;
            betCoefficients[betTo].Click();

            return true;
        }
    }
}
