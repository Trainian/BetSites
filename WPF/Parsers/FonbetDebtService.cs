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
using WPF.Services.Factory;
using WPF.Static;

namespace WPF.Parsers
{
    public class FonbetDebtService : IDebtService
    {
        private readonly IBetService _betService;

        public FonbetDebtService()
        {
            var services = ServiceProviderFactory.Get;
            _betService = services.GetService<IBetService>()!;
        }
        public async Task<bool> CheckAndDebt(IWebElement webElement, Bet bet)
        {
            var betDb = await _betService.GetBetByName(bet.Name);
            if(betDb == null)
            {
                Debug.WriteLine("------------> Список Ставок был пуст...");
                return false;
            }

            // Проверяем что ещё ставка не совершена, иначе выходим
            var coefficients = betDb.Coefficients.FirstOrDefault(c => c.IsMadeBet == true);
            if (coefficients != null)
                return false;

            // Проверяем что счет не равный, иначе выходим
            var result = bet.Score.Split(':')?.Select(Int32.Parse)?.ToList();
            if (result![0] == result![1])
                return false;

            // Выбираем команду победителя и делаем ставку
            var betCoefficients = webElement.FindElements(By.CssSelector(SearchElements.BetCoefficient));
            var betTo = result[0] > result[1] ? 0 : 2;
            betCoefficients[betTo].Click();

            ///TODO: Добавить клики на сумму ставки и на сделать ставку
            return true;
        }
    }
}
