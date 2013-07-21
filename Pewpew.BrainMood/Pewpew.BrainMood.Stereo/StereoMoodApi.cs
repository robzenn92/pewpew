using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Pewpew.BrainMood.Stereo
{
	public class StereoMoodApi
	{
		private static Dictionary<string, int> SongIndexes { get; set; }
		private static Dictionary<string, int> TotalPages { get; set; }
		private const string ApiKey = "201106acca1d39ef81f8ab793c51699a051e5080e";

		public StereoMoodApi()
		{
			SongIndexes = new Dictionary<string, int>();
			SongIndexes["CONFUSED"] = 0;
			SongIndexes["CONTEMPLATIVE"] = 0;
			SongIndexes["FOCUSED"] = 0;
			SongIndexes["TOUGHTFUL"] = 0;

			TotalPages = new Dictionary<string, int>();
			TotalPages["CONFUSED"] = 76;
			TotalPages["CONTEMPLATIVE"] = 67;
			TotalPages["FOCUSED"] = 3;
			TotalPages["TOUGHTFUL"] = 19;
		}

		public string GetSong(String mood) {
			switch (mood)
			{
				case "CONFUSED":
					return this.GetSong(Moods.type.CONFUSED);
				case "CONTEMPLATIVE":
					return this.GetSong(Moods.type.CONTEMPLATIVE);
				case "FOCUSED":
					return this.GetSong(Moods.type.FOCUSED);
				case "TOUGHTFUL":
					return this.GetSong(Moods.type.TOUGHTFUL);
				default:
					return null;
			}
		}

		public string GetSong(Enum mood)
		{
			Random randomizer = new Random();

			int page = randomizer.Next(0, TotalPages[mood.ToString()]);

			var url = string.Format(
				"http://www.stereomood.com/api/search.json?api_key={0}&type=mood&q={1}&page={2}",
				ApiKey,
				mood.ToString(),
				page
			);

			WebClient wc = new WebClient();

			var json_result = wc.DownloadString(url);

			var reader = new JsonTextReader(new StringReader(json_result));

			dynamic target = new JsonSerializer().Deserialize(reader);

			object jsonObject = new
			{
				Mood = mood.ToString(),
				Title = target.songs[SongIndexes[mood.ToString()]].title,
				Url = target.songs[SongIndexes[mood.ToString()]].audio_url,
				Artist = target.songs[SongIndexes[mood.ToString()]].artist
			};

			SongIndexes[mood.ToString()] += 1;

			SongIndexes[mood.ToString()] = SongIndexes[mood.ToString()] % 20;

			return JsonConvert.SerializeObject(jsonObject);
		}
	}


}
