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

		// ���������� ��� ���������, ����� 4 ����.
		public static int Round;
		// ����������, ������ �� �� P, � ����� ����������� �� ����������.
		public static bool OkPst;
		// ����������, ������������� �� �������� ������������� ��������.
		public static bool LessThanZero;

		public static string InputForm;
		public static bool IsDuplex;

		static Params()
		{
			��������� = new ���������();
			���������� = new ����������();
			���������� = new ����������();
		}

		// cmdLastEstimate �� ����������.

		public static ��������� ��������� { get; set; }
		public static ���������� ���������� { get; set; }
		public static ���������� ���������� { get; set; }
	}
}