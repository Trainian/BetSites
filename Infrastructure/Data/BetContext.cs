using ApplicationCore.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class BetContext : DbContext
    {
        public BetContext(DbContextOptions<BetContext> options): base (options)
        {
            try
            {
                //Database.EnsureDeleted();
                Database.EnsureCreated();
            }
            catch (Exception e)
            {
            }
        }

        public DbSet<Bet> Bets { get; set; }
        public DbSet<Coefficient> Coefficients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var e = modelBuilder.Entity<Bet>();
            if(modelBuilder.Entity<Bet>() == null)
            {
                modelBuilder.Entity<Coefficient>().HasData(GetCoefficients());
                modelBuilder.Entity<Bet>().HasData(GetBets());
            }
            base.OnModelCreating(modelBuilder);
        }

        private List<Bet> GetBets()
        {
            var bets = new List<Bet>()
            {
                new Bet()
                {
                    Id = 1,
                    AuxiliaryLocator = "asd",
                    Name = "Test",
                    BetTime = TimeSpan.FromMinutes(10),
                    Score = "0 - 1",
                    CreateDate = DateTime.Now
                }
            };
            return bets;
        }
        private List<Coefficient> GetCoefficients()
        {
            var coefs = new List<Coefficient>()
            {
                new Coefficient()
                {
                    Id = 1,
                    BetId = 1,
                    RatioFirst = 1.0,
                    RatioSecond = 2.1,
                    RatioThird = 3.2,
                    BetTime = TimeSpan.FromMinutes(1),
                    Score = "0 - 0",
                    Time = DateTime.Now - new TimeSpan(0,10,30)
                },
                new Coefficient()
                {
                    Id = 2,
                    BetId = 1,
                    RatioFirst = 0.8,
                    RatioSecond = 2.1,
                    RatioThird = 3.5,
                    BetTime = TimeSpan.FromMinutes(10),
                    Score = "0 - 1",
                    Time = DateTime.Now
                }
            };
            return coefs;
        }
    }
}
