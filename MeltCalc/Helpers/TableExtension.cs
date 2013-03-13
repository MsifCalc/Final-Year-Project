using System.Data;
using System.Linq;

namespace MeltCalc.Helpers
{
	public static class TableExtension
	{
		public static int RowCount(this DataTable table)
		{
			return table.Rows.Count;
		}

		public static string[] SelectRowRange(this DataTable table, int index)
		{
			return table.Rows[index].ItemArray.Select(item => item.ToString()).ToArray();
		}
	}
}