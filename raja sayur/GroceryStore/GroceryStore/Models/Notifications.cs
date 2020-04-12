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
    public class Notifications
    {
        public string title { get; set; }
        public string description { get; set; }
        public string notification_type { get; set; }

        public static async Task<NotificationResponse> GetNotification(int UserId)
        {
            NotificationResponse notifications;
            using (HttpClient httpClient = new HttpClient(new NativeMessageHandler()))
            {
                var url = string.Format(Config.GetNotification, UserId);
                var response = await httpClient.GetAsync(url);
                var json = await response.Content.ReadAsStringAsync();
                notifications = JsonConvert.DeserializeObject<NotificationResponse>(json);
            }
            return notifications;
        }
    }

    public class NotificationResponse
    {
        public int status { get; set; }
        public string message { get; set; }
        public IList<Notifications> data { get; set; }
    }
}
