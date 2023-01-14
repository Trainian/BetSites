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
        public async IAsyncEnumerable<string> Run(double sum, double debt, bool isOptionalSimulate, double minRate, double maxRate , bool isBetToWinner)
        {
            var sumNow = sum;
            IEnumerable<Bet> bets;

            bets = isOptionalSimulate == true 
                ? await _betService.GetAllBetsWithTheEnd() 
                : await _betService.GetAllBetsWithDebtAndWithTheEnd();

            if (bets.Count() != 0)
            {
                foreach (var bet in bets)
                {
                    var coefIsMade = isOptionalSimulate == true
                        ? await _coefService.GetOptionalCoefficientByBetId(bet.Id, minRate, maxRate, isBetToWinner)
                        : await _coefService.GetIsMadeBetCoefficientByBetId(bet.Id);

                    if (coefIsMade == null)
                        continue;

                    var coefIsWin = await _coefService.GetWinnerCoefficientByBetId(bet.Id);

                    var whoBet = GetWhoBet(coefIsMade);
                    var whoWin = GetWinnerByScore(coefIsWin);
                    var ratio = GetRatio(coefIsMade, whoBet);
                    sumNow = whoBet == whoWin ? sumNow + (debt*ratio-debt) : sumNow - debt;
                    var result = whoBet == whoWin 
                        ? $"+++ Ставка {debt}руб. победила, коэф:{ratio} - выиграно:{debt*ratio-debt}." 
                        : $"--- Ставка {debt}руб. проиграла, коэф:{ratio}.";
                    yield return result + $" Текущий счет: {sumNow}";
                }
            }
            yield break;
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

        private double GetRatio(Coefficient coefficient, WinnerEnum whoRatio)
        {
            double ratio = 0;
            switch (whoRatio)
            {
                case WinnerEnum.FirstWin:
                    ratio = coefficient.RatioFirst;
                    break; 
                case WinnerEnum.SecondWin:
                    ratio = coefficient.RatioThird;
                    break;
                default:
                    ratio = coefficient.RatioSecond;
                    break;
            }
            return ratio;
        }
    }
}
