using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace Pewpew.BrainMood.DataManagement
{
	public static class SequenceManager
	{
		static string key = "sequence";
		private static MemoryCache Cache { get; set; }

		static SequenceManager()
		{
			Cache = MemoryCache.Default;
		}

		public static void AddSequence(Guid id, string identificator)
		{

			IList<Guid> sequenceList = Cache.Get(GetKey(identificator)) as IList<Guid>;

			if (sequenceList == null)
				sequenceList = new List<Guid>();
			else
				Cache.Remove(GetKey(identificator));

			sequenceList.Add(id);
			Cache.Add(GetKey(identificator),
					sequenceList,
					new CacheItemPolicy()
					{
						SlidingExpiration = new TimeSpan(0, 20 , 0),
					});
			
		}

		public static IList<Guid> GetSequence(string identificator)
		{
			return Cache.Get(GetKey(identificator)) as IList<Guid>;
		}

		public static void Clear(string identificator)
		{
			Cache.Remove(GetKey(identificator));
		}

		private static string GetKey(string identificator)
		{
			return string.Format("{0} - {1}", key, identificator);
		}

	}
}
