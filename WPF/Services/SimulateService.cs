using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Interfaces;

namespace WPF.Services
{
    public class SimulateService : ISimulateService
    {
        private IBetService _betService;

        public SimulateService(IBetService betService)
        {
            _betService = betService;
        }
        public async IAsyncEnumerable<string> Run(double sum, double debt)
        {
            var sumNow = sum;
            var bets = await _betService.GetAllBetAsync();
            bets = bets.Where(b => b.Coefficients.Last().Score != "0:0");

            while (bets.Count() != 0)
            {
                foreach (var bet in bets)
                {

                }
            }
            yield break;
        }
    }
}
