using System;

namespace MeltCalc.Helpers
{
	public static class StringExtensions
	{
		public static int ToInt(this string value)
		{
			int outValue;
			if (string.IsNullOrWhiteSpace(value) || !int.TryParse(value, out outValue))
			{
				throw new ArgumentException(value, string.Format("value = '{0}'", value));
			}
			return outValue;
		}

		public static double ToDouble(this string value)
		{
			double outValue;
			if (string.IsNullOrWhiteSpace(value) || !double.TryParse(value, out outValue))
			{
				throw new ArgumentException(value, string.Format("value = '{0}'", value));
			}
			return outValue;
		}

		public static double ToDoubleOrDefault(this string value)
		{
			double outValue;
			if (string.IsNullOrWhiteSpace(value) || !double.TryParse(value, out outValue))
			{
				return 0.0d;
			}
			return outValue;
		}

		public static float ToFloat(this string value)
		{
			float outValue;
			if (string.IsNullOrWhiteSpace(value) || !float.TryParse(value, out outValue))
			{
				throw new ArgumentException(value, string.Format("value = '{0}'", value));
			}
			return outValue;
		}

		public static int ToIntOrZero(this string value)
		{
			if (string.IsNullOrWhiteSpace(value)) return 0;
			int outValue;
			return int.TryParse(value, out outValue) ? outValue : 0;
		}

		public static float ToFloatOrZero(this string value)
		{
			if (string.IsNullOrWhiteSpace(value)) return 0;
			float outValue;
			return float.TryParse(value, out outValue) ? outValue : 0.0f;
		}

		public static double ToDoubleOrZero(this string value)
		{
			if (string.IsNullOrWhiteSpace(value)) return 0;
			double outValue;
			return double.TryParse(value, out outValue) ? outValue : 0.0d;
		}

		public static Tuple<bool, double> ToDoubleSafe(this string value)
		{
			if (string.IsNullOrWhiteSpace(value)) return new Tuple<bool, double>(false, -1.0d);
			double outValue;
			return
				double.TryParse(value, out outValue)
					? new Tuple<bool, double>(true, outValue)
					: new Tuple<bool, double>(false, -1.0d);
		}
	}
}