using Pewpew.BrainMood.ObjectModel;
using Pewpew.BrainMood.ObjectModel.AzureModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace Pewpew.BrainMood.DataManagement
{
	public class Calculator
	{
		private TableStorageContext Context { get; set; }

		public Calculator()
		{
			Context = TableStorageContext.Instance;
		}

		private IQueryable<DetectionEntity> ReadDetection()
		{
			var val = CacheManager.GetValue("lastupdate");
			DateTime lastUpdate = val == null ? DateTime.MinValue : (DateTime)val;

			if (lastUpdate == DateTime.MinValue)
				lastUpdate = DateTime.Now.AddMinutes(-1);

			return (from x in Context.DetectionTable
					where x.InsertDateTime > lastUpdate
					select x)
					.AsQueryable();
		}

		public IQueryable<DetectionResult> CalculateAVG()
		{
			var detectionList = ReadDetection();
			var item = (from x in detectionList
						group x by new { x.TypeOfFrequency } into grp
						select new DetectionResult
						{
							TypeOfFrequency = grp.Key.TypeOfFrequency,
							Average = grp.Average(x => x.Value)
						})
						.OrderBy(x => x.TypeOfFrequency).AsQueryable();
			return item;
		}

		public IQueryable<DetectionResult> CalculateAVG(IQueryable<DetectionEntity> detectionList)
		{
			var item = (from x in detectionList
						group x by new { x.TypeOfFrequency } into grp
						select new DetectionResult
						{
							TypeOfFrequency = grp.Key.TypeOfFrequency,
							Average = grp.Average(x => x.Value)
						})
						.OrderBy(x => x.TypeOfFrequency).AsQueryable();
			return item;
		}

		public IQueryable<Detection> CalculateStandardDerivation()
		{
			var detectionList = ReadDetection();
			var average = CalculateAVG(detectionList);
			List<Detection> resultList = new List<Detection>();
			object locker = new object();

			Parallel.ForEach(average, x =>
				{
					IQueryable<DetectionEntity> entity = (from y in detectionList
														  where y.TypeOfFrequency == x.TypeOfFrequency
														  select y).AsQueryable();
					var count = entity.Count();

					var sommatoria = entity.Sum(z => Math.Pow((z.Value - x.Average), 2));
					var diff = Math.Sqrt((sommatoria / count));

					lock (locker)
					{
						resultList.Add(new Detection { TypeOfFrequency = x.TypeOfFrequency, StandardDerivation = diff });
					}
				});

			return resultList.AsQueryable();
		}



	}
}
