using System.Collections.Generic;
using System.Windows.Controls;
using MeltCalc.Helpers;
using MeltCalc.Model;

namespace MeltCalc.ViewModel
{
	public class AlloyAndDensingPresenter
	{
		private const string TableName = "FerroAlloys";
		private const int FerroAlloysShift = 2;

		private readonly ComboBox _alloys;
		private readonly LooseMdb _looseMdb = new LooseMdb();
		private readonly List<TextBox> _values;

		public AlloyAndDensingPresenter(ComboBox alloys, List<TextBox> values)
		{
			_values = values;
			_alloys = alloys;
			_alloys.SelectionChanged += IndexChanged;

			NamesLoad();
		}

		private void IndexChanged(object sender, SelectionChangedEventArgs e)
		{
			_looseMdb.FillBoxes(TableName, _alloys.SelectedIndex, _values, FerroAlloysShift);
		}

		private void NamesLoad()
		{
			var items = _looseMdb.Reader.SelectColumnRange<string>(TableName, "Наименование");
			foreach (var item in items)
			{
				_alloys.Items.Add(item);
			}
			_alloys.SelectedIndex = 0;
		}
	}
}