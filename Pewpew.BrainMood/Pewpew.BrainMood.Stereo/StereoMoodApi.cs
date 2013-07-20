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
            SongIndexes["HAPPY"] = 0;
            SongIndexes["SAD"] = 0;
            SongIndexes["RELAXED"] = 0;
            SongIndexes["CRAZY"] = 0;

            TotalPages = new Dictionary<string, int>();
            TotalPages["HAPPY"] = 76;
            TotalPages["SAD"] = 67;
            TotalPages["RELAXED"] = 3;
            TotalPages["CRAZY"] = 19;
        }

        public string GetSong(String mood) {
            switch (mood)
            {
                case "HAPPY":
                    return this.GetSong(Moods.type.HAPPY);
                case "SAD":
                    return this.GetSong(Moods.type.SAD);
                case "RELAXED":
                    return this.GetSong(Moods.type.RELAXED);
                case "CRAZY":
                    return this.GetSong(Moods.type.CRAZY);
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
