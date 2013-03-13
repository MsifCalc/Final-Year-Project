using System.Collections.Generic;

namespace MeltCalc.Helpers
{
	public class Map<TKey, TValue>
	{
		private readonly IDictionary<TKey, TValue> _map;

		public Map(IDictionary<TKey, TValue> map)
		{
			_map = map;
		}

		public TValue this[TKey key]
		{
			get
			{
				if (!_map.ContainsKey(key))
				{
					throw new KeyNotFoundException(string.Format("Key '{0}' not found", key));
				}

				return _map[key];
			}
		}
	}
}