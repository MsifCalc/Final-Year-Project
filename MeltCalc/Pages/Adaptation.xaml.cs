using System.Windows.Input;
using MeltCalc.Chemistry;
using MeltCalc.Helpers;
using MeltCalc.ViewModel;

namespace MeltCalc.Pages
{
	/// <summary>
	/// Interaction logic for Adaptation.xaml
	/// </summary>
	public partial class Adaptation
	{
		private readonly AdaptationModel _model = new AdaptationModel();

		public Adaptation()
		{
			InitializeComponent();
			Loaded += OnLoad;
		}

		private void OnLoad(object sender, System.Windows.RoutedEventArgs e)
		{
			InitC();
			InitSi();
			InitMn();
			InitP();
			InitS();
			InitSypuch();
		}

		private void InitSypuch()
		{
			_sypuchType.Items.Add(1);
			_sypuchType.Items.Add(2);
			_sypuchType.Items.Add(3);

			_sypuchType.SelectedIndex = 1;
		}

		private void InitS()
		{
			_chkS.Items.Add(0.005);
			_chkS.Items.Add(0.010);
			_chkS.Items.Add(0.015);
			_chkS.Items.Add(0.020);
			_chkS.Items.Add(0.025);
			_chkS.Items.Add(0.030);
			_chkS.Items.Add(0.035);

			_chkS.SelectedIndex = 3;
		}

		private void InitP()
		{
			_chkP.Items.Add(0.005);
			_chkP.Items.Add(0.010);
			_chkP.Items.Add(0.015);
			_chkP.Items.Add(0.020);
			_chkP.Items.Add(0.025);
			_chkP.Items.Add(0.030);
			_chkP.Items.Add(0.035);

			_chkP.SelectedIndex = 4;
		}

		private void InitMn()
		{
			_chkMn.Items.Add(0.01);
			_chkMn.Items.Add(0.05);
			_chkMn.Items.Add(0.1);
			_chkMn.Items.Add(0.15);
			_chkMn.Items.Add(0.2);
			_chkMn.Items.Add(0.25);
			_chkMn.Items.Add(0.3);

			_chkMn.SelectedIndex = 4;
		}

		private void InitSi()
		{
			_chkSi.Items.Add(0.005);
			_chkSi.Items.Add(0.01);
			_chkSi.Items.Add(0.015);
			_chkSi.Items.Add(0.02);
			_chkSi.Items.Add(0.025);
			_chkSi.Items.Add(0.03);
			_chkSi.Items.Add(0.04);

			_chkSi.SelectedIndex = 3;
		}

		private void InitC()
		{
			_chkC.Items.Add(0.04);
			_chkC.Items.Add(0.06);
			_chkC.Items.Add(0.08);
			_chkC.Items.Add(0.1);
			_chkC.Items.Add(0.15);
			_chkC.Items.Add(0.2);
			_chkC.Items.Add(0.25);

			_chkC.SelectedIndex = 5;
		}

		private void NextCanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		private void NextExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			Tube.Лом.C = _chkC.SelectedValue.ToString().ToFloat();
			Tube.Лом.Mn = _chkMn.SelectedValue.ToString().ToFloat();
			Tube.Лом.P = _chkP.SelectedValue.ToString().ToFloat();
			Tube.Лом.S = _chkS.SelectedValue.ToString().ToFloat();
			Tube.Лом.Si = _chkSi.SelectedValue.ToString().ToFloat();

			var fixedMass = _adaptFixedMass.IsChecked.HasValue && _adaptFixedMass.IsChecked.Value;

			_model.Run(SypuchType, fixedMass);

			// TODO: Step 11.
		}

		private int SypuchType
		{
			get { return _sypuchType.SelectedValue.ToString().ToInt() - 1; }
		}

		private void PrevCanPrevious(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		private void PrevExecuted(object sender, ExecutedRoutedEventArgs e)
		{
		}
	}
}
