using System;
using System.Windows.Input;

namespace MeltCalc.Pages
{
	/// <summary>
	/// Interaction logic for Step1.xaml
	/// </summary>
	public partial class Step1
	{
		public Step1()
		{
			InitializeComponent();
		}

		private void NextCanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			if (_option1.IsChecked != null) e.CanExecute = (bool) _option1.IsChecked;
		}

		private void NextExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			if (NavigationService != null && _option1.IsChecked.HasValue && _option1.IsChecked.Value)
				NavigationService.Navigate(new Uri(@"Pages\Step11.xaml", UriKind.Relative));
		}
	}
}
