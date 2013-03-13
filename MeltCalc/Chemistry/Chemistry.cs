using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using MeltCalc.Converters;
using MeltCalc.Model;
using MeltCalc.Helpers;
using MeltCalc.Providers;
using System.Linq;

namespace MeltCalc.Chemistry
{
	public class Навеска
	{
		private const int InvalidMaterial = -1000;
		private static readonly LooseMdb _looseMdb = new LooseMdb();

		protected Навеска()
		{
			Material = (Materials) InvalidMaterial;
		}

		/// <summary>
		/// Создание с записью в реестр и с сохранением кода материала - это название таблицы из loose.mdb
		/// </summary>
		protected Навеска(ICollection<Навеска> registry)
		{
			Material = (Materials)InvalidMaterial;
			registry.Add(this);
		}

		/// <summary>
		/// Масса
		/// </summary>
		public double G { get; set; }
		/// <summary>
		/// Усвоение
		/// </summary>
		public double ALFA { get; set; }
		/// <summary>
		/// Сыпучий материал - шлакообразующий элемент?
		/// </summary>
		public Materials Material { get; protected set; }

		public void Load()
		{
			Load(GetSavedSelection(SavedColumnIndex) - 1);
		}

		/// <summary>
		/// Загрузка материала из БД
		/// </summary>
		public virtual void Load(int index)
		{
			try
			{
				var map = new Map<string, string>(LoadRowDictionary(index));
				PropertyLoad(map);
				AfterPropertyLoad(map);
			}
			catch (Exception ex)
			{
				throw new Exception(string.Format("Material '{0}' failed to load", Material), ex);
			}
		}

		protected virtual TableCacheReader Reader
		{
			get { return _looseMdb.Reader; }
		}

		/// <summary>
		/// Возвращает сохраненный индекс из "save" таблицы базы loose.mdb
		/// </summary>
		protected virtual int SavedColumnIndex
		{
			get { return -1; }
		}

		/// <summary>
		/// Код материала
		/// </summary>
		protected virtual string TableName
		{
			get { return Material.ToTableName(); }
		}

		protected virtual void PropertyLoad(Map<string, string> map)
		{}

		protected virtual void AfterPropertyLoad(Map<string, string> map)
		{
			ALFA = map["Assimilation"].ToDouble();
		}

		protected string[] LoadRow(int rowindex)
		{
			if (string.IsNullOrWhiteSpace(TableName))
			{
				throw new NullReferenceException("TableName is not initialized");
			}

			return _looseMdb.Reader.SelectRowRange(TableName, rowindex);
		}

		/// <summary>
		/// Возвращает сохраненный индекс из "save" таблицы базы loose.mdb
		/// Нужно передать номер колонки.
		/// </summary>
		private int GetSavedSelection(int columnNumber)
		{
			var idxs = _looseMdb.Reader.SelectRowRange("save", 0).Select(x=>x.ToInt()).ToArray();
			if (columnNumber < 0)
			{
				throw new InvalidOperationException(
					string.Format("This kind of operation is not supported for '{0}'", Material));
			}
			if (columnNumber >= idxs.Length)
			{
				throw new ArgumentException(string.Format("Column number = '{0}' exceeds array lenght ", columnNumber));
			}
			return idxs[columnNumber];
		}

		private Dictionary<string, string> LoadRowDictionary(int rowindex)
		{
			if (string.IsNullOrWhiteSpace(TableName))
			{
				throw new NullReferenceException("TableName is not initialized");
			}

			return Reader.SelectRowDictionary(TableName, rowindex);
		}
	}

	public class Известь : Навеска
	{
		public Известь(ICollection<Навеска> registry)
			: base(registry)
		{
			Material = Materials.Известь;
		}

		public double Al2O3 { get; set; }
		public double CaO { get; set; }
		public double H2O { get; set; }
		public double MgO { get; set; }
		public double P2O5 { get; set; }
		public double SiO2 { get; set; }

		protected override int SavedColumnIndex
		{
			get { return 1; }
		}

		protected override void PropertyLoad(Map<string, string> map)
		{
			CaO = map["CaO"].ToDouble();
			SiO2 = map["SiO2"].ToDouble();
			MgO = map["MgO"].ToDouble();
			P2O5 = map["P2O5"].ToDouble();
			H2O = map["H2O"].ToDouble();
			Al2O3 = map["Al2O3"].ToDouble();
		}
	}

	public class Известняк : Навеска
	{
		public Известняк(ICollection<Навеска> registry)
			: base(registry)
		{
			Material = Materials.Известняк;
		}

		public double CO2 { get; set; }
		public double CaCO3 { get; set; }
		public double CaO { get; set; }
		public double H2O { get; set; }
		public double P2O5 { get; set; }
		public double SiO2 { get; set; }

		protected override int SavedColumnIndex
		{
			get { return 2; }
		}

		protected override void PropertyLoad(Map<string, string> map)
		{
			CaCO3 = map["CaCO3"].ToDouble();
			H2O = map["H2O"].ToDouble();
			P2O5 = map["P2O5"].ToDouble();
			SiO2 = map["SiO2"].ToDouble();
		}
	}

	public class Окалина : Навеска
	{
		public Окалина(ICollection<Навеска> registry)
			: base(registry)
		{
			Material = Materials.Окалина;
		}

		public double Fe2O3 { get; set; }
		public double Fe3O4 { get; set; }
		public double FeO { get; set; }
		public double MgO { get; set; }
		public double MnO { get; set; }
		public double P { get; set; }
		public double SiO2 { get; set; }

		protected override int SavedColumnIndex
		{
			get { return 10; }
		}

		protected override void PropertyLoad(Map<string, string> map)
		{
			Fe3O4 = map["Fe3O4"].ToDouble();
			MgO = map["MgO"].ToDouble();
			MnO = map["MnO"].ToDouble();
			P = map["P"].ToDouble();
			SiO2 = map["SiO2"].ToDouble();
		}
	}

	public class Шпат : Навеска
	{
		public Шпат(ICollection<Навеска> registry)
			: base(registry)
		{
			Material = Materials.ПлавиковыйШпат;
		}

		public double CaF2 { get; set; }
		public double CaO { get; set; }
		public double SiO2 { get; set; }

		protected override int SavedColumnIndex
		{
			get { return 12; }
		}

		protected override void PropertyLoad(Map<string, string> map)
		{
			CaF2 = map["CaF2"].ToDouble();
			SiO2 = map["SiO2"].ToDouble();
		}
	}

	public class Шлак : Навеска
	{
		public double Al2O3 { get; set; }
		public double B { get; set; }
		public double Bmax { get; set; }
		public double Bmin { get; set; }
		public double CaO { get; set; }
		public double Fe2O3 { get; set; }
		public double FeO { get; set; }
		public double MgO { get; set; }
		public double MnO { get; set; }
		public double P2O5 { get; set; }
		public double SiO2 { get; set; }
		public double TOTALFeO { get; set; }
		public double V2O5 { get; set; }
	}

	public class ОставленныйШлак : Навеска
	{
		private const int Index = 1;
		private static readonly ParamsMdb _paramMdb = new ParamsMdb();

		public double Al2O3 { get; set; }
		public double CaO { get; set; }
		public double Fe2O3 { get; set; }
		public double FeO { get; set; }
		public double MgO { get; set; }
		public double MnO { get; set; }
		public double P2O5 { get; set; }
		public double SiO2 { get; set; }

		public void Load()
		{		
			Load(Index);
		}

		protected override TableCacheReader Reader
		{
			get { return _paramMdb.Reader; }
		}

		protected override string TableName
		{
			get { return "mixandostshl"; }
		}

		protected override void PropertyLoad(Map<string, string> map)
		{
			CaO = map["СаО"].ToDouble();
			Fe2O3 = map["Fe2O3"].ToDouble();
			FeO = map["FeO"].ToDouble();
			MgO = map["MgO"].ToDouble();
			MnO = map["MnO"].ToDouble();
			P2O5 = map["P2O5"].ToDouble();
			SiO2 = map["SiO2"].ToDouble();
		}

		protected override void AfterPropertyLoad(Map<string, string> map)
		{
		}
	}

	public class МиксерныйШлак : Навеска
	{
		private const int Index = 0;
		private static readonly ParamsMdb _paramMdb = new ParamsMdb();

		public double CaO { get; set; }
		public double Fe2O3 { get; set; }
		public double FeO { get; set; }
		public double MgO { get; set; }
		public double MnO { get; set; }
		public double P2O5 { get; set; }
		public double SiO2 { get; set; }

		public void Load()
		{
			Load(Index);
		}

		protected override TableCacheReader Reader
		{
			get { return _paramMdb.Reader; }
		}

		protected override string TableName
		{
			get { return "mixandostshl"; }
		}

		protected override void PropertyLoad(Map<string, string> map)
		{
			CaO = map["СаО"].ToDouble();
			Fe2O3 = map["Fe2O3"].ToDouble();
			FeO = map["FeO"].ToDouble();
			MgO = map["MgO"].ToDouble();
			MnO = map["MnO"].ToDouble();
			P2O5 = map["P2O5"].ToDouble();
			SiO2 = map["SiO2"].ToDouble();
		}

		protected override void AfterPropertyLoad(Map<string, string> map)
		{
		}
	}

	public class Чугун : Навеска
	{
		public double C { get; set; }
		public double Mn { get; set; }
		public double P { get; set; }
		public double S { get; set; }
		public double Si { get; set; }
		public double T { get; set; }
	}

	public class Сталь : Навеска
	{
		public double C { get; set; }
		public double GYield { get; set; }
		public double GYieldmemo { get; set; }
		public double Mn { get; set; }
		public double P { get; set; }
		public double PMAX { get; set; }
		public double S { get; set; }
		public double SMax { get; set; }
		public double Si { get; set; }
		public double T { get; set; }
		public double Tplav { get; set; }
		public double V { get; set; }
	}

	/// <summary>
	/// This is a row from params.mdb.
	/// Result lom composition.
	/// </summary>
	public class Лом : Навеска
	{
		private const string Lomdata = "lomdata";
		private static readonly ParamsMdb _paramsMdb = new ParamsMdb();
		private static readonly StringToDoubleConverter _converter = new StringToDoubleConverter();

		private readonly RowIndex _index;
		private readonly DataTable _table;

		public Лом()
		{
		}

		public Лом(RowIndex index)
		{
			_index = index;
			_table = _paramsMdb.Reader.FetchTable(Lomdata);

			Mn = ReadValue("Mn");
			Si = ReadValue("Si");
			P = ReadValue("P");
			S = ReadValue("S");
			C = ReadValue("С");
			Part = ReadValue("Доля лома");
		}

		public float C { get; set; }
		public float DolyaLegkovesa { get; set; }
		public float Mn { get; set; }
		public float P { get; set; }
		public float S { get; set; }
		public float Si { get; set; }
		public float Part { get; set; }

		private float ReadValue(string column)
		{
			var value = _table.Rows[(int)_index][column];
			try
			{
				return (float)_converter.ConvertBack(value, typeof(float), null, null);
			}
			catch (Exception e)
			{
				// TODO: Удалить.
				MessageBox.Show(e.ToString());
			}
			return 0;
		}

		/// <summary>
		/// Углеродность - Размерность.
		/// </summary>
		public enum RowIndex
		{
			LowSmall = 0,
			LowMed = 1,
			LowBig = 2,
			MidSmall = 3,
			MidMed = 4,
			MidBig = 5,
			HighSmall = 6,
			HighMed = 7,
			HighBig = 8
		}
	}

	public class Футеровка : Навеска
	{
		public double Al2O3 { get; set; }
		public double C { get; set; }
		public double CaO { get; set; }
		public double MgO { get; set; }
		public double P2O5 { get; set; }
		public double SiO2 { get; set; }
		public double GTotal { get; set; }
	}

	public class Дутье : Навеска
	{
		public double Ar { get; set; }
		public double N2 { get; set; }
		public double O2 { get; set; }
		public double V { get; set; }
	}

	public class Имф : Навеска
	{
		public Имф(ICollection<Навеска> registry)
			: base(registry)
		{
			Material = Materials.ИзвестковоМагнитныйФлюс;
		}

		public double CaO { get; set; }
		public double Fe2O3 { get; set; }
		public double MgO { get; set; }
		public double SiO2 { get; set; }

		protected override int SavedColumnIndex
		{
			get { return 5; }
		}

		protected override void PropertyLoad(Map<string, string> map)
		{
			CaO = map["CaO"].ToDouble();
			MgO = map["MgO"].ToDouble();
			SiO2 = map["SiO2"].ToDouble();
		}
	}

	public class Кокс : Навеска
	{
		public Кокс(ICollection<Навеска> registry)
			: base(registry)
		{
			Material = Materials.Кокс;
		}

		public double C { get; set; }

		protected override int SavedColumnIndex
		{
			get { return 6; }
		}

		protected override void PropertyLoad(Map<string, string> map)
		{
			C = map["C"].ToDouble();
		}
	}

	public class Песок : Навеска
	{
		public Песок(ICollection<Навеска> registry)
			: base(registry)
		{
			Material = Materials.Песок;
		}

		public double H2O { get; set; }
		public double SiO2 { get; set; }

		protected override int SavedColumnIndex
		{
			get { return 7; }
		}

		protected override void PropertyLoad(Map<string, string> map)
		{
			H2O = map["H2O"].ToDouble();
			SiO2 = map["SiO2"].ToDouble();
		}
	}

	public class Руда : Навеска
	{
		public Руда(ICollection<Навеска> registry)
			: base(registry)
		{
			Material = Materials.Руда;
		}

		public double Al2O3 { get; set; }
		public double CaO { get; set; }
		public double Fe2O3 { get; set; }
		public double P { get; set; }
		public double SiO2 { get; set; }

		protected override int SavedColumnIndex
		{
			get { return 9; }
		}

		protected override void PropertyLoad(Map<string, string> map)
		{
			Al2O3 = map["Al2O3"].ToDouble();
			CaO = map["CaO"].ToDouble();
			Fe2O3 = map["Fe2O3"].ToDouble();
			P = map["P"].ToDouble();
			SiO2 = map["SiO2"].ToDouble();
		}
	}

	public class Окатыши : Навеска
	{
		public Окатыши(ICollection<Навеска> registry)
			: base(registry)
		{
			Material = Materials.Окатыши;
		}

		public double Fe2O3 { get; set; }
		public double FeO { get; set; }
		public double SiO2 { get; set; }

		protected override int SavedColumnIndex
		{
			get { return 8; }
		}

		protected override void PropertyLoad(Map<string, string> map)
		{
			Fe2O3 = map["Fe2O3"].ToDouble();
			FeO = map["FeO"].ToDouble();
			SiO2 = map["SiO2"].ToDouble();
		}
	}

	public class Агломерат : Навеска
	{
		public Агломерат(ICollection<Навеска> registry)
			: base(registry)
		{
			Material = Materials.Агломерат;
		}

		public double CaO { get; set; }
		public double Fe2O3 { get; set; }
		public double FeO { get; set; }

		protected override int SavedColumnIndex
		{
			get { return 11; }
		}

		protected override void PropertyLoad(Map<string, string> map)
		{
			CaO = map["CaO"].ToDouble();
			Fe2O3 = map["Fe2O3"].ToDouble();
			FeO = map["FeO"].ToDouble();
		}
	}

	public class Ферросплав : Навеска
	{
		public double Al { get; set; }
		public double C { get; set; }
		public double Fe
		{
			get { return 100.0 - Al - C - Mn - P - S - Si; }
		}
		public double Mn { get; set; }
		public double P { get; set; }
		public double S { get; set; }
		public double Si { get; set; }

		public static bool Validate(double galloy)
		{
			return !(Math.Abs(galloy - 0.0) < 0.000001 || (galloy >= 0.01 && galloy <= 10.0));
		}
	}

	public class ВлажныйДоломит : Навеска
	{
		public ВлажныйДоломит(ICollection<Навеска> registry)
			: base(registry)
		{
			Material = Materials.ВлажныйДоломит;
		}

		public double Al2O3 { get; set; }
		public double CO2 { get; set; }
		public double CaO { get; set; }
		public double Fe2O3 { get; set; }
		public double H2O { get; set; }
		public double MgO { get; set; }
		public double SiO2 { get; set; }

		protected override int SavedColumnIndex
		{
			get { return 4; }
		}

		protected override void PropertyLoad(Map<string, string> map)
		{
			Al2O3 = map["Al2O3"].ToDouble();
			CO2 = map["CO2"].ToDouble();
			CaO = map["CaO"].ToDouble();
			Fe2O3 = map["Fe2O3"].ToDouble();
			H2O = map["H2O"].ToDouble();
			MgO = map["MgO"].ToDouble();
			SiO2 = map["SiO2"].ToDouble();
		}
	}

	public class Доломит : Навеска
	{
		public Доломит(ICollection<Навеска> registry)
			: base(registry)
		{
			Material = Materials.Доломит;
		}

		public double Al2O3 { get; set; }
		public double CaO { get; set; }
		public double Fe2O3 { get; set; }
		public double MgO { get; set; }
		public double SiO2 { get; set; }

		protected override int SavedColumnIndex
		{
			get { return 3; }
		}

		protected override void PropertyLoad(Map<string, string> map)
		{
			Al2O3 = map["Al2O3"].ToDouble();
			CaO = map["CaO"].ToDouble();
			Fe2O3 = map["Fe2O3"].ToDouble();
			MgO = map["MgO"].ToDouble();
			SiO2 = map["SiO2"].ToDouble();
		}
	}
}