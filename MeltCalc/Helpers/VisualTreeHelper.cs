using System.Collections.Generic;
using System.Windows;

namespace MeltCalc.Helpers
{
	public static class VisualTreeHelper
	{
		public static IEnumerable<TItem> FindVisualChild<TItem>(this DependencyObject obj) where TItem : DependencyObject
		{
			for (var i = 0; i < System.Windows.Media.VisualTreeHelper.GetChildrenCount(obj); i++)
			{
				var child = System.Windows.Media.VisualTreeHelper.GetChild(obj, i);
				if (child is TItem)
				{
					yield return (TItem) child;
				}

				foreach (var item in FindVisualChild<TItem>(child))
				{
					yield return item;
				}
			}
		}
	}
}