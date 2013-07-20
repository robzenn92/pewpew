using Pewpew.BrainMood.DataManagement;
using Pewpew.BrainMood.ObjectModel.ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Pewpew.BrainMood.Services.Controllers
{
	public class GraphController : ApiController
	{

		public DetectionDTO Get()
		{
			return GraphManager.GetLastDetection();
		}
	}
}
