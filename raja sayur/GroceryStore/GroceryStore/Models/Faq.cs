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
    public class Faq
    {
        public string question { get; set; }
        public string answer { get; set; }

        public static async Task<FaqResponse> GetFaq()
        {
            FaqResponse FaqResponse;
            using (HttpClient httpClient = new HttpClient(new NativeMessageHandler()))
            {
                var response = await httpClient.GetAsync(Config.GetFaq);
                var json = await response.Content.ReadAsStringAsync();
                FaqResponse = JsonConvert.DeserializeObject<FaqResponse>(json);
            }

            return FaqResponse;
        }
    }

    public class FaqResponse
    {
        public int status { get; set; }
        public string message { get; set; }
        public IList<Faq> data { get; set; }
    }
}
