using System;
using System.Collections.Generic;

namespace MeltCalc.Model
{
	public static class Mapper
	{
		// Названия материалов в родит. п.
		private static readonly Dictionary<Materials, string> _genitives;

		static Mapper()
		{
			_genitives = new Dictionary<Materials, string>();
			_genitives[Materials.ПлавиковыйШпат] = "Шпата";
			_genitives[Materials.Агломерат] = "Агломерата";
			_genitives[Materials.Доломит] = "Доломита";
			_genitives[Materials.ИзвестковоМагнитныйФлюс] = "ИМФ";
			_genitives[Materials.Известняк] = "Известняка";
			_genitives[Materials.Известь] = "Извести";
			_genitives[Materials.Кокс] = "Кокса";
			_genitives[Materials.Окалина] = "Окалины";
			_genitives[Materials.Окатыши] = "Окатышей";
			_genitives[Materials.Песок] = "Песка";
			_genitives[Materials.ВлажныйДоломит] = "Сырого Доломита";
			_genitives[Materials.Руда] = "Руды";
		}
  
		public static string ToName(this Materials materials)
		{
			switch (materials)
			{
				case Materials.ИзвестковоМагнитныйФлюс:
					return "ИМФ";
				case Materials.ВлажныйДоломит:
					return "Вл доломит";
				case Materials.ПлавиковыйШпат:
					return "П шпат";
				default:
					return materials.ToString();
			}
		}

		public static string ToGenitive(this Materials material)
		{
			return _genitives[material];
		}

		public static string ToTableName(this Materials materials)
		{
			switch (materials)
			{
				case Materials.ИзвестковоМагнитныйФлюс:
					return "IMF";
				case Materials.ВлажныйДоломит:
					return "VlDol";
				case Materials.ПлавиковыйШпат:
					return "Shp";
				case Materials.Кокс:
					return "Koks";
				case Materials.Известь:
					return "Izv";
				case Materials.Известняк:
					return "Izk";
				case Materials.Доломит:
					return "Dol";
				case Materials.Окалина:
					return "Okal";
				case Materials.Агломерат:
					return "Agl";
				case Materials.Окатыши:
					return "Okat";
				case Materials.Песок:
					return "Pes";
				case Materials.Руда:
					return "Ruda";
				default:
					throw new Exception("Not found material");
			}
		}
	}
}