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
    public class CoefficientService : ICoefficientService
    {
        private ICoefficientRepository _repository;

        public CoefficientService(ICoefficientRepository repository)
        {
            _repository = repository;
        }
        public async Task<Coefficient> GetWinnerCoefficientByBetId(int betId)
        {
            var coefficients = await _repository.GetAsync(c => c.BetId == betId);
            coefficients.OrderBy(c => c.Id);
            return coefficients.Last();
        }
        public async Task<Coefficient?> GetIsMadeBetCoefficientByBetId(int betId)
        {
            var coefficients = await _repository.GetAsync(c => c.BetId == betId && c.IsMadeBet == true);
            coefficients.OrderBy(c => c.Id);
            return coefficients.FirstOrDefault();
        }

        public async Task<Coefficient?> GetOptionalCoefficientByBetId(int betId, decimal minRate, decimal maxRate, bool isBetToWinner)
        {
            var coefficients = await _repository.GetAsync(c => c.BetId == betId);
            List<Coefficient> coefficientsSorted = new List<Coefficient>();
            
            foreach(var coefficient in coefficients)
            {
                if (coefficient.RatioFirst == coefficient.RatioThird)
                    continue;

                var ratio = coefficient.RatioFirst < coefficient.RatioThird ? (decimal)coefficient.RatioFirst : (decimal)coefficient.RatioFirst;
                if (ratio > minRate && ratio < maxRate)
                    coefficientsSorted.Add(coefficient);
            }

            if (coefficientsSorted.Count == 0)
                return null;

            coefficientsSorted.OrderBy(c => c.Id);

            if(isBetToWinner)
            {
                foreach(var coefficient in coefficients)
                {
                    var score = coefficient.Score.Split(':');
                    if (score[0] == score[1])
                        continue;
                    else
                        return coefficient;
                }
                return null;
            }

            return coefficientsSorted.First();
        }

    }
}
