using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IBaseParser
    {
        Task Run(CancellationToken token, int scrolls = 0);
    }
}
