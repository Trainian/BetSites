using ApplicationCore.Models;
using Microsoft.Extensions.DependencyInjection;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WPF.Enums;
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
        public override async Task<bool> CheckAndDebt(IWebElement webElement, Bet bet, decimal minRation, decimal maxRatio, bool debtToWinner)
        {
            WhoDebt wb = WhoDebt.Nobody;
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

            // Ищем походящую ставку по заданному диапазону
            wb = GetRelevantCoefficient(bet, minRation, maxRatio);

            // Проверяем, совпадает ли предположительная ставка с побеждающей командой
            if (debtToWinner)
            {
                var winner = GetWinner(bet);

                // Если нет победителя, выходим
                if (winner == WhoDebt.Nobody && winner != wb)
                    wb = WhoDebt.Nobody;
            }

            if (wb == WhoDebt.Nobody)
                return false;

            // Выбираем коэффициенты, для клика, где 0 - Первая команда, 1 - Ничья, 2 - Вторая команда
            var betCoefficients = webElement.FindElements(By.CssSelector(SearchElements.BetCoefficient));
            var betTo = wb == WhoDebt.FirstCommand ? 0 : 2;
            betCoefficients[betTo].Click();

            return true;
        }

        private WhoDebt GetWinner (Bet bet)
        {
            // Проверяем что счет не равный, иначе выходим
            var result = bet.Score.Split(':')?.Select(int.Parse)?.ToList();
            if (result![0] == result![1])
                return WhoDebt.Nobody;

            return result[0] > result[1] ? WhoDebt.FirstCommand : WhoDebt.SecondCommand;
        }

        private WhoDebt GetRelevantCoefficient (Bet bet, decimal minRation, decimal maxRatio)
        {
            WhoDebt wb = WhoDebt.Nobody;
            var coefficient = bet.Coefficients.Last();
            wb = coefficient.RatioFirst >= minRation && coefficient.RatioFirst <= maxRatio ? WhoDebt.FirstCommand : WhoDebt.Nobody;
            if(wb == WhoDebt.Nobody)
                wb = coefficient.RatioThird >= minRation && coefficient.RatioThird <= maxRatio ? WhoDebt.SecondCommand : WhoDebt.Nobody;
            return wb;
        }
    }
}
