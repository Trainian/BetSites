using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF.Interfaces
{
    public interface ISimulateService
    {
        IAsyncEnumerable<string> FindOptimalSettings(decimal sum, decimal debt, DateTime startDate, DateTime endDate, decimal minRate, decimal maxRate, bool isBetToWinner);
        IAsyncEnumerable<string> Run(decimal sum, decimal debt, DateTime startDate, DateTime endDate, bool isOptionalSimulate, decimal minRate, decimal maxRate, bool isBetToWinner);
    }
}
