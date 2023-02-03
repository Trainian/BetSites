using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WPF.Interfaces
{
    public interface IBetService
    {
        Task<Bet> AddBetAsync(Bet entity);
        Task<Bet> CreateBetAsync(Bet entity);
        Task<Bet> UpdateBetAsync(Bet entityOld, Bet entityDb);
        Task<Bet?> GetBetByIdAsync(int id);
        Task<Bet?> GetBetByName(string name);
        Task<IEnumerable<Bet>?> GetAllBetAsync();
        Task<IEnumerable<Bet>> GetAllBetsWithTheEnd();
        Task<IEnumerable<Bet>> GetAllBetsWithDebtAndWithTheEnd();
        Task<IEnumerable<Bet>> GetBetsWithTheEndByDate(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Bet>> GetBetsWithDebtAndWithTheEndByDate(DateTime startDate, DateTime endDate);
        Task<IReadOnlyList<DateTime>> GetEnabledDatesPickBets();
    }
}
