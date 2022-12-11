using ApplicationCore.Models;
using ApplicationCore.Repositories;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class BetRepository : Repository<Bet>, IBetRepository
    {
        public BetRepository(BetContext context) : base(context)
        {
        }
    }
}
