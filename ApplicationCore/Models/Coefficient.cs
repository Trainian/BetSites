using System;
using System.Collections.Generic;

namespace ApplicationCore.Models
{
    public class Coefficient : BaseObject
    {
        public DateTime Time { get; set; }
        public double Ratio { get; set; }
        public Dictionary<DateTime, int> History { get; set; } = new Dictionary<DateTime, int>();
        public int BetId { get; set; }


        public Bet Bet { get; set; }
    }
}