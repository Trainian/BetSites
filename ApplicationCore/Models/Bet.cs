using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Models
{
    public class Bet : BaseObject
    {
        public string Name { get; set; } = "";
        public string AuxiliaryLocator { get; set; } = "";
        public TimeSpan BetTime { get; set; }
        public string Score { get; set; } = "0:0";
        public DateTime CreateDate { get; set; } = DateTime.Now;


        public virtual ICollection<Coefficient> Coefficients { get; set; } = new ObservableCollection<Coefficient>();
    }
}
