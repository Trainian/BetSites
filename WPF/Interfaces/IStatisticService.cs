using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF.Interfaces
{
    public interface IStatisticService
    {
        Task<decimal> GetPercentWins();
        Task<decimal> GetPercentChangedWiners();
        Task<decimal> GetPercentWinsFavorits();
    }
}
