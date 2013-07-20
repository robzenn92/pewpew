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
			DetectionEntity entity1 = null;
			DetectionEntity entity2 = null;
			long max0 = -1;
			long max1 = -1;
			foreach (var item in Context.DetectionTable)
			{
				if (item.TypeOfFrequency == 0)
				{
					if (max0 < item.InsertDateTime)
						entity1 = item;
				}
				if (item.TypeOfFrequency == 1)
				{
					if (max0 < item.InsertDateTime)
						entity2 = item;
				}
			}
			result.Add(entity1);
			result.Add(entity2);
			return result.AsQueryable();
		}

		public static List<long> s { get; set; }
	}
}
