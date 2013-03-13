using System;
using System.Linq;
using System.Windows;
using MeltCalc.Helpers;
using MeltCalc.Model;

namespace MeltCalc.Chemistry
{
	/// <summary>
	/// Стандартные изменения энтальпий реакций окисляющегося компонента
	/// </summary>
	public static class Hp
	{
		private const string CpTable = "dH_dS";
		private static readonly TeploPhysConstantsMdb _constantsMdb = new TeploPhysConstantsMdb();

		static Hp()
		{
			LoadConstantSafe();
		}

		public static double dSc_mno_mol { get; set; }
		public static double dHc_mno_mol { get; set; }
		public static double dSp_feo_mol { get; set; }
		public static double dHp_feo_mol { get; set; }
		public static double dSmn_feo_mol { get; set; }
		public static double dHmn_feo_mol { get; set; }
		public static double dSsi_feo_mol { get; set; }
		public static double dHsi_feo_mol { get; set; }
		public static double dSc_feo_mol { get; set; }
		public static double dHc_feo_mol { get; set; }
		public static double dSc_O2_mol { get; set; }
		public static double dSco_co2_mol { get; set; }
		public static double dSs_O2_mol { get; set; }
		public static double dHs_O2_mol { get; set; }
		public static double dSp_O2_mol { get; set; }
		public static double dSsi_O2_mol { get; set; }
		public static double dSmn_O2_mol { get; set; }
		public static double dSfe_O2_mol { get; set; }
		public static double dHmshl { get; set; }
		public static double dHshl { get; set; }
		public static double dHst { get; set; }
		public static double dHchug { get; set; }
		public static double dHlom { get; set; }
		public static double dHostshl { get; set; }
		public static double dHizkPlavl { get; set; }
		public static double dHlomPlavl { get; set; }
		public static double dHchugPlavl { get; set; }
		public static double dHsio2_2caosio2 { get; set; }
		public static double dHp2o5_3caop2o5 { get; set; }
		public static double dHsi_O2_mol { get; set; }
		public static double dHmn_O2_mol { get; set; }
		public static double dHp_O2_mol { get; set; }
		public static double dHfe_O2_mol { get; set; }
		public static double dHfe_fe2o3_o2_mol { get; set; }
		public static double dHc_O2_mol { get; set; }
		public static double dHco_co2_mol { get; set; }

		private static void LoadConstantSafe()
		{
			try
			{
				LoadConstants();
			}
			catch (Exception)
			{
				MessageBox.Show(string.Format("Failed to load data from '{0}'", CpTable));
			}
		}

		private static void LoadConstants()
		{
			var rows = _constantsMdb.Reader
				.SelectAllRows(CpTable)
				.ToDictionary(row => row[0], row => new Tuple<string, string>(row[1], row[2]));

			dHfe_O2_mol = rows["fe_O2_mol"].Item1.ToDoubleOrZero();
			dSfe_O2_mol = rows["fe_O2_mol"].Item2.ToDoubleOrZero();

			dHmn_O2_mol = rows["mn_O2_mol"].Item1.ToDoubleOrZero();
			dSmn_O2_mol = rows["mn_O2_mol"].Item2.ToDoubleOrZero();

			dHsi_O2_mol = rows["si_O2_mol"].Item1.ToDoubleOrZero();
			dSsi_O2_mol = rows["si_O2_mol"].Item2.ToDoubleOrZero();

			dHp_O2_mol = rows["p_O2_mol"].Item1.ToDoubleOrZero();
			dSp_O2_mol = rows["p_O2_mol"].Item2.ToDoubleOrZero();

			dHs_O2_mol = rows["s_O2_mol"].Item1.ToDoubleOrZero();
			dSs_O2_mol = rows["s_O2_mol"].Item2.ToDoubleOrZero();

			dHco_co2_mol = rows["co_co2_mol"].Item1.ToDoubleOrZero();
			dSco_co2_mol = rows["co_co2_mol"].Item2.ToDoubleOrZero();

			dHc_O2_mol = rows["c_o2_mol"].Item1.ToDoubleOrZero();
			dSc_O2_mol = rows["c_o2_mol"].Item2.ToDoubleOrZero();

			dHc_feo_mol = rows["c_feo_mol"].Item1.ToDoubleOrZero();
			dSc_feo_mol = rows["c_feo_mol"].Item2.ToDoubleOrZero();

			dHsi_feo_mol = rows["si_feo_mol"].Item1.ToDoubleOrZero();
			dSsi_feo_mol = rows["si_feo_mol"].Item2.ToDoubleOrZero();

			dHmn_feo_mol = rows["mn_feo_mol"].Item1.ToDoubleOrZero();
			dSmn_feo_mol = rows["mn_feo_mol"].Item2.ToDoubleOrZero();

			dHp_feo_mol = rows["p_feo_mol"].Item1.ToDoubleOrZero();
			dSp_feo_mol = rows["p_feo_mol"].Item2.ToDoubleOrZero();

			dHc_mno_mol = rows["c_mno_mol"].Item1.ToDoubleOrZero();
			dSc_mno_mol = rows["c_mno_mol"].Item2.ToDoubleOrZero();

			dHfe_fe2o3_o2_mol = rows["fe_fe2o3_o2_mol"].Item1.ToDoubleOrZero();

			dHchugPlavl = rows["dHchugPlavl"].Item1.ToDoubleOrZero();
			dHlomPlavl = rows["dHlomPlavl"].Item1.ToDoubleOrZero();
			dHizkPlavl = rows["dHizkPlavl"].Item1.ToDoubleOrZero();
			dHsio2_2caosio2 = rows["dHsio2_2caosio2"].Item1.ToDoubleOrZero();
			dHp2o5_3caop2o5 = rows["dHp2o5_3caop2o5"].Item1.ToDoubleOrZero();
		}
	}
}