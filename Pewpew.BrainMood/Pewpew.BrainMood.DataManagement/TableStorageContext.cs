using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Table.DataServices;
using Pewpew.BrainMood.ObjectModel.AzureModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pewpew.BrainMood.DataManagement
{
	public class TableStorageContext : TableServiceContext
	{

		private TableStorageContext(CloudTableClient client)
			: base(client) { }

		private static TableStorageContext _instance;

		public static TableStorageContext Instance
		{
			get
			{
				if (_instance == null)
					_instance = new TableStorageContext(CloudClient);
				return _instance;
			}
		}

		private static CloudTableClient cloudClient;
		public static CloudTableClient CloudClient
		{
			get
			{
				if (cloudClient == null)
				{
					CloudStorageAccount account = CloudStorageAccount.Parse(
						   System.Configuration.ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString);
					cloudClient = account.CreateCloudTableClient();

					var table = cloudClient.GetTableReference(DetectionTableName);
					table.CreateIfNotExists();
				}
				return cloudClient;
			}
		}

		#region Detection

		private const string DetectionTableName = "DetectionTable";

		/// <summary>
		/// Gestisce la tabella delle rilevazioni
		/// </summary>
		public IQueryable<DetectionEntity> DetectionTable
		{
			get
			{
				return this.CreateQuery<DetectionEntity>(DetectionTableName);
			}
		}

		public void AddDetection(DetectionEntity item)
		{
			this.AddObject(DetectionTableName, item);
		}

		#endregion Detection

	}
}
