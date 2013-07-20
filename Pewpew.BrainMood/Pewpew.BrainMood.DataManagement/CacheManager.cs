using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace Pewpew.BrainMood.DataManagement
{
	public static class CacheManager
	{
		private static MemoryCache cache = MemoryCache.Default;

		public static void AddOrUpdate(string key, object value)
		{
			object valCache = cache.Get(key);

			if (valCache != null)
				cache.Remove(key);

			cache.Add(key,
				value,
				new CacheItemPolicy()
				{
					SlidingExpiration = new TimeSpan(0, 2, 0)
				});
		}

		public static object GetValue(string key)
		{
			return cache.Get(key);
		}
	}
}
