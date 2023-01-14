using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF.Interfaces
{
    public interface ISimulateService
    {
        IAsyncEnumerable<string> Run(double sum, double debt, bool isOptionalSimulate, double minRate, double maxRate, bool isBetToWinner);
    }
}
