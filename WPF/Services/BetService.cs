using ApplicationCore.Models;
using ApplicationCore.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
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
            // Находим Ставку с таким же названием и текущей датой
            var bets = await _betRepository.GetAsync(bet => 
                bet.Name == entity.Name && 
                bet.CreateDate.Date == entity.CreateDate.Date);            
            var bet = bets.FirstOrDefault();

            // Если новая ставка, создаем и возвращаем
            if (bet == null)
                return await CreateBetAsync(entity);

            // Если в ставке были изминения, обновляем их
            if (!ComparerBet(entity,bet))
                bet = await UpdateBetAsync(entity, bet);

            // Проверяем коэффициенты, если изменились, добавляем
            if (!ComparerCoefficient(entity.Coefficients.Last(), bet.Coefficients.Last()))
                await AddCoefficientToBet(bet, entity.Coefficients.Last());

            return bet;
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
            await App.Current.Dispatcher.Invoke(async() => await _coefficientRepository.AddAsync(coefficient).ConfigureAwait(false)).ConfigureAwait(false);
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
                entityNew.RatioThird == entityDb.RatioThird;
        }
    }
}
