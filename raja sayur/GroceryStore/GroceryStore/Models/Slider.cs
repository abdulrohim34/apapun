using GroceryStore.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ModernHttpClient;

namespace GroceryStore.Models
{
    public class Slider
    {
        public int id { get; set; }
        public string image { get; set; }

        public static async Task<SliderResponse> GetSliders()
        {
            SliderResponse generalResponse;
            using (HttpClient httpClient = new HttpClient(new NativeMessageHandler()))
            {
                var response = await httpClient.GetAsync(Config.GetSlider);
                var json = await response.Content.ReadAsStringAsync();
                generalResponse = JsonConvert.DeserializeObject<SliderResponse>(json);
            }
            return generalResponse;
        }
    }

    public class SliderResponse
    {
        public int status { get; set; }
        public string message { get; set; }
        public IList<Slider> data { get; set; }
    }
}
