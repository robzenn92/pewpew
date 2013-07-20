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
			var max = (from y in Context.DetectionTable
					   where y.TypeOfFrequency == 0
					   select y.InsertDateTime).Max();
			var item = (from x in Context.DetectionTable
						where x.TypeOfFrequency == 0
						   && x.InsertDateTime == max
						select x).FirstOrDefault();
			result.Add(item);

			var max1 = (from y in Context.DetectionTable
					   where y.TypeOfFrequency == 0
					   select y.InsertDateTime).Max();
			var item1 = (from x in Context.DetectionTable
						where x.TypeOfFrequency == 0
						   && x.InsertDateTime == max1
						select x).FirstOrDefault();
			result.Add(item1);

			return result.AsQueryable<DetectionEntity>();
		}
	}
}
