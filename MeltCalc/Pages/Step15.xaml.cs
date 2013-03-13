using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using MeltCalc.Chemistry;
using MeltCalc.Controls;
using MeltCalc.Converters;
using MeltCalc.Helpers;
using MeltCalc.Model;

namespace MeltCalc.Pages
{
	/// <summary>
	/// Interaction logic for Step15.xaml
	/// </summary>
	public partial class Step15
	{
		private const string StAndChugLists = "StAndChugLists";
		private readonly StringToDoubleConverter _converter = new StringToDoubleConverter();
		private readonly ParamsMdb _paramsMdb = new ParamsMdb();
		private const double Epsilon = 0.00001;
		private double _stStep;

		public Step15()
		{
			InitializeComponent();
			Loaded += OnLoaded;
		}

		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			// TODO:
			// If ROUND = 0 Then cmdLastEstimate.Enabled = False Else cmdLastEstimate.Enabled = True

			InitializeFromGlobals();
			LomChem_Load();
			LomChemTotalCount();
			Lists_Load();
			InitProstoi();
		}

		private void GoToResults()
		{
			if (NavigationService != null)
			{
				NavigationService.Navigate(new Uri(@"Pages\Results.xaml", UriKind.Relative));
			}
		}

		private void InitProstoi()
		{
			InitCombobox(_prostoi, Enumerable.Range((int) (Params.TAUprost - 2), 54));
			_prostoi.SelectedIndex = 2;
			InitCombobox(_sliv, Enumerable.Range(2, 7));
			_sliv.SelectedIndex = 4;
		}

		private void InitializeFromGlobals()
		{
			_steelMass.Value = Tube.Сталь.GYield;

			if (Params.IsDuplex)
			{
				_shlak.IsEnabled = false;
				_chkLeftShlak.IsChecked = false;
			}

			if (Params.InputForm == "auto")
			{
				_steelMass.IsEnabled = true;
				_steelMass.AllowSpin = true;
				// TODO: Использовать step.
				_stStep = Tube.Сталь.GYieldmemo / 100.0;
			}
			else
			{
				_steelMass.IsEnabled = false;
				_steelMass.AllowSpin = false;
				_stStep = 0.0;
			}
		}

		private void LomChem_Load()
		{
			_lowSmall.Text = Params.ЛомМелкий.НизкоУглеродный.Part.ToString(CultureInfo.InvariantCulture);
			_lowMiddle.Text = Params.ЛомСредний.НизкоУглеродный.Part.ToString(CultureInfo.InvariantCulture);
			_lowLarge.Text = Params.ЛомКрупный.НизкоУглеродный.Part.ToString(CultureInfo.InvariantCulture);

			_midSmall.Text = Params.ЛомМелкий.СреднеУглеродный.Part.ToString(CultureInfo.InvariantCulture);
			_midMiddle.Text = Params.ЛомСредний.СреднеУглеродный.Part.ToString(CultureInfo.InvariantCulture);
			_midLarge.Text = Params.ЛомКрупный.СреднеУглеродный.Part.ToString(CultureInfo.InvariantCulture);

			_highSmall.Text = Params.ЛомМелкий.ВысокоУглеродный.Part.ToString(CultureInfo.InvariantCulture);
			_highMiddle.Text = Params.ЛомСредний.ВысокоУглеродный.Part.ToString(CultureInfo.InvariantCulture);
			_highLarge.Text = Params.ЛомКрупный.ВысокоУглеродный.Part.ToString(CultureInfo.InvariantCulture);
		}

		private void LomChemTotalCount()
		{
			Tube.Лом.C =
				Params.ЛомМелкий.НизкоУглеродный.C		* ToFloat(_lowSmall.Text) +
				Params.ЛомСредний.НизкоУглеродный.C		* ToFloat(_lowMiddle.Text) +
				Params.ЛомКрупный.НизкоУглеродный.C		* ToFloat(_lowLarge.Text) +
				Params.ЛомМелкий.СреднеУглеродный.C		* ToFloat(_midSmall.Text) +
				Params.ЛомСредний.СреднеУглеродный.C	* ToFloat(_midMiddle.Text) +
				Params.ЛомКрупный.СреднеУглеродный.C	* ToFloat(_midLarge.Text) +
				Params.ЛомМелкий.ВысокоУглеродный.C		* ToFloat(_highSmall.Text) +
				Params.ЛомСредний.ВысокоУглеродный.C	* ToFloat(_highMiddle.Text) +
				Params.ЛомКрупный.ВысокоУглеродный.C	* ToFloat(_highLarge.Text);

			Tube.Лом.Si =
				Params.ЛомМелкий.НизкоУглеродный.Si		* ToFloat(_lowSmall.Text) +
				Params.ЛомСредний.НизкоУглеродный.Si	* ToFloat(_lowMiddle.Text) +
				Params.ЛомКрупный.НизкоУглеродный.Si	* ToFloat(_lowLarge.Text) +
				Params.ЛомМелкий.СреднеУглеродный.Si	* ToFloat(_midSmall.Text) +
				Params.ЛомСредний.СреднеУглеродный.Si	* ToFloat(_midMiddle.Text) +
				Params.ЛомКрупный.СреднеУглеродный.Si	* ToFloat(_midLarge.Text) +
				Params.ЛомМелкий.ВысокоУглеродный.Si	* ToFloat(_highSmall.Text) +
				Params.ЛомСредний.ВысокоУглеродный.Si	* ToFloat(_highMiddle.Text) +
				Params.ЛомКрупный.ВысокоУглеродный.Si	* ToFloat(_highLarge.Text);

			Tube.Лом.Mn =
				Params.ЛомМелкий.НизкоУглеродный.Mn		* ToFloat(_lowSmall.Text) +
				Params.ЛомСредний.НизкоУглеродный.Mn	* ToFloat(_lowMiddle.Text) +
				Params.ЛомКрупный.НизкоУглеродный.Mn	* ToFloat(_lowLarge.Text) +
				Params.ЛомМелкий.СреднеУглеродный.Mn	* ToFloat(_midSmall.Text) +
				Params.ЛомСредний.СреднеУглеродный.Mn	* ToFloat(_midMiddle.Text) +
				Params.ЛомКрупный.СреднеУглеродный.Mn	* ToFloat(_midLarge.Text) +
				Params.ЛомМелкий.ВысокоУглеродный.Mn	* ToFloat(_highSmall.Text) +
				Params.ЛомСредний.ВысокоУглеродный.Mn	* ToFloat(_highMiddle.Text) +
				Params.ЛомКрупный.ВысокоУглеродный.Mn	* ToFloat(_highLarge.Text);

			Tube.Лом.P =
				Params.ЛомМелкий.НизкоУглеродный.P		* ToFloat(_lowSmall.Text) +
				Params.ЛомСредний.НизкоУглеродный.P		* ToFloat(_lowMiddle.Text) +
				Params.ЛомКрупный.НизкоУглеродный.P		* ToFloat(_lowLarge.Text) +
				Params.ЛомМелкий.СреднеУглеродный.P		* ToFloat(_midSmall.Text) +
				Params.ЛомСредний.СреднеУглеродный.P	* ToFloat(_midMiddle.Text) +
				Params.ЛомКрупный.СреднеУглеродный.P	* ToFloat(_midLarge.Text) +
				Params.ЛомМелкий.ВысокоУглеродный.P		* ToFloat(_highSmall.Text) +
				Params.ЛомСредний.ВысокоУглеродный.P	* ToFloat(_highMiddle.Text) +
				Params.ЛомКрупный.ВысокоУглеродный.P	* ToFloat(_highLarge.Text);

			Tube.Лом.S =
				Params.ЛомМелкий.НизкоУглеродный.S		* ToFloat(_lowSmall.Text) +
				Params.ЛомСредний.НизкоУглеродный.S		* ToFloat(_lowMiddle.Text) +
				Params.ЛомКрупный.НизкоУглеродный.S		* ToFloat(_lowLarge.Text) +
				Params.ЛомМелкий.СреднеУглеродный.S		* ToFloat(_midSmall.Text) +
				Params.ЛомСредний.СреднеУглеродный.S	* ToFloat(_midMiddle.Text) +
				Params.ЛомКрупный.СреднеУглеродный.S	* ToFloat(_midLarge.Text) +
				Params.ЛомМелкий.ВысокоУглеродный.S		* ToFloat(_highSmall.Text) +
				Params.ЛомСредний.ВысокоУглеродный.S	* ToFloat(_highMiddle.Text) +
				Params.ЛомКрупный.ВысокоУглеродный.S	* ToFloat(_highLarge.Text);

			Tube.Лом.C /= 100;
			Tube.Лом.S /= 100;
			Tube.Лом.P /= 100;
			Tube.Лом.Si /= 100;
			Tube.Лом.Mn /= 100;

			_lomC.Text = Tube.Лом.C .ToString(CultureInfo.InvariantCulture);
			_lomSi.Text = Tube.Лом.Si.ToString(CultureInfo.InvariantCulture);
			_lomMn.Text = Tube.Лом.Mn.ToString(CultureInfo.InvariantCulture);
			_lomP.Text = Tube.Лом.P.ToString(CultureInfo.InvariantCulture);
			_lomS.Text = Tube.Лом.S.ToString(CultureInfo.InvariantCulture);
		}

		private void Lists_Load()
		{
			InitCombobox(_chugTemp, Row.TempChug);
			InitCombobox(_chugC, Row.CChug);
			InitCombobox(_chugS, Row.SChug);
			InitCombobox(_chugSi, Row.SiChug);
			InitCombobox(_chugP, Row.PChug);
			InitCombobox(_chugMn, Row.MnChug);

			// Нижний ряд комбобоксов.
			InitCombobox(_steelTemp, Row.TempSteel);
			InitComboboxCSteel();
			InitCombobox(_steelP, Row.PSteel);
			InitCombobox(_bMin, Row.Bmin);
			InitCombobox(_bMax, Row.Bmax);
			InitCombobox(_gostshl, Row.Gostshl);
		}

		private void InitComboboxCSteel()
		{
			var boundaries = SelectBoundaries(Row.CSteel);
			var step = boundaries.Item3;

			for (var idx = boundaries.Item1; idx < boundaries.Item2; idx += step)
			{
				if (idx > 0.099 && idx < 0.11)
				{
					step *= 2.0f;
				}

				if (idx > 0.299 && idx < 0.311)
				{
					step *= 2.0f;
				}

				_steelC.Items.Add(string.Format("{0:0.###}", idx));
			}

			_steelC.SelectedIndex = Convert.ToInt32(boundaries.Item4);
		}

		private void InitCombobox<T>(Selector combobox, IEnumerable<T> values)
		{
			foreach (var value in values)
			{
				combobox.Items.Add(value);
			}

			combobox.SelectedIndex = 0;
		}

		private void InitCombobox(Selector chugTemp, Row row)
		{
			var boundaries = SelectBoundaries(row);

			for (var idx = boundaries.Item1; idx < boundaries.Item2; idx += boundaries.Item3)
			{
				chugTemp.Items.Add(string.Format("{0:0.###}", idx));
			}

			chugTemp.SelectedIndex = Convert.ToInt32(boundaries.Item4);
		}

		private Tuple<float, float, float, float> SelectBoundaries(Row rowIndex)
		{
			var tempParams = _paramsMdb.Reader
				.SelectRowRange(StAndChugLists, (int)rowIndex)
				.Select(x => x.ToFloatOrZero()).ToArray();

			return new Tuple<float, float, float, float>(tempParams[2], tempParams[3], tempParams[4], tempParams[5]);
		}

		private void NextExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			if (Math.Abs(
				_chugTemp.GetDoubleValue() *
				_chugC.GetDoubleValue() *
				_chugSi.GetDoubleValue() *
				_chugMn.GetDoubleValue() *
				_chugP.GetDoubleValue() *
				_steelTemp.GetDoubleValue() *
				_steelC.GetDoubleValue() *
				_steelP.GetDoubleValue() - 0) < Epsilon && string.IsNullOrWhiteSpace(_lomC.Text))
			{
				MessageBox.Show("Рассчёт невозможен", "Введенные данные неверны или неполны", MessageBoxButton.OK);
				return;
			}

			if (Params.BottomBlowUse)
			{
				AdaptationData.VArBlow = Params.BlowingTime *
				                         InputWindow.ReadDouble("Введите МИНУТНЫЙ РАСХОД ИНЕРТНОГО ГАЗА донной продувки, м3/мин");
			}
			else
			{
				AdaptationData.VArBlow = 0.0;
			}

			if (!_chkLeftShlak.IsChecked.HasValue || !_chkLeftShlak.IsChecked.Value)
			{
				Tube.ОставленныйШлак.G = 0;
			}

			// присвоение основным переменным значений
			Texts_To_Vars();

			if (!Params.IsDuplex)
			{
				SaveLastLomType();
			}

			SaveLastListIndexes();

			// Расчет доли легковесного лома
			if (!Params.IsDuplex)
			{
				Tube.Лом.DolyaLegkovesa =
					(float) ((_highSmall.GetDoubleValue() +
					          _midSmall.GetDoubleValue() +
					          _lowSmall.GetDoubleValue() +
					          (_highMiddle.GetDoubleValue() +
					           _midMiddle.GetDoubleValue() +
					           _lowMiddle.GetDoubleValue()) * 0.5) / 100.0);
			}


			var longOperation = new ProgressWindow();
			longOperation.Run(new Estimation());

			GoToResults();

			// TODO: Реализация функций и расчет лома в левом вехнем углу.
		}

		private void SaveLastListIndexes()
		{
			// TODO
		}

		private void SaveLastLomType()
		{
			// TODO
		}

		private void Texts_To_Vars()
		{
			Params.TAUprostREAL = ToFloat(_prostoi.Text);
			Params.TAPtime = ToFloat(_sliv.Text);

			Tube.Чугун.T = ToDouble(_chugTemp.Text);
			Tube.Чугун.C = ToDouble(_chugC.Text);
			Tube.Чугун.Mn = ToDouble(_chugMn.Text);
			Tube.Чугун.P = ToDouble(_chugP.Text);
			Tube.Чугун.S = ToDouble(_chugS.Text);
			Tube.Чугун.Si = ToDouble(_chugSi.Text);

			Tube.Сталь.T = ToDouble(_steelTemp.Text);
			Tube.Сталь.C = ToDouble(_steelC.Text);
			Tube.Сталь.PMAX = ToDouble(_steelP.Text);

			if (!Params.IsDuplex)
			{
				Tube.Шлак.Bmin = ToDouble(_bMin.Text);
				Tube.Шлак.Bmax = ToDouble(_bMax.Text);
			}
			else
			{
				Tube.Шлак.V2O5 = ToDouble(_shlakV2O5.Text);
			}

			if (_chkLeftShlak.IsChecked.HasValue && _chkLeftShlak.IsChecked.Value)
			{
				Tube.ОставленныйШлак.G = ToDouble(_gostshl.Text);
			}
			else
			{
				Tube.ОставленныйШлак.G = 0;
			}
		}

		private void PreviousExecuted(object sender, ExecutedRoutedEventArgs e)
		{
		}

		private void NextCanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		private void PreviousCanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
		}

		private float ToFloat(string text)
		{
			var convertBack = _converter.ConvertBack(text, typeof (float), null, null);
			return Convert.ToSingle(convertBack);
		}

		private double ToDouble(string text)
		{
			return (double) _converter.ConvertBack(text, typeof(double), null, null);
		}

		private enum Row
		{
			TempChug = 0,
			CChug = 1,
			SiChug = 2,
			MnChug = 3,
			PChug = 4,
			SChug = 5,
			TempSteel = 6,
			CSteel = 7,
			PSteel = 8,
			Bmin = 9,
			Bmax = 10,
			Gostshl = 11
		}
	}
}
