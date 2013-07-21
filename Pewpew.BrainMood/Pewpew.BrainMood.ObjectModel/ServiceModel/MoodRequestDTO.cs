using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Pewpew.BrainMood.ObjectModel.ServiceModel
{
	[DataContract]
	public class MoodRequestDTO
	{
		[DataMember(Name = "mood")]
		public string Mood { get; set; }

		[DataMember(Name = "songUrl")]
		public string SongUrl { get; set; }

		[DataMember(Name = "artist")]
		public string Artist { get; set; }

		[DataMember(Name = "title")]
		public string Title { get; set; }

		[DataMember(Name = "imageURL")]
		public string ImageURL { get; set; }

		[DataMember(Name = "color")]
		public string Color { get; set; }
	}
}
