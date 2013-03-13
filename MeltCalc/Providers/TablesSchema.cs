using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace MeltCalc.Providers
{
	/// <summary>
	/// Ñõåìà ÁÄ.
	/// </summary>
	public class TablesSchema
	{
		private const string Pattern = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0}";
		private static readonly DbProviderFactory _factory = DbProviderFactories.GetFactory("System.Data.OleDb");

		private readonly string _file;

		public TablesSchema(string file)
		{
			_file = file;
		}

		public IEnumerable<string> GetTableNames()
		{
			DataTable userTables;

			using (var connection = _factory.CreateConnection())
			{
				if (connection == null)
					throw new ArgumentException("connection");

				connection.ConnectionString = string.Format(Pattern, _file);

				// We only want user tables, not system tables
				var restrictions = new string[4];
				restrictions[3] = "Table";

				connection.Open();

				// Get list of user tables
				userTables = connection.GetSchema("Tables", restrictions);
			}

			return userTables.Rows
				.Cast<DataRow>()
				.Select(row => row[2].ToString()).ToList();
		}
	}
}