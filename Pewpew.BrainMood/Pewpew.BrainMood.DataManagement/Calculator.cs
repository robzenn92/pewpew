using Pewpew.BrainMood.ObjectModel;
using Pewpew.BrainMood.ObjectModel.AzureModel;
using System;
using System.Collections.Generic;
using System.Linq;
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


		private IQueryable<DetectionEntity> ReadDetection(string deviceIdentificator)
		{
			IEnumerable<Guid> sequenceList = (from x in SequenceManager.GetSequence(deviceIdentificator)
											  select x)
												.ToList();
			//SequenceManager.Clear(deviceIdentificator);

			List<DetectionEntity> detectionList = new List<DetectionEntity>();
			foreach (Guid seq in sequenceList)
				detectionList.AddRange(
					from x in Context.DetectionTable
					where x.PartitionKey == seq.ToString()
					select x);

			return detectionList.AsQueryable<DetectionEntity>();

			//return (from x in Context.DetectionTable
			//		where sequenceList.Where(y => y == x.SequenceId).Count() > 0
			//		select x).AsQueryable<DetectionEntity>();


		}


		public void SaveDetection(IEnumerable<Detection> detection, Guid sequenceId, string deviceIdentificator)
		{
			List<DetectionEntity> detectionList = new List<DetectionEntity>();
			foreach (Detection item in detection)
			{
				detectionList.Add(new DetectionEntity()
					{
						Id = Guid.NewGuid(),
						SequenceId = sequenceId,
						Value = item.Value,
						TypeOfFrequency = item.TypeOfFrequency,
					});
			}

			foreach (var item in detectionList)
			{
				Context.AddDetection(item);
			}
			Context.SaveChanges();
			SequenceManager.AddSequence(sequenceId, deviceIdentificator);
		}

		public IQueryable<DetectionResult> CalculateAVG(string deviceIdentificator)
		{
			var detectionList = ReadDetection(deviceIdentificator);
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

		public IQueryable<Detection> CalculateStandardDerivation(string deviceIdentificator)
		{
			var detectionList = ReadDetection(deviceIdentificator);
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
