using MeltCalc.Properties;
using MeltCalc.Providers;

namespace MeltCalc.Model
{
	public class LooseMdb : MdbReader
	{
		public LooseMdb() 
			: base(Settings.Default.LooseMdb)
		{
		}
	}
}