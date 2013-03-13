using System.Windows;
using System.Windows.Controls;
using MeltCalc.Chemistry;

namespace MeltCalc.Pages
{
	/// <summary>
	/// Interaction logic for Results.xaml
	/// </summary>
	public partial class Results : Page
	{
		public Results()
		{
			InitializeComponent();
			Loaded += OnLoaded;
		}

		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			SHLcount();
			// TODO: Дизейбл тех контролов, которые не являются сыпучими.

			LoadCommonParams();
			LoadSypuch();
			LoadShlakCompund();
			LoadSteelCompound();

			// TODO: Черный шрифт окон.
			// TODO: Проверка на отрицательность
		}

		private void LoadCommonParams()
		{
			_massChugun.Text = string.Format("{0:0.00}", Estimation.GchugSAVE[Params.Round - 1]);
			_massLlom.Text = string.Format("{0:0.00}", Estimation.GlomSAVE[Params.Round - 1]);
			_oxygenVolume.Text = string.Format("{0:0.00}", Estimation.VdutSAVE[Params.Round - 1]);
		}

		private void LoadSypuch()
		{
			_izv.Text = string.Format("{0:0.00}", Estimation.GizvSAVE[Params.Round - 1]);
			_izk.Text = string.Format("{0:0.00}", Estimation.GizkSAVE[Params.Round - 1]);
			_dol.Text = string.Format("{0:0.00}", Estimation.GdolSAVE[Params.Round - 1]);
			_vldol.Text = string.Format("{0:0.00}", Estimation.GvldolSAVE[Params.Round - 1]);
			_imf.Text = string.Format("{0:0.00}", Estimation.GimfSAVE[Params.Round - 1]);
			_shp.Text = string.Format("{0:0.00}", Estimation.GshpSAVE[Params.Round - 1]);

			_koks.Text = string.Format("{0:0.00}", Tube.Кокс.G);
			_pesok.Text = string.Format("{0:0.00}", Tube.Песок.G);
			_okat.Text = string.Format("{0:0.00}", Tube.Окатыши.G);
			_ruda.Text = string.Format("{0:0.00}", Tube.Руда.G);
			_okal.Text = string.Format("{0:0.00}", Tube.Окалина.G);
			_agl.Text = string.Format("{0:0.00}", Tube.Агломерат.G);
		}

		private void LoadShlakCompund()
		{
			_osnovnost.Text = string.Format("{0:0.00}", Tube.Шлак.B);
			_massa.Text = string.Format("{0:0.000}", Estimation.GshlSAVE[Params.Round - 1]);
			_feo.Text = string.Format("{0:0.000}", Tube.Шлак.TOTALFeO);
			_cao.Text = string.Format("{0:0.00}", Tube.Шлак.CaO);
			_mgo.Text = string.Format("{0:0.00}", Tube.Шлак.MgO);
			_mno.Text = string.Format("{0:0.00}", Tube.Шлак.MnO);
			_p2o5.Text = string.Format("{0:0.00}", Tube.Шлак.P2O5);
		}

		private void LoadSteelCompound()
		{
			_c.Text = string.Format("{0:0.000}", Tube.Сталь.C);
			_si.Text = string.Format("{0:0.000}", Tube.Сталь.Si);
			_mn.Text = string.Format("{0:0.000}", Tube.Сталь.Mn);
			_p.Text = string.Format("{0:0.000}", Tube.Сталь.P);
			_s.Text = string.Format("{0:0.000}", Tube.Сталь.S);
			_temp.Text = string.Format("{0:0.000}", Tube.Сталь.T);
		}

		private static void SHLcount()
		{
			Tube.Шлак.CaO = (Tube.Известь.ALFA * Tube.Известь.G * (Tube.Известь.CaO) +
							 Tube.Известняк.ALFA * Tube.Известняк.G * 56.0 / 100.0 * Tube.Известняк.CaCO3 +
							 Tube.Доломит.ALFA * Tube.Доломит.G * (Tube.Доломит.CaO) +
							 Tube.ВлажныйДоломит.ALFA * Tube.ВлажныйДоломит.G * (Tube.ВлажныйДоломит.CaO) +
							 Tube.Имф.ALFA * Tube.Имф.G * (Tube.Имф.CaO) + Tube.Футеровка.G * (Tube.Футеровка.CaO) +
							 Tube.Агломерат.ALFA * Tube.Агломерат.G * Tube.Агломерат.CaO +
							 Tube.Руда.ALFA * Tube.Руда.G * Tube.Руда.CaO + Tube.ОставленныйШлак.G * (Tube.ОставленныйШлак.CaO) +
							 Tube.МиксерныйШлак.G * (Tube.МиксерныйШлак.CaO) +
							 Tube.Пакеты.ALFA * Tube.Пакеты.G * (Tube.Пакеты.CaO)) / Tube.Шлак.G;

			Tube.Шлак.MgO = (Tube.Известь.ALFA * Tube.Известь.G * (Tube.Известь.MgO) +
							 Tube.Доломит.ALFA * Tube.Доломит.G * (Tube.Доломит.MgO) +
							 Tube.Окалина.ALFA * Tube.Окалина.G * Tube.Окалина.MgO +
							 Tube.ВлажныйДоломит.ALFA * Tube.ВлажныйДоломит.G * (Tube.ВлажныйДоломит.MgO) +
							 Tube.Имф.ALFA * Tube.Имф.G * (Tube.Имф.MgO) + Tube.Футеровка.G * (Tube.Футеровка.MgO) +
							 Tube.ОставленныйШлак.G * (Tube.ОставленныйШлак.MgO) +
							 Tube.МиксерныйШлак.G * (Tube.МиксерныйШлак.MgO) +
							 Tube.Пакеты.ALFA * Tube.Пакеты.G * (Tube.Пакеты.MgO)) / Tube.Шлак.G;
		}
	}
}
