using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF.Interfaces
{
    public interface ICoefficientService
    {
        Task<Coefficient> GetWinnerCoefficientByBetId(int betId);
        Task<Coefficient?> GetIsMadeBetCoefficientByBetId(int betId);
        Task<Coefficient?> GetOptionalCoefficientByBetId(int betId, decimal minRate, decimal maxRate, bool isBetToWinner);
    }
}
