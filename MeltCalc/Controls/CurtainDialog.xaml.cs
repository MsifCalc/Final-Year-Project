using System.Windows;

namespace MeltCalc.Controls
{
	/// <summary>
	/// Interaction logic for CurtainDialog.xaml
	/// </summary>
	public partial class CurtainDialog
	{
		public static readonly DependencyProperty MessageProperty =
			DependencyProperty.Register("Message", typeof(string), typeof(CurtainDialog),
				new UIPropertyMetadata(string.Empty));

		private bool _hideRequest;
		private bool _result;

		public CurtainDialog()
		{
			InitializeComponent();
			Visibility = Visibility.Collapsed;
		}

		public string Message
		{
			get { return (string) GetValue(MessageProperty); }
			set { SetValue(MessageProperty, value); }
		}

		public bool ShowHandlerDialog(string message)
		{
			Message = message;
			Visibility = Visibility.Visible;
			return _result;
		}

		private void HideHandlerDialog()
		{
			_hideRequest = true;
			Visibility = Visibility.Collapsed;
		}

		private void OkButton_Click(object sender, RoutedEventArgs e)
		{
			_result = true;
			HideHandlerDialog();
		}

		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			_result = false;
			HideHandlerDialog();
		}
	}
}