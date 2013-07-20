using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class FlickerApi
    {
        public WebClient wc;

        public string GetImage(string mood)
        {
            string url;

            switch (mood)
            {
                case "HAPPY":
                    url = "http://ycpi.api.flickr.com/services/rest/?method=flickr.photos.search&api_key=95fe9d071e2410bf75ded644d8ccf757&tags=HAPPY&format=json&nojsoncallback=1&api_sig=c4828f6ac256321b9d0ec2dd021601bf";
                    break;
                case "SAD":
                    url = "http://ycpi.api.flickr.com/services/rest/?method=flickr.photos.search&api_key=95fe9d071e2410bf75ded644d8ccf757&tags=SAD&format=json&nojsoncallback=1&api_sig=593aa89d2cf912b57f55402d8d76f19f";
                    break;
                case "RELAXED":
                    url = "http://ycpi.api.flickr.com/services/rest/?method=flickr.photos.search&api_key=95fe9d071e2410bf75ded644d8ccf757&tags=RELAXED&format=json&nojsoncallback=1&api_sig=3dd36f104cafe0b62d10f701bab17cdb";
                    break;
                case "CRAZY":
                    url = "http://ycpi.api.flickr.com/services/rest/?method=flickr.photos.search&api_key=95fe9d071e2410bf75ded644d8ccf757&tags=mad&format=json&nojsoncallback=1&api_sig=40ac9387681b1d7c486c76d23278a842";
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
            wc = new WebClient();

            string fileName = "file.jpg";

            wc.DownloadFile(url, fileName);

            Color predominantColor = ColorMath.getDominantColor(new Bitmap(fileName));

            string rgb = predominantColor.Name;

            rgb = rgb.Remove(0, 2);

            return rgb;
        }
    }
}
