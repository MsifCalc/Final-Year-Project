using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MeltCalc.Chemistry;
using MeltCalc.Helpers;
using MeltCalc.ViewModel;

namespace MeltCalc.Pages
{
	/// <summary>
	/// Interaction logic for AlloyAndDensingInput.xaml
	/// </summary>
	public partial class AlloyAndDensingInput
	{
		private AlloyAndDensingPresenter _presenter;

		public AlloyAndDensingInput()
		{
			InitializeComponent();
			Loaded += OnLoad;
		}

		private void OnLoad(object sender, RoutedEventArgs e)
		{
			var values = _values.FindVisualChild<TextBox>().ToList();
			_presenter = new AlloyAndDensingPresenter(_alloys, values);

			SelectAlloyOrDensing();
		}

		private void SelectAlloyOrDensing()
		{
			var tab1 = (TabItem) _tabs.Items[0];
			var tab2 = (TabItem) _tabs.Items[1];

			tab1.IsEnabled = AdaptationData.AlloyUse;
			tab2.IsEnabled = AdaptationData.DensingUse;

			tab1.IsSelected = tab1.IsEnabled;
			tab2.IsSelected = tab2.IsEnabled;

			_alloyWeight.Text = String.Format("{0:0.000}", Tube.Ферросплав.G * 1000.0);
			_densingWeight.Text = String.Format("{0:0.000}", AdaptationData.GDensing * 1000.0);
		}

		private void NextExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			if (AssingAlloy() && NavigationService != null)
			{
				NavigationService.Navigate(new Step15());
			}
		}

		private bool AssingAlloy()
		{
			double galloy;
			var result = double.TryParse(_alloyWeight.Text, out galloy);

			if (result && Ферросплав.Validate(galloy))
			{
				Tube.Ферросплав.G = galloy;
				return true;
			}

			MessageBox.Show("Проверьте правильность введенных данных!", "Ошибка данных",
			                MessageBoxButton.OK, MessageBoxImage.Error);
			return false;
		}

		private void NextCanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}
	}
}