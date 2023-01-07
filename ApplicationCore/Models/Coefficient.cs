using System;
using System.Collections.Generic;

namespace ApplicationCore.Models
{
    public class Coefficient : BaseObject
    {
        public DateTime Time { get; set; }
        public double RatioFirst { get; set; }
        public double RatioSecond { get; set; }
        public double RatioThird { get; set; }
        public TimeSpan BetTime { get; set; }
        public string Score { get; set; }
        public bool IsMadeBet { get; set; }
        public int BetId { get; set; }


        public virtual Bet Bet { get; set; }
    }
}