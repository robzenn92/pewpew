using Pewpew.BrainMood.ObjectModel.AzureModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pewpew.BrainMood.DataManagement
{
	public class GraphManager
	{
		private static TableStorageContext Context { get; set; }

		static GraphManager()
		{
			Context = TableStorageContext.Instance;
		}

		public static IQueryable<DetectionEntity> GetLastDetection()
		{
			List<DetectionEntity> result = new List<DetectionEntity>();
			var values= (from y in Context.DetectionTable
							 where y.TypeOfFrequency == 0
							 select y.InsertDateTime).ToList();

			long max = -1;
			foreach (var y in values)
			{
				if (y > max)
					max = y;
			}
			var item = (from x in Context.DetectionTable
						where x.TypeOfFrequency == 0
						   && x.InsertDateTime == max
						select x).FirstOrDefault();
			result.Add(item);

			var values1 = (from y in Context.DetectionTable
						   where y.TypeOfFrequency == 0
						   select y.InsertDateTime).ToList();

			long max1 = -1;
			foreach (var y in values1)
			{
				if (y > max1)
					max1 = y;
			}

			var item1 = (from x in Context.DetectionTable
						 where x.TypeOfFrequency == 0
							&& x.InsertDateTime == max1
						 select x).FirstOrDefault();
			result.Add(item1);

			return result.AsQueryable<DetectionEntity>();
		}

		public static List<long> s { get; set; }
	}
}
