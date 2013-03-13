using System.Collections.Generic;

namespace MeltCalc.Helpers
{
	public static class ArrayExtension
	{
		public static IEnumerable<T> RangeInclusive<T>(this T[] source, int from, int to)
		{
			if (to > source.Length - 1)
			{
				to = source.Length - 1;
			}

			for (var idx = from; idx <= to; idx++)
			{
				yield return source[idx];
			}
		}
	}
}