using System.Linq;
using System.Collections.Generic;
using MeltCalc.Model;

namespace MeltCalc.Chemistry
{
	public class Tube
	{
		private static readonly List<Навеска> _registry = new List<Навеска>();

		static Tube()
		{
			Известняк = new Известняк(_registry);
			Известь = new Известь(_registry);
			Шпат = new Шпат(_registry);
			Окалина = new Окалина(_registry);
			Шлак = new Шлак();
			ОставленныйШлак = new ОставленныйШлак();
			МиксерныйШлак = new МиксерныйШлак();
			Чугун = new Чугун();
			Сталь = new Сталь();
			Лом = new Лом();
			Футеровка = new Футеровка();
			Дутье = new Дутье();
			Имф = new Имф(_registry);
			Кокс = new Кокс(_registry);
			Песок = new Песок(_registry);
			Руда = new Руда(_registry);
			Окатыши = new Окатыши(_registry);
			Ферросплав = new Ферросплав();
			Агломерат = new Агломерат(_registry);
			Доломит = new Доломит(_registry);
			ВлажныйДоломит = new ВлажныйДоломит(_registry);
			Пакеты = new Packets();
			Шлакообразующий = 0;
		}

		public static Известняк Известняк { get; set; }
		public static Известь Известь { get; set; }
		public static Шпат Шпат { get; set; }
		public static Окалина Окалина { get; set; }
		public static Шлак Шлак { get; set; }
		public static ОставленныйШлак ОставленныйШлак { get; set; }
		public static МиксерныйШлак МиксерныйШлак { get; set; }
		public static Чугун Чугун { get; set; }
		public static Сталь Сталь { get; set; }
		public static Лом Лом { get; set; }
		public static Футеровка Футеровка { get; set; }
		public static Дутье Дутье { get; set; }
		public static Имф Имф { get; set; }
		public static Кокс Кокс { get; set; }
		public static Песок Песок { get; set; }
		public static Руда Руда { get; set; }
		public static Окатыши Окатыши { get; set; }
		public static Ферросплав Ферросплав { get; set; }
		public static Агломерат Агломерат { get; set; }
		public static Доломит Доломит { get; set; }
		public static ВлажныйДоломит ВлажныйДоломит { get; set; }
		public static Packets Пакеты { get; set; }
		public static Materials Шлакообразующий { get; set; }

		/// <summary>
		/// Находит игредиент по его типу.
		/// </summary>
		public static T FindSubstance<T>(Materials material) where T : Навеска
		{
			return (T) _registry.FirstOrDefault(x => x.Material == material);
		}
	}
}