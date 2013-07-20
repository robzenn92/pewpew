using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using Pewpew.BrainMood.ObjectModel;
using Pewpew.BrainMood.ObjectModel.AzureModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pewpew.BrainMood.DataManagement
{
	public static class QueueStorageContext
	{
		static CloudQueue queue;

		static QueueStorageContext()
		{
			CloudStorageAccount account = CloudStorageAccount.Parse(
						   System.Configuration.ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString);
			CloudQueueClient queueClient = account.CreateCloudQueueClient();

			queue = queueClient.GetQueueReference(DetectionQueueName);
			queue.CreateIfNotExists();
		}


		#region Detection

		public const string DetectionQueueName = "DetectionQueue";

		public static void Enqueue(DetectionEntity detection)
		{
			string message = JsonConvert.SerializeObject(detection);
			queue.AddMessage(new CloudQueueMessage(message));
		}

		public static DetectionEntity Dequeue()
		{
			CloudQueueMessage message = queue.GetMessage();
			if (message == null)
				return null;

			DetectionEntity detection = JsonConvert.DeserializeObject<DetectionEntity>(message.AsString);
			queue.DeleteMessage(message);
			return detection;
		}

		#endregion Detection
	}
}
