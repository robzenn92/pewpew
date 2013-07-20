using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Pewpew.BrainMood.ObjectModel.ServiceModel
{
	[DataContract]
	public class DetectionDTO
	{
		[DataMember(Name = "meditation")]
		public long Meditation { get; set; }

		[DataMember(Name = "attention")]
		public long Attention { get; set; }
	}
}
