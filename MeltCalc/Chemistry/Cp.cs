using System;
using System.Collections.Generic;
using System.Windows;
using MeltCalc.Helpers;
using MeltCalc.Model;
using System.Linq;

namespace MeltCalc.Chemistry
{
	// Переменные теплового баланса
	public static class Cp
	{
		private const string CpTable = "Cp";
		private static readonly TeploPhysConstantsMdb _constantsMdb;

		static Cp()
		{
			_constantsMdb = new TeploPhysConstantsMdb();
			LoadConstants();	
		}

		public static double H2O { get; set; }
		public static double CO { get; set; }
		public static double CO2 { get; set; }
		public static double N2 { get; set; }
		public static double Ar { get; set; }
		public static double O2 { get; set; }
		public static double Dut { get; set; }
		public static double FeO { get; set; }
		public static double Fe { get; set; }
		public static double IMF { get; set; }

		public static double Alloy { get; set; }
		public static double izv { get; set; }
		public static double izk { get; set; }
		public static double koks { get; set; }
		public static double pes { get; set; }
		public static double ruda { get; set; }
		public static double shp { get; set; }
		public static double okal { get; set; }
		public static double okat { get; set; }
		public static double agl { get; set; }
		public static double dol { get; set; }
		public static double vldol { get; set; }

		// CpChugSolid, CpMetRZ - не используется!

		public static double ChugSolid { get; set; }
		public static double ChugLiquid { get; set; }
		public static double LomSolid { get; set; }
		public static double Met { get; set; }

		//Что-то связанное с известью.
		public static double Densing { get; set; }

		public static double DUT { get; set; }

		private static void LoadConstants()
		{
			var rows = _constantsMdb.Reader
				.SelectAllRows(CpTable)
				.ToDictionary(row => row[0], row => row[1]);

			// CpChugSolid, CpMetRZ - не используется!

			ChugLiquid	= SafeValue("CpChugLiquid", rows);
			LomSolid	= SafeValue("CpLomSolid", rows);
			Met			= SafeValue("CpMet", rows);
		}

		private static double SafeValue(string param, IDictionary<string, string> rows)
		{
			try
			{
				return rows[param].ToDoubleOrZero();
			}
			catch (Exception)
			{
				var msg = string.Format("Failed to find '{0}'. All the following calculations will not be correct", param);
				MessageBox.Show(msg);
			}
			return 0.0;
		}
	}
}