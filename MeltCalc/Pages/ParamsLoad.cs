using System.Linq;
using MeltCalc.Helpers;
using MeltCalc.Chemistry;
using MeltCalc.Model;

namespace MeltCalc.Pages
{
	public class ParamsLoad
	{
		private const int DefaultIndex = 0;
		private readonly ParamsMdb _paramsMdb = new ParamsMdb();

		public void Run()
		{
			Load_LOOSERANGE();
			Load_ChemOfMixShl();
			Load_ChemOfOstShl();

			if (Params.InputForm == "auto")
			{
				Load_COUNTDATA();
				// TODO: Step 12
			}
			else
			{
				// TODO: Load ParamsInput
			}
		}

		private void Load_COUNTDATA()
		{
			var range = _paramsMdb.Reader
				.SelectRowRange("countdata", Params.SelectedPlant)
				.Skip(2)
				.Select(x => x.ToDoubleOrDefault())
				.ToArray();

			var idx = -1;

			Tube.Сталь.GYield = range[++idx];
			Tube.Сталь.GYieldmemo = Tube.Сталь.GYield;
			Params.alfaFe = range[++idx];
			++idx;
			Params.L = range[++idx];
			Estimation.minimumGchug = range[++idx];
			Estimation.maximumGchug = range[++idx];
			Estimation.minimumGlom = range[++idx];
			Estimation.maximumGlom = range[++idx];
			Estimation.minimumVdut = range[++idx];
			Estimation.maximumVdut = range[++idx];
			Estimation.minimumGshl = range[++idx];
			Estimation.maximumGshl = range[++idx];
			Estimation.minimumMnOshl = range[++idx];
			Estimation.maximumMnOshl = range[++idx];
			Estimation.minimumPst = range[++idx];
			Estimation.maximumPst = range[++idx];
			Params.StAndShlLoss = range[++idx];
			Tube.Футеровка.GTotal = range[++idx];
			Tube.МиксерныйШлак.G = range[++idx];
			Params.TAUprost = range[++idx];
			Params.TeplFutLoss = range[++idx];

			// Строка Gfut = GfutTOTAL / FutDurability не выполняется никогда.

			// Эта строка в оригинале не выполняется никогда.

			if (Params.IsDuplex)
			{
				Estimation.maximumGokat = range[39];
			}
		}

		private void Load_ChemOfOstShl()
		{
			var range = LoadShlak(1);

			var idx = -1;

			Tube.ОставленныйШлак.CaO = range[++idx];
			Tube.ОставленныйШлак.SiO2 = range[++idx];
			Tube.ОставленныйШлак.MnO = range[++idx];
			Tube.ОставленныйШлак.MgO = range[++idx];
			Tube.ОставленныйШлак.P2O5 = range[++idx];
			Tube.ОставленныйШлак.FeO = range[++idx];
			Tube.ОставленныйШлак.Fe2O3 = range[++idx];
		}

		private void Load_ChemOfMixShl()
		{
			var range = LoadShlak(DefaultIndex);

			var idx = -1;

			Tube.МиксерныйШлак.CaO = range[++idx];
			Tube.МиксерныйШлак.SiO2 = range[++idx];
			Tube.МиксерныйШлак.MnO = range[++idx];
			Tube.МиксерныйШлак.MgO = range[++idx];
			Tube.МиксерныйШлак.P2O5 = range[++idx];
			Tube.МиксерныйШлак.FeO = range[++idx];
			Tube.МиксерныйШлак.Fe2O3 = range[++idx];
		}

		private double[] LoadShlak(int index)
		{
			var range = _paramsMdb.Reader
				.SelectRowRange("mixandostshl", index)
				.Skip(2)
				.Select(x => x.ToDoubleOrDefault())
				.ToArray();
			return range;
		}

		private void Load_LOOSERANGE()
		{
			
			var range = _paramsMdb.Reader
				.SelectRowRange("looserange", Params.InputForm == "auto" ? Params.SelectedPlant : DefaultIndex)
				.Skip(2)
				.Take(25)
				.Select(x => x.ToDoubleOrDefault())
				.ToArray();

			var idx = -1;

			Estimation.minimumGizv = range[++idx];
			Estimation.maximumGizv = range[++idx];
			Estimation.minimumGizk = range[++idx];
			Estimation.maximumGizk = range[++idx];
			Estimation.minimumGdol = range[++idx];
			Estimation.maximumGdol = range[++idx];
			Estimation.minimumGvldol = range[++idx];
			Estimation.maximumGvldol = range[++idx];
			Estimation.minimumGimf = range[++idx];
			Estimation.maximumGimf = range[++idx];
			Estimation.minimumGpes = range[++idx];
			Estimation.maximumGpes = range[++idx];
			Estimation.minimumGkoks = range[++idx];
			Estimation.maximumGkoks = range[++idx];
			Estimation.minimumGokat = range[++idx];
			Estimation.maximumGokat = range[++idx];
			Estimation.minimumGruda = range[++idx];
			Estimation.maximumGruda = range[++idx];
			Estimation.minimumGokal = range[++idx];
			Estimation.maximumGokal = range[++idx];
			Estimation.minimumGagl = range[++idx];
			Estimation.maximumGagl = range[++idx];
			Estimation.minimumGshp = range[++idx];
			Estimation.maximumGshp = range[++idx];
		}
	}
}
