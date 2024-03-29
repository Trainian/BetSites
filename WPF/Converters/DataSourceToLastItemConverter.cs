﻿using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WPF.Converters
{
    public class DataSourceToLastItemConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IEnumerable<Coefficient> items = value as IEnumerable<Coefficient>;
            if (items.Count() != 0)
            {
                switch (parameter)
                {
                    case "RatioFirst":
                        return items.LastOrDefault().RatioFirst;
                    case "RatioSecond":
                        return items.LastOrDefault().RatioSecond;
                    case "RatioThird":
                        return items.LastOrDefault().RatioThird;
                    default: throw new ArgumentException("Не верный аргумент параметра");
                }
            }
            else return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
