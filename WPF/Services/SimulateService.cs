using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Enums;
using WPF.Interfaces;

namespace WPF.Services
{
    public class SimulateService : ISimulateService
    {
        private IBetService _betService;
        private ICoefficientService _coefService;

        public SimulateService(IBetService betService, ICoefficientService coefService)
        {
            _betService = betService;
            _coefService = coefService;
        }
        public async IAsyncEnumerable<string> Run(decimal sum, decimal debt, DateTime startDate, DateTime endDate, bool isOptionalSimulate, decimal minRate, decimal maxRate , bool isBetToWinner)
        {
            var sumNow = sum;
            IEnumerable<Bet> bets;

            bets = isOptionalSimulate == true
                ? await _betService.GetBetsWithTheEndByDate(startDate, endDate)
                : await _betService.GetBetsWithDebtAndWithTheEndByDate(startDate, endDate);

            if (bets.Count() != 0)
            {
                foreach (var bet in bets)
                {
                    var resultSimulate = SimulateBet(bet, isOptionalSimulate, minRate, maxRate, isBetToWinner).Result;

                    var winOrLose = resultSimulate.winOrNot;
                    var ratio = resultSimulate.ratio;

                    if (winOrLose == WinOrLose.NoBet)
                        continue;

                    sumNow = winOrLose == WinOrLose.Winning ? sumNow + (debt*ratio-debt) : sumNow - debt;

                    var result = winOrLose == WinOrLose.Winning 
                        ? $"+++ Ставка {debt}руб. победила, коэф:{ratio} - выиграно:{debt*ratio-debt}." 
                        : $"--- Ставка {debt}руб. проиграла, коэф:{ratio}.";
                    yield return result + $" Текущий счет: {sumNow}";
                }
            }
            yield break;
        }

        public async IAsyncEnumerable<string> FindOptimalSettings(decimal sum, decimal debt, DateTime startDate, DateTime endDate, decimal minRate, decimal maxRate, bool isBetToWinner)
        {
            IEnumerable<Bet> bets;
            decimal step = 0.01M;
            Dictionary<string, decimal> betList = new Dictionary<string, decimal>();
            IOrderedEnumerable<KeyValuePair<string, decimal>> betListSorted;
            IEnumerable<KeyValuePair<string, decimal>> best;

            bets = await _betService.GetAllBetsWithTheEnd();

            if (bets.Count() != 0)
            {
                yield return "-----Перебираем значения и отображаем итог сделок-----\n" +
                             "--Минимум коэф.--|--Максмум коэф.--|--Итоговая сумма";
                // Итерация минимума ставки, с повышением каждый раз на 0.01
                for (decimal iMin = minRate; iMin <= maxRate; iMin = iMin + step)
                {
                    // Итерация максимума ставки, с уменьшением каждый раз на 0.01
                    for(decimal iMax = maxRate; iMax >= iMin; iMax = iMax - step)
                    {
                        decimal sumNow = sum;
                        foreach (var bet in bets)
                        {
                            var resultSimulate = SimulateBet(bet, true, iMin, iMax, isBetToWinner).Result;

                            var winOrLose = resultSimulate.winOrNot;
                            var ratio = resultSimulate.ratio;

                            if (winOrLose == WinOrLose.NoBet)
                                continue;

                            sumNow = winOrLose == WinOrLose.Winning ? sumNow + (debt * ratio - debt) : sumNow - debt;
                        }
                        string textResult = $"--{iMin,25}--|--{iMax,20}--|--{sumNow,20}";
                        yield return textResult;

                        if(sumNow > sum)
                            betList.Add(textResult, sumNow);
                    }
                }
                betListSorted = betList.OrderByDescending(l => l.Value);
                best = betListSorted.Take(10);
                yield return "\n\n-----Топ 10 лучших коэффициентов-----\n"+
                    "--Минимум коэф.--|--Максмум коэф.--|--Итоговая сумма";
                foreach (var coef in best)
                {
                    yield return coef.Key;
                }

            }
        }

        private async Task<(WinOrLose winOrNot, decimal ratio)> SimulateBet (Bet bet, bool isOptionalSimulate, decimal minRate, decimal maxRate, bool isBetToWinner)
        {
            var coefIsMade = isOptionalSimulate == true
                ? await _coefService.GetOptionalCoefficientByBetId(bet.Id, minRate, maxRate, isBetToWinner)
                : await _coefService.GetIsMadeBetCoefficientByBetId(bet.Id);

            if (coefIsMade == null)
                return (WinOrLose.NoBet, 0);

            var coefIsWin = await _coefService.GetWinnerCoefficientByBetId(bet.Id);

            var whoBet = GetWhoBet(coefIsMade);
            var whoWin = GetWinnerByScore(coefIsWin);

            decimal ratio = GetRatio(coefIsMade, whoBet);

            return whoBet == whoWin
                ? (WinOrLose.Winning, ratio)
                : (WinOrLose.Losing, ratio);
        }

        private WinnerEnum GetWhoBet(Coefficient coefficient)
        {
            return coefficient.RatioFirst < coefficient.RatioThird ? WinnerEnum.FirstWin : WinnerEnum.SecondWin;
        }

        private WinnerEnum GetWinnerByScore(Coefficient coefficient)
        {
            var score = coefficient.Score.Split(':');
            var firstComScore = int.Parse(score[0]);
            var secondComScore = int.Parse(score[1]);

            if (firstComScore > secondComScore)
                return WinnerEnum.FirstWin;
            else if (firstComScore < secondComScore)
                return WinnerEnum.SecondWin;
            else
                return WinnerEnum.NoOneWin;
        }

        private decimal GetRatio(Coefficient coefficient, WinnerEnum whoRatio)
        {
            decimal ratio = 0;
            switch (whoRatio)
            {
                case WinnerEnum.FirstWin:
                    ratio = (decimal)coefficient.RatioFirst;
                    break; 
                case WinnerEnum.SecondWin:
                    ratio = (decimal)coefficient.RatioThird;
                    break;
                default:
                    ratio = (decimal)coefficient.RatioSecond;
                    break;
            }
            return ratio;
        }
    }
}
