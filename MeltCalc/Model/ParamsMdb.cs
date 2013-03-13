using MeltCalc.Properties;
using MeltCalc.Providers;

namespace MeltCalc.Model
{
	public class ParamsMdb : MdbReader
	{
		public ParamsMdb()
			: base(Settings.Default.ParamsMdb)
		{
		}
	}
}