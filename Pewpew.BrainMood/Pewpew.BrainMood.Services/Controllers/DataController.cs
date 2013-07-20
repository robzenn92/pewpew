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


		public void Get([FromBody]long att, [FromBody]long med)
		{
			Guid sequence = Guid.NewGuid();
			DetectionEntity attention = new DetectionEntity()
			{
				Id = Guid.NewGuid(),
				SequenceId = sequence,
				TypeOfFrequency = 0,
				Value = att,
				InsertDateTime = DateTime.Now.Ticks,
			};
			DetectionEntity meditetion = new DetectionEntity()
			{
				Id = Guid.NewGuid(),
				SequenceId = sequence,
				TypeOfFrequency = 1,
				Value = med,
				InsertDateTime = DateTime.Now.Ticks,
			};
			List<DetectionEntity> datas = new List<DetectionEntity>();
			datas.Add(attention);
			datas.Add(meditetion);
			QueueStorageContext.EnqueueList(datas);
		}


    }
}
