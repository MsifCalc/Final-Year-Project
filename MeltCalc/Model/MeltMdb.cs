using MeltCalc.Properties;
using MeltCalc.Providers;

namespace MeltCalc.Model
{
	public class MeltMdb : MdbReader
	{
		public MeltMdb() 
			: base(Settings.Default.MelpMdb)
		{
		}
	}
}