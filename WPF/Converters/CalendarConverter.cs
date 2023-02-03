using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WPF.Converters
{
    public class CalendarConverter : IValueConverter
    {
        public static List<DateTime> dict = new List<DateTime>();

        static CalendarConverter()
        {

        }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string text = null;
            for (int i = 0; i < dict.Count; i++)
            {
                if (dict[i] == (DateTime)value)
                {
                    text = "HaveHistory";
                }
            }

            return text;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        public static void Update(List<DateTime> MIList)
        {
            dict.Clear();
            dict = MIList;
        }
    }
}
