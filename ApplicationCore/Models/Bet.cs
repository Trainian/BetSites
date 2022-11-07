using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Models
{
    public class Bet : BaseObject
    {
        public string Name { get; set; }
        public string AuxiliaryLocator { get; set; }
        public IEnumerable<Coefficient> Coefficients { get; set; }
    }
}
