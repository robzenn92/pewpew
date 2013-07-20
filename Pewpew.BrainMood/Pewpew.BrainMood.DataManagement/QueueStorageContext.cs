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

		public const string DetectionQueueName = "detectionqueue";

		public static void EnqueueList(IEnumerable<DetectionEntity> detections)
		{
			string message = JsonConvert.SerializeObject(detections);
			queue.AddMessage(new CloudQueueMessage(message));
		}

		public static IEnumerable<DetectionEntity> DequeueList()
		{
			CloudQueueMessage message = queue.GetMessage();
			if (message == null)
				return null;

			IEnumerable<DetectionEntity> detections = JsonConvert.DeserializeObject<IEnumerable<DetectionEntity>>(message.AsString);
			queue.DeleteMessage(message);
			return detections;
		}

		#endregion Detection
	}
}
