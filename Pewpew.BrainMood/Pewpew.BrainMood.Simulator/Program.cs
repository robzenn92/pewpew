using Pewpew.BrainMood.DataManagement;
using Pewpew.BrainMood.ObjectModel;
using Pewpew.BrainMood.ObjectModel.AzureModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Pewpew.BrainMood.Simulator
{
	class Program
	{
		static void Main(string[] args)
		{
			Random rnd = new Random();

			for (int k = 0; k < 2; k++)
			{
				for (int i = 0; i < 10; i++)
				{
					List<Detection> detection = new List<Detection>();
					for (int j = 0; j < 2; j++)
						detection.Add(new Detection()
						{
							TypeOfFrequency = j,
							Value = (j + 1) * rnd.Next((j + 1) * 10000),
						});

					Guid sequenceId = Guid.NewGuid();
					List<DetectionEntity> detectEntityList = new List<DetectionEntity>();
					detectEntityList.AddRange(detection.Select(x => new DetectionEntity()
					{
						Id = Guid.NewGuid(),
						SequenceId = sequenceId,
						TypeOfFrequency = x.TypeOfFrequency,
						Value = x.Value,
					}));

					Calculator calc = new Calculator();
					QueueStorageContext.EnqueueList(detectEntityList);
					Thread.Sleep(5000);
					Console.WriteLine("{0} item", i);
				}

				Calculator calculator = new Calculator();
				Console.WriteLine("start average - {0}", DateTime.Now.ToString("dd/MM/YYYY HH:mm:sss"));
				var results = calculator.CalculateAVG().ToList();
				Console.WriteLine("end average - {0}", DateTime.Now.ToString("dd/MM/YYYY HH:mm:sss"));

				foreach (var item in results)
				{
					Console.WriteLine("Type: {0} - Average: {1}", item.TypeOfFrequency, item.Average);
				}

				Console.WriteLine("start diff - {0}", DateTime.Now.ToString("dd/MM/YYYY HH:mm:sss"));
				var diff = calculator.CalculateStandardDerivation().ToList();
				Console.WriteLine("end diff - {0}", DateTime.Now.ToString("dd/MM/YYYY HH:mm:sss"));

				foreach (var item in diff)
				{
					Console.WriteLine("Type: {0} - Diff: {1}", item.TypeOfFrequency, item.StandardDerivation);
				}

				CacheManager.AddOrUpdate("lastupdate", DateTime.Now);

				Console.ReadLine();
			}
		}
	}
}
