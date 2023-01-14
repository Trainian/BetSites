using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF.Converters
{
    public static class ParsTimeToTimeSpan
    {
        public static TimeSpan ConvertToTimeSpan (this TimeSpan ts, string str = "00:00")
        {
            var separators = str.Split(":").Length;
            if (separators == 2)
                str = "00:" + str;

            var strTimes = str.Split(":").ToList();
            var newTs = TimeSpan.FromHours(Double.Parse(strTimes[0])) + TimeSpan.FromMinutes(Double.Parse(strTimes[1])) + TimeSpan.FromSeconds(Double.Parse(strTimes[2]));
            return newTs;
        }
    }
}
