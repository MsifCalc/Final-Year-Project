using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MeltCalc.Chemistry;
using MeltCalc.Helpers;
using MeltCalc.Model;

namespace MeltCalc.Pages
{
	/// <summary>
	/// Interaction logic for Step12.xaml
	/// </summary>
	public partial class Step12
	{
		private List<CheckBox> _checkBoxes;

		public Step12()
		{
			InitializeComponent();
			Loaded +=OnLoaded;
		}

		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			_checkBoxes = _grid.FindVisualChild<CheckBox>().ToList();

			if (Params.IsDuplex)
			{
				_checkBoxes.Single(x => x.Material() == Materials.Песок).IsEnabled = false;
				_checkBoxes.Single(x => x.Material() == Materials.Окатыши).IsChecked = true;
				_checkBoxes.Single(x => x.Material() == Materials.Окатыши).IsEnabled = false;
				_checkBoxes.Single(x => x.Material() == Materials.Окалина).IsChecked = true;
				_checkBoxes.Single(x => x.Material() == Materials.Окалина).IsEnabled = false;
			}
			else
			{
				_checkBoxes.Single(x => x.Material() == Materials.Песок).IsEnabled = true;
				_checkBoxes.Single(x => x.Material() == Materials.Окатыши).IsChecked = false;
				_checkBoxes.Single(x => x.Material() == Materials.Окатыши).IsEnabled = true;
				_checkBoxes.Single(x => x.Material() == Materials.Окалина).IsChecked = false;
				_checkBoxes.Single(x => x.Material() == Materials.Окалина).IsEnabled = true;				
			}
		}

		private void NextCanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		private void NextExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			var checkBoxs = _grid
				.FindVisualChild<CheckBox>()
				.Where(x=>x.IsChecked.HasValue && x.IsChecked.Value)
				.Select(x=>x.Tag).OfType<Materials>().ToList();

			if (NavigationService != null)
			{
				NavigationService.Navigate(new Step13(checkBoxs));
			}
		}

		private void PrevCanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		private void PrevExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			if (NavigationService != null)
			{
				NavigationService.Navigate(new Uri(@"Pages\Step11.xaml", UriKind.Relative));
			}
		}
	}
}
