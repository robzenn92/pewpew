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
        private static int SongIndex { get; set; }
        private const string ApiKey = "201106acca1d39ef81f8ab793c51699a051e5080e";

        public StereoMoodApi()
        {
            SongIndex = 0;
        }

        public string GetSong(Enum mood)
        {
            var url = string.Format(
                "http://www.stereomood.com/api/search.json?api_key={0}&type=mood&q={1}",
                ApiKey,
                mood.ToString()
            );

            WebClient wc = new WebClient();

            var json_result = wc.DownloadString(url);

            var reader = new JsonTextReader(new StringReader(json_result));
            dynamic target = new JsonSerializer().Deserialize(reader);

            object jsonObject = new
            {
                Mood = mood.ToString(),
                Id = target.songs[SongIndex].id,
                Title = target.songs[SongIndex].title,
                Url = target.songs[SongIndex].audio_url,
                Artist = target.songs[SongIndex++].artist
            };

            return JsonConvert.SerializeObject(jsonObject);
        }
    }


}
