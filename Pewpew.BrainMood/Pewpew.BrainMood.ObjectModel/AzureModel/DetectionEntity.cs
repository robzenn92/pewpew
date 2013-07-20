using Microsoft.WindowsAzure.Storage.Table.DataServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pewpew.BrainMood.ObjectModel.AzureModel
{
	public class DetectionEntity : TableServiceEntity
	{
		public DetectionEntity() { }

		public DetectionEntity(Guid id, Guid sequenceId)
		{
			Id = id;
			SequenceId = sequenceId;
		}

		public Guid Id
		{
			get { return Guid.Parse(RowKey); }
			set { RowKey = value.ToString(); }
		}

		public Guid SequenceId
		{
			get { return Guid.Parse(PartitionKey); }
			set { PartitionKey = value.ToString(); }
		}

		public long Value { get; set; }

		public int TypeOfFrequency { get; set; }

		public DateTime InsertDateTime { get; set; }

	}
}
