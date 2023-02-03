using System;
using System.Collections.Generic;

namespace ApplicationCore.Models
{
    public class Coefficient : BaseObject
    {
        public DateTime Time { get; set; }
        public decimal RatioFirst { get; set; }
        public decimal RatioSecond { get; set; }
        public decimal RatioThird { get; set; }
        public TimeSpan BetTime { get; set; }
        public string Score { get; set; } = "0:0";
        public bool IsMadeBet { get; set; }
        public int BetId { get; set; }


        public virtual Bet Bet { get; set; }
    }
}