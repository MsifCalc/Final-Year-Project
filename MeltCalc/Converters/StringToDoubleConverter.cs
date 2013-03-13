using System;
using System.Globalization;
using System.Windows.Data;

namespace MeltCalc.Converters
{
	public class StringToDoubleConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value == null ? string.Empty : value.ToString();
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is double) return value;
			if (value is float) return value;
			if (value is int) return ConvertBack(value, targetType);
			if (value is string) return ConvetrFromString(value, targetType);
			return null;
		}

		private static object ConvertBack(object value, Type targetType)
		{
			if (targetType == typeof(int))
			{
				return System.Convert.ToInt32(value);
			}

			if (targetType == typeof(Single))
			{
				return System.Convert.ToSingle(value);
			}

			return System.Convert.ToDouble(value);
		}

		private static object ConvetrFromString(object value, Type targetType)
		{
			return string.IsNullOrEmpty(value as string) ? 0.0 : ConvertBack(value, targetType);
		}
	}
}