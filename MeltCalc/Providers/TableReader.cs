using System;
using System.Data;
using System.Data.OleDb;
using System.IO;

namespace MeltCalc.Providers
{
	public class TableReader
	{
		private const string ConnStr = "Provider=Microsoft.JET.OLEDB.4.0;data source={0};";
		private readonly string _file;

		public TableReader(string file)
		{
			_file = file;
			if (!File.Exists(_file))
			{
				throw new FileNotFoundException(string.Format("File not found: '{0}'", _file));
			}

			SubKey = Path.GetFileName(_file);
		}

		public virtual DataTable FetchTable(string table)
		{
			using (var conn = new OleDbConnection(string.Format(ConnStr, _file)))
			{
				conn.Open();

				using (var cmd = new OleDbCommand(string.Format("select * from {0}", table)) { Connection = conn })
				{
					using (var oleDbDataReader = cmd.ExecuteReader())
					{
						if (oleDbDataReader == null)
						{
							throw new ApplicationException(string.Format("Reader is null for 'select * from {0}'", table));
						}

						using (var dt = new DataTable())
						{
							dt.Load(oleDbDataReader);
							return dt;
						}
					}
				}
			}
		}

		protected string SubKey { get; private set; }
	}
}