﻿using Newtonsoft.Json;
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
            SongIndexes["THOUGHTFUL"] = 0;

            TotalPages = new Dictionary<string, int>();
            TotalPages["CONFUSED"] = 1;
            TotalPages["CONTEMPLATIVE"] = 4;
            TotalPages["FOCUSED"] = 1;
            TotalPages["THOUGHTFUL"] = 3;
        }

        public string GetSong(String mood)
        {
            switch (mood)
            {
                case "CONFUSED":
                    return this.GetSong(Moods.type.CONFUSED);
                case "CONTEMPLATIVE":
                    return this.GetSong(Moods.type.CONTEMPLATIVE);
                case "FOCUSED":
                    return this.GetSong(Moods.type.FOCUSED);
                case "THOUGHTFUL":
                    return this.GetSong(Moods.type.THOUGHTFUL);
                default:
                    return null;
            }
        }

        public string GetSong(Enum mood)
        {
            Random randomizer = new Random();
            byte soundcloud = 0;

            int page = (int)randomizer.Next(0, TotalPages[mood.ToString()]);

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

            int song = -1;

            while (soundcloud == 0)
            {
                song = (int)randomizer.Next(0, 19);

                if (song.CompareTo((Int32)target.total) >= 0)
                {
                    song = (int)randomizer.Next(0, (Int32)target.total);
                }

                soundcloud = (byte)target.songs[song].soundcloud;
            }

            object jsonObject = new
            {
                Mood = mood.ToString(),
                Title = target.songs[song].title,
                Url = target.songs[song].audio_url,
                Artist = target.songs[song].artist
            };

            SongIndexes[mood.ToString()] += 1;

            SongIndexes[mood.ToString()] = SongIndexes[mood.ToString()] % 20;

            return JsonConvert.SerializeObject(jsonObject);
        }
    }


}
