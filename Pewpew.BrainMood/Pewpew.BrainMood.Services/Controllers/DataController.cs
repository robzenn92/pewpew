using Pewpew.BrainMood.DataManagement;
using Pewpew.BrainMood.ObjectModel;
using Pewpew.BrainMood.ObjectModel.AzureModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Pewpew.BrainMood.Services.Controllers
{
    public class DataController : ApiController
    {


		public void Get(IEnumerable<Detection> datas)
		{
			Guid sequence = Guid.NewGuid();
			QueueStorageContext.EnqueueList(datas.Select(x => new DetectionEntity()
				{
					Id = Guid.NewGuid(),
					SequenceId = sequence,
					TypeOfFrequency = x.TypeOfFrequency,
					Value = x.Value
				}));
		}


    }
}
