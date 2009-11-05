﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Data;
using System.Globalization;

namespace Rolodex
{
  public class CurrencyConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      if (value != null && value.ToString() != String.Empty)
      {
        decimal price = (decimal)value;
        return price.ToString("C");
      }
      else
        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      string price = value.ToString();

      decimal result;
      if (Decimal.TryParse(price, NumberStyles.Any, null, out result))
      {
        return result;
      }
      return 0;
    }
  }
}
