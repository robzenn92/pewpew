using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pewpew.BrainMood.ObjectModel.ServiceModel
{
	public class MoodRequestDTO
	{
		/// <summary>
		/// Dettagli canzone
		/// </summary>
		public string SongJSON { get; set; }

		/// <summary>
		/// Dettagli immagini
		/// </summary>
		public string ImageJSON { get; set; }
	}
}
