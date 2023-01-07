using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
