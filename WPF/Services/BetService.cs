using ApplicationCore.Models;
using ApplicationCore.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WPF.Interfaces;

namespace WPF.Services
{
    public class BetService : IBetService
    {
        private IBetRepository _betRepository;
        private ICoefficientRepository _coefficientRepository;

        public BetService(IBetRepository betRepository, ICoefficientRepository coefficientRepository)
        {
            _betRepository = betRepository;
            _coefficientRepository = coefficientRepository;
        }

        public async Task<Bet> AddBetAsync(Bet entity)
        {
            try
            {
                // Находим Ставку с таким же названием и текущей датой
                var bets = await _betRepository.GetAsync(bet =>
                    bet.Name == entity.Name &&
                    bet.CreateDate.Date == entity.CreateDate.Date);
                var bet = bets.FirstOrDefault();

                // Если новая ставка, создаем и возвращаем
                if (bet == null)
                    return await CreateBetAsync(entity);

                // Если в ставке были изминения, обновляем их
                if (!ComparerBet(entity, bet))
                    bet = await UpdateBetAsync(entity, bet);

                // Проверяем коэффициенты, если изменились, добавляем
                if (!ComparerCoefficient(entity.Coefficients.Last(), bet.Coefficients.Last()))
                    await AddCoefficientToBet(bet, entity.Coefficients.Last());

                return bet;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return new Bet();
        }

        public async Task<Bet> CreateBetAsync(Bet entity)
        {
            return await _betRepository.AddAsync(entity);
        }

        public async Task<IEnumerable<Bet>?> GetAllBetAsync()
        {
            return await _betRepository.GetAllAsync();
        }

        public async Task<Bet?> GetBetByIdAsync(int id)
        {
            return await _betRepository.GetByIdAsync(id);
        }

        public async Task<Bet?> GetBetByName(string name)
        {
            var bets = await _betRepository.GetAsync(b => b.Name == name);
            return bets.LastOrDefault();
        }

        public async Task<Bet> UpdateBetAsync(Bet entityNew, Bet entityDb)
        {
            entityDb.Name = entityNew.Name;
            entityDb.AuxiliaryLocator = entityNew.AuxiliaryLocator;
            entityDb.BetTime = entityNew.BetTime;
            entityDb.Score = entityNew.Score;
            await _betRepository.UpdateAsync(entityDb);
            return entityDb;
        }

        private async Task AddCoefficientToBet (Bet bet, Coefficient coefficient)
        {
            coefficient.BetId = bet.Id;
            coefficient.Time = DateTime.Now;
            //await _coefficientRepository.AddAsync(coefficient);
            await App.Current.Dispatcher.Invoke(async() => await _coefficientRepository.AddAsync(coefficient));
        }

        public async Task<IEnumerable<Bet>> GetAllBetsWithDebtAndWithTheEnd()
        {
            var bets = new List<Bet>();
            var timeSpan = new TimeSpan(1, 28, 00);
            if (await _betRepository.CountAsync() != 0)
            {
                var betsDb = (List<Bet>)await _betRepository.GetAsync(b => b.Coefficients.Count >= 10);
                betsDb = betsDb.Where(b => b.BetTime >= timeSpan).ToList();
                foreach (var bet in betsDb)
                {
                    var isMade = bet.Coefficients.FirstOrDefault(c => c.IsMadeBet == true);
                    if (isMade != null)
                        bets.Add(bet);
                }
            }
            return bets;
        }

        public async Task<IEnumerable<Bet>> GetAllBetsWithTheEnd()
        {
            var bets = new List<Bet>();
            if (await _betRepository.CountAsync() != 0)
            {
                var betsDb = (List<Bet>)await _betRepository.GetAsync(b => b.Coefficients.Count >= 10);
                bets = betsDb.Where(b => b.BetTime >= new TimeSpan(1, 24, 00)).ToList();
            }
            return bets;
        }

        private bool ComparerBet (Bet entityNew, Bet entityDb)
        {
            return entityNew.AuxiliaryLocator == entityDb.AuxiliaryLocator && 
                entityNew.Name == entityDb.Name &&
                entityNew.BetTime == entityDb.BetTime &&
                entityNew.Score == entityDb.Score;
        }
        private bool ComparerCoefficient (Coefficient entityNew, Coefficient entityDb)
        {
            return entityNew.RatioFirst == entityDb.RatioFirst &&
                entityNew.RatioSecond == entityDb.RatioSecond &&
                entityNew.RatioThird == entityDb.RatioThird &&
                entityNew.Score == entityDb.Score;
        }

        public async Task<IEnumerable<Bet>> GetBetsWithTheEndByDate(DateTime startDate, DateTime endDate)
        {
            endDate = endDate.AddDays(1);
            var bets = await GetAllBetsWithTheEnd();
            var betsByDate = bets.Where(b => b.CreateDate >= startDate && b.CreateDate < endDate);
            return betsByDate;
        }

        public async Task<IEnumerable<Bet>> GetBetsWithDebtAndWithTheEndByDate(DateTime startDate, DateTime endDate)
        {
            endDate = endDate.AddDays(1);
            var bets = await GetAllBetsWithDebtAndWithTheEnd();
            var betsByDate = bets.Where(b => b.CreateDate >= startDate && b.CreateDate < endDate);
            return betsByDate;
        }

        public async Task<IReadOnlyList<DateTime>> GetEnabledDatesPickBets()
        {
            var result = new List<DateTime>();
            var bets = await _betRepository.GetAllAsync();
            var betsDates = bets.Select(b => b.CreateDate).ToList();
            foreach(var betsDate in betsDates)
            {
                var year = betsDate.Year;
                var monuth = betsDate.Month;
                var day = betsDate.Day;
                result.Add(new DateTime(year, monuth, day));
            }
            return result.Distinct().ToList();
        }
    }
}
