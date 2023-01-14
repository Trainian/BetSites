using ApplicationCore.Models;
using ApplicationCore.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class BetRepository : Repository<Bet>, IBetRepository
    {
        private readonly BetContext _context;
        public BetRepository(BetContext context) : base(context)
        {
            _context = context;
        }

    }
}
