namespace MeltCalc.Chemistry
{
	public abstract class ЛомРазмерный
	{
		/// <summary>
		/// Низкоуглеродный лом.
		/// </summary>
		public Лом НизкоУглеродный { get; set; }

		/// <summary>
		/// Среднеуглеродный.
		/// </summary>
		public Лом СреднеУглеродный { get; set; }

		/// <summary>
		/// Высокоуглеродный.
		/// </summary>
		public Лом ВысокоУглеродный { get; set; }
	}

	public class ЛомМелкий : ЛомРазмерный
	{
		public ЛомМелкий()
		{
			НизкоУглеродный = new Лом(Лом.RowIndex.LowSmall);
			СреднеУглеродный = new Лом(Лом.RowIndex.MidSmall);
			ВысокоУглеродный = new Лом(Лом.RowIndex.HighSmall);
		}
	}

	public class ЛомСредний : ЛомРазмерный
	{
		public ЛомСредний()
		{
			НизкоУглеродный = new Лом(Лом.RowIndex.LowMed);
			СреднеУглеродный = new Лом(Лом.RowIndex.MidMed);
			ВысокоУглеродный = new Лом(Лом.RowIndex.HighMed);
		}
	}

	public class ЛомКрупный : ЛомРазмерный
	{
		public ЛомКрупный()
		{
			НизкоУглеродный = new Лом(Лом.RowIndex.LowBig);
			СреднеУглеродный = new Лом(Лом.RowIndex.MidBig);
			ВысокоУглеродный = new Лом(Лом.RowIndex.HighBig);
		}
	}
}