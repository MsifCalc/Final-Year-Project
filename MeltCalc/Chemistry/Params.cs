namespace MeltCalc.Chemistry
{
	public static class Params
	{
		public const int IterationNumber = 6;

		public static double alfaFe;
		public static double L;
		public static double StAndShlLoss;
		public static double TAUprost;
		public static double TAUprostREAL;
		public static double TeplFutLoss;
		public static double TAPtime;
		public static double Lp;
		public static double Tog;

		public static int SelectedPlant;
		public static int SelectedAdaptedPlant;
		public static int FutTypeSelected;
		public static int LomTypeSelected;
		public static int AirTemp;
		public static int FutDurability;
		public static int BlowingTime;
		public static bool BottomBlowUse;

		// ѕоказывает шаг уточнени€, всего 4 шага.
		public static int Round;
		// ѕоказывает, прошли ли по P, а также увеличивали ли основность.
		public static bool OkPst;
		// ѕоказывает, отрицательное ли значение рассчитанного сыпучего.
		public static bool LessThanZero;

		public static string InputForm;
		public static bool IsDuplex;

		static Params()
		{
			Ћомћелкий = new Ћомћелкий();
			Ћом—редний = new Ћом—редний();
			Ћом рупный = new Ћом рупный();
		}

		// cmdLastEstimate не реализован.

		public static Ћомћелкий Ћомћелкий { get; set; }
		public static Ћом—редний Ћом—редний { get; set; }
		public static Ћом рупный Ћом рупный { get; set; }
	}
}