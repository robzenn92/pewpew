using Pewpew.BrainMood.ObjectModel.AzureModel;
using Pewpew.BrainMood.ObjectModel.ServiceModel;
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

		public static DetectionDTO GetLastDetection()
		{
			List<DetectionEntity> result = new List<DetectionEntity>();
			DetectionEntity attention = null;
			DetectionEntity meditation = null;
			long max0 = -1;
			long max1 = -1;
			foreach (var item in Context.DetectionTable)
			{
				if (item.TypeOfFrequency == 0)
				{
					if (max0 < item.InsertDateTime)
					{
						attention = item;
						max0 = item.InsertDateTime;
					}
				}
				if (item.TypeOfFrequency == 1)
				{
					if (max1 < item.InsertDateTime)
					{
						meditation = item;
						max1 = item.InsertDateTime;
					}
				}
			}

			return new DetectionDTO()
			{
				Attention = attention != null ? attention.Value : 0,
				Meditation = meditation != null ? meditation.Value : 0
			};
		}

		public static List<long> s { get; set; }
	}
}
