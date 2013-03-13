using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MeltCalc.Helpers;
using MeltCalc.Model;

namespace MeltCalc.Pages
{
	/// <summary>
	/// Шаг3: Выбор рассчитываемого шлакообразующего
	/// </summary>
	public partial class Step13
	{
		private readonly List<Materials> _disabled = new List<Materials>
		                                             	{
		                                             		Materials.Кокс,
		                                             		Materials.Песок,
		                                             		Materials.Окатыши,
		                                             		Materials.Руда,
		                                             		Materials.Окалина,
		                                             		Materials.Агломерат,
		                                             		Materials.ПлавиковыйШпат
		                                             	};

		private readonly List<Materials> _selectedMaterials;
		private List<RadioButton> _radioButtons;

		public Step13()
		{
			InitializeComponent();
		}

		public Step13(List<Materials> selectedMaterials)
		{
			InitializeComponent();
			_selectedMaterials = selectedMaterials;
			Loaded += OnLoaded;
		}

		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			Loaded -= OnLoaded;

			_radioButtons = _grid.FindVisualChild<RadioButton>().ToList();

			VisualHelper.UpdateVisibility(_radioButtons, _selectedMaterials);
			VisualHelper.UpdateEnabled(_radioButtons, _disabled);

			DefaultValues(_radioButtons);
		}

		private void DefaultValues(IEnumerable<RadioButton> buttons)
		{
			if (_selectedMaterials.Contains(Materials.Известь))
			{
				var checkBox = (RadioButton) buttons.FindByMaterial(Materials.Известь);
				checkBox.IsChecked = true;
			}
			else if (_selectedMaterials.Contains(Materials.Доломит))
			{
				var checkBox = (RadioButton) buttons.FindByMaterial(Materials.Доломит);
				checkBox.IsChecked = true;
			}
		}

		private void CommandBindingCanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		private void CommandBindingExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			var radioButton = _radioButtons.Single(x => x.IsChecked.HasValue && x.IsChecked.Value);

			if (NavigationService != null)
			{
				NavigationService.Navigate(new Step14(_selectedMaterials, radioButton.Material()));
			}
		}

		private void CommandBindingCanPrevious(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		private void CommandBindingPreviousPage(object sender, ExecutedRoutedEventArgs e)
		{
			if (NavigationService != null)
			{
				NavigationService.Navigate(new Uri(@"Pages\Step12.xaml", UriKind.Relative));
			}
		}
	}
}