using Pewpew.BrainMood.DataManagement;
using Pewpew.BrainMood.Flickr;
using Pewpew.BrainMood.Light;
using Pewpew.BrainMood.ObjectModel.ServiceModel;
using Pewpew.BrainMood.Stereo;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Pewpew.BrainMood.Services.Controllers
{
    public class MoodController : ApiController
    {

		public Calculator CalculatorProp { get; set; }

		public MoodController()
		{
			CalculatorProp = new Calculator();
		}

		public MoodRequestDTO Get()
		{
			var averages = CalculatorProp.CalculateAVG().ToList();
			var mood = CalculatorProp.GetMood(averages.AsQueryable());

			string songJSON = new StereoMoodApi().GetSong(mood);

			var flicker = new FlickerApi();
			string imageURL = flicker.GetImage(mood.ToString());
			string color = flicker.GetColor(imageURL);

			var image = new
			{
				imageURL = imageURL,
				color = color
			};

			CacheManager.AddOrUpdate("lastupdate", DateTime.Now);

            LightController lightController = new LightController();

            lightController.SetColorLamps(image.color);

			return new MoodRequestDTO()
			{
				SongJSON = songJSON,
				ImageJSON = Newtonsoft.Json.JsonConvert.SerializeObject(image),
			};

		}

    }
}
