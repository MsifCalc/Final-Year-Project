using System.IO;
using MeltCalc.Properties;

namespace MeltCalc.Providers
{
	public abstract class MdbReader
	{
		private readonly TableCacheReader _cacheReader;

		protected MdbReader(string path)
		{
			path = Path.Combine(System.Environment.CurrentDirectory, Settings.Default.DatabaseRelativePath, path);
			ValidatePath(path);
			_cacheReader = new TableCacheReader(path);
		}

		public TableCacheReader Reader
		{
			get { return _cacheReader; }
		}

		public int RowCount(string table)
		{
			return Reader.FetchTable(table).Rows.Count;
		}

		private static void ValidatePath(string path)
		{
			if (!File.Exists(path))
			{
				throw new FileNotFoundException(path);
			}
		}
	}
}