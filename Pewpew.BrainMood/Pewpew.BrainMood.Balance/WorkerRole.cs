using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Pewpew.BrainMood.DataManagement;

namespace Pewpew.BrainMood.Balance
{
	public class WorkerRole : RoleEntryPoint
	{

		public TableStorageContext TableContext { get; set; }

		public override void Run()
		{
			// This is a sample worker implementation. Replace with your logic.
			Trace.TraceInformation("Pewpew.BrainMood.Balance entry point called", "Information");

			TableContext = TableStorageContext.Instance;

			while (true)
			{
				var detections = QueueStorageContext.DequeueList();

				if (detections != null)
					TableContext.SaveDetection(detections, Guid.NewGuid());
			}
		}

		public override bool OnStart()
		{
			// Set the maximum number of concurrent connections 
			ServicePointManager.DefaultConnectionLimit = 12;

			// For information on handling configuration changes
			// see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

			return base.OnStart();
		}
	}
}
