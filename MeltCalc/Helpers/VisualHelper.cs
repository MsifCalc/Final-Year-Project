using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MeltCalc.Converters;
using MeltCalc.Model;
using Xceed.Wpf.Toolkit;

namespace MeltCalc.Helpers
{
	public static class VisualHelper
	{
		public static double GetDoubleValue(this ComboBox comboBox)
		{
			var converter = new StringToDoubleConverter();
			return (double) converter.ConvertBack(comboBox.SelectedValue, typeof (double), null, CultureInfo.InvariantCulture);
		}

		public static double GetDoubleValue(this TextBox comboBox)
		{
			var converter = new StringToDoubleConverter();
			return (double)converter.ConvertBack(comboBox.Text, typeof(double), null, CultureInfo.InvariantCulture);
		}

		public static double GetDoubleValue(this DoubleUpDown comboBox)
		{
			var converter = new StringToDoubleConverter();
			return (double)converter.ConvertBack(comboBox.Text, typeof(double), null, CultureInfo.InvariantCulture);
		}

		public static T FindButton<T>(IEnumerable<T> coll, Materials material) where T : ContentControl
		{
			return coll.Single(x => x.Tag is Materials && (Materials) x.Tag == material);
		}

		public static Materials Material(this ContentControl button)
		{
			return (Materials) button.Tag;
		}

		public static ContentControl FindByMaterial(this IEnumerable<ContentControl> coll, Materials materials)
		{
			return coll.Single(x => x.Material() == materials);
		}

		public static void UpdateEnabled(IEnumerable<ContentControl> controls, ICollection<Materials> disabledList)
		{
			foreach (var button in controls.Where(control => disabledList.Contains(control.Material())))
			{
				button.IsEnabled = false;
			}
		}

		public static void UpdateVisibility(IEnumerable<ContentControl> controls, ICollection<Materials> visibleList)
		{
			foreach (var control in controls.Where(control => !visibleList.Contains(control.Material())))
			{
				control.Visibility = Visibility.Collapsed;
			}
		}
	}
}