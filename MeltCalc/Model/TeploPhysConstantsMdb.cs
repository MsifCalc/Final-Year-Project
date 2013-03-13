using MeltCalc.Properties;
using MeltCalc.Providers;

namespace MeltCalc.Model
{
	public class TeploPhysConstantsMdb : MdbReader
	{
		public TeploPhysConstantsMdb()
			: base(Settings.Default.TeploPhisConstsMdb)
		{
		}
	}
}