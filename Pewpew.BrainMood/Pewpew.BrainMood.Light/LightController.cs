using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Pewpew.BrainMood.Light
{
    public class LightController
    {
        public List<LightLamp> Lamps { get; set; }

        public void SetColorLamps(string rgb)
        {
            int HUEColor = this.ConvertRGBToHUE(rgb);

            WebClient wc = new WebClient();

            dynamic[] light = new dynamic[3];
            for (int i = 0; i < light.Length; i++)
            {
                light[i] = new ExpandoObject();
                light[i].on = true;
                light[i].sat = 255;
                light[i].bri = 125;
            }

            light[0].hue = HUEColor;
            light[1].hue = (HUEColor - 1000) < 0 ? 64535 : HUEColor - 1000;
            light[2].hue = (HUEColor + 1000) > 65535 ? 1000 : HUEColor + 1000;

            for (int i = 1; i <= light.Length; i++)
            {
                var json_result = wc.UploadString("http://10.1.3.7/api/coduzpewpew/lights/" + i + "/state", "PUT", JsonConvert.SerializeObject(light[i - 1]));
            }

        }

        public int ConvertRGBToHUE(string RGB)
        {
            int red = int.Parse(RGB.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            int green = int.Parse(RGB.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            int blue = int.Parse(RGB.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

            Color color = Color.FromArgb(255, red, green, blue);

            float f = color.GetHue();

            return (int)((f / 360) * (65535));

        }

    }
}

