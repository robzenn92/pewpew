using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Pewpew.BrainMood.Flickr
{
	public class FlickerApi
	{
		public WebClient wc;

		public string GetImage(string mood)
		{
			string url;

			switch (mood)
			{
				case "FOCUSED":
					url = "http://ycpi.api.flickr.com/services/rest/?method=flickr.photos.search&api_key=d80965d3b782413eb5323c654e2ad100&tags=FOCUSED&format=json&nojsoncallback=1&api_sig=23aae1d19aae09a984c5b5c59b906ac1";
					break;
				case "THOUGHTFUL":
					url = "http://ycpi.api.flickr.com/services/rest/?method=flickr.photos.search&api_key=d80965d3b782413eb5323c654e2ad100&tags=THOUGHTFUL&format=json&nojsoncallback=1&api_sig=d21b27467c72f84dcd377358f54d75ac";
					break;
				case "CONTEMPLATIVE":
					url = "http://ycpi.api.flickr.com/services/rest/?method=flickr.photos.search&api_key=d80965d3b782413eb5323c654e2ad100&tags=CONTEMPLATIVE&format=json&nojsoncallback=1&api_sig=fac045711a2b39c5e2e8f6c734effa00";
					break;
				case "CONFUSED":
					url = "http://ycpi.api.flickr.com/services/rest/?method=flickr.photos.search&api_key=d80965d3b782413eb5323c654e2ad100&tags=CONFUSED&format=json&nojsoncallback=1&api_sig=8cc99d36d5654416a0a4624407a02743";
					break;
				default:
					return null;
			}

			var upload_mood = string.Format(url);

			wc = new WebClient();

			Random r = new Random();

			var json_result = wc.DownloadString(upload_mood);

			var reader = new JsonTextReader(new StringReader(json_result));

			dynamic target = new JsonSerializer().Deserialize(reader);

			var photo_list = new List<dynamic>();

			foreach (var item in target.photos.photo)
			{
				photo_list.Add(new
				{
					id = item.id,
					secret = item.secret,
					server = item.server,
					farm = item.farm,
				});
			}

			var random_photo = photo_list[r.Next(0, 100)];

			return string.Format("http://farm{0}.staticflickr.com/{1}/{2}_{3}_b.jpg",
				random_photo.farm,
				random_photo.server,
				random_photo.id,
				random_photo.secret
				);

		}

		public string GetColor(string url)
		{
			string fileName;
			string rgb;
			using (wc = new WebClient())
			{

				fileName = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/"), string.Format("file-{0}.jpg", Guid.NewGuid()));

				wc.DownloadFile(url, fileName);

				Color predominantColor = ColorMath.getDominantColor(new Bitmap(fileName));

				rgb = predominantColor.Name;
				rgb = rgb.Remove(0, 2);
			}

			//File.Delete(fileName);

			return rgb;
		}
	}
}
