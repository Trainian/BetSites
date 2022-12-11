using ApplicationCore.Models;
using ApplicationCore.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Interfaces;

namespace WPF.Services
{
    public class StatisticService : IStatisticService
    {
        private IBetRepository _betRepository;

        public StatisticService(IBetRepository betRepository)
        {
            _betRepository = betRepository;
        }
        /// <summary>
        /// Высчитать процент изменения победителей, без учета игр не набравших очков ни одной командой за весь матч.
        /// </summary>
        /// <returns>Процент</returns>
        public async Task<decimal> GetPercentChangedWiners()
        {
            var betList = await _betRepository.GetAllAsync();
            var bets = betList.Where(b => b.Coefficients.Count > 10)
                //.Where(b => b.Coefficients.First().BetTime <= new TimeSpan(0,10,0))
                .Where(b => b.Coefficients.Last().Time <= DateTime.Now - new TimeSpan(0,30,0))
                .ToList();

            var betsCount = 0;
            if (bets.Count == 0)
                return 0;

            var betsChangedWinners = 0;
            foreach (var bet in bets)
            {
                var coefList = bet.Coefficients.ToList();
                WinnerEnum startWin = WinnerEnum.NoOneWin;
                WinnerEnum endWin = WinnerEnum.NoOneWin;
                foreach (var coef in coefList)
                {
                    var win = GetWinnerByScore(coef);

                    if (win == WinnerEnum.NoOneWin)
                        continue;

                    startWin = win;
                    betsCount++;
                    break;
                }

                var winner = coefList.Last();
                endWin = GetWinnerByScore(winner);

                if (startWin != endWin && startWin != WinnerEnum.NoOneWin)
                    betsChangedWinners++;
            }
            decimal perc = (decimal)(100 / ((decimal)betsCount / (decimal)betsChangedWinners));
            perc = Math.Round(perc, 2);
            return betsChangedWinners == 0 ? 100 : perc;
        }

        public async Task<decimal> GetPercentWins()
        {
            var betList = await _betRepository.GetAllAsync();
            var bets = betList.Where(b => b.Coefficients.Count > 10)
                .Where(b => b.Coefficients.First().BetTime <= new TimeSpan(0, 30, 0))
                .ToList();

            var betsCount = 0;
            var betWins = 0;
            foreach (var bet in bets)
            {
                var coefList = bet.Coefficients.ToList();
                WinnerEnum startWin = WinnerEnum.NoOneWin;
                WinnerEnum endWin = WinnerEnum.NoOneWin;

                var firstCoef = coefList.First();
                startWin = GetWinnerByCoefficient(firstCoef);
                betsCount++;

                var winner = coefList.Last();
                endWin = GetWinnerByScore(winner);

                if (startWin == endWin && startWin != WinnerEnum.NoOneWin)
                    betWins++;
            }

            decimal perc = (decimal)(100 / ((decimal)betsCount / (decimal)betWins));
            perc = Math.Round(perc, 2);
            return betWins == 0 ? 100 : perc;
        }

        public async Task<decimal> GetPercentWinsFavorits()
        {
            var betList = await _betRepository.GetAllAsync();
            var bets = betList.Where(b => b.Coefficients.Count > 10)
                .Where(b => b.Coefficients.First().BetTime <= new TimeSpan(0, 30, 0))
                .ToList();

            var betsCount = 0;
            var betWins = 0;
            foreach (var bet in bets)
            {
                var coefList = bet.Coefficients.ToList();
                WinnerEnum startWin = WinnerEnum.NoOneWin;
                WinnerEnum endWin = WinnerEnum.NoOneWin;

                foreach (var coef in coefList)
                {
                    var win = GetWinnerByScore(coef);

                    if (win == WinnerEnum.NoOneWin)
                        continue;

                    startWin = win;
                    betsCount++;
                    break;
                }

                var winner = coefList.Last();
                endWin = GetWinnerByScore(winner);
                if (startWin == endWin && startWin != WinnerEnum.NoOneWin)
                    betWins++;
            }

            decimal perc = (decimal)(100 / ((decimal)betsCount / (decimal)betWins));
            perc = Math.Round(perc, 2);
            return betWins == 0 ? 100 : perc;
        }

        private enum WinnerEnum
        {
            FirstWin,
            SecondWin,
            NoOneWin
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

        private WinnerEnum GetWinnerByCoefficient (Coefficient coefficient)
        {
            var firstComScore = coefficient.RatioFirst;
            var noOneComScore = coefficient.RatioSecond;
            var secondComScore = coefficient.RatioThird;

            if (firstComScore < 2.0)
                return WinnerEnum.FirstWin;
            else if (secondComScore < 2.0)
                return WinnerEnum.SecondWin;
            else
                return WinnerEnum.NoOneWin;
        }
    }
}
