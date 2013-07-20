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
		[DataMember(Name = "value")]
		public long Value { get; set; }

		[DataMember(Name = "typeOfFrequency")]
		public int TypeOfFrequency { get; set; }
	}
}
