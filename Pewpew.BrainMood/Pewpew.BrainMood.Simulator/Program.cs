using Pewpew.BrainMood.DataManagement;
using Pewpew.BrainMood.ObjectModel;
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

			string deviceID = "device";
			Random rnd = new Random();

			for (int i = 0; i < 10; i++)
			{
				List<Detection> detection = new List<Detection>();
				for (int j = 0; j < 4; j++)
					detection.Add(new Detection()
					{
						TypeOfFrequency = j,
						Value = (j+1) * rnd.Next((j+1) * 10000),
					});

				Calculator calc = new Calculator();
				calc.SaveDetection(detection, Guid.NewGuid(), deviceID);
				Thread.Sleep(1000);
				Console.WriteLine("{0} item", i);
			}

			Calculator calculator = new Calculator();
			Console.WriteLine("start average - {0}", DateTime.Now.ToString("dd/MM/YYYY HH:mm:sss"));
			var results = calculator.CalculateAVG(deviceID).ToList();
			Console.WriteLine("end average - {0}", DateTime.Now.ToString("dd/MM/YYYY HH:mm:sss"));

			foreach (var item in results)
			{
				Console.WriteLine("Type: {0} - Average: {1}", item.TypeOfFrequency, item.Average);
			}

			Console.WriteLine("start diff - {0}", DateTime.Now.ToString("dd/MM/YYYY HH:mm:sss"));
			var diff = calculator.CalculateStandardDerivation(deviceID).ToList();
			Console.WriteLine("end diff - {0}", DateTime.Now.ToString("dd/MM/YYYY HH:mm:sss"));

			foreach (var item in diff)
			{
				Console.WriteLine("Type: {0} - Diff: {1}", item.TypeOfFrequency, item.StandardDerivation);
			}

			Console.ReadLine();
		}
	}
}
