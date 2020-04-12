using GroceryStore.Helpers;
using GroceryStore.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static GroceryStore.Models.OrderHistory;
using ModernHttpClient;

namespace GroceryStore.Logic
{
    public class OrderLogic
    {
        public async static Task<UpcomingOrderResponse> GetUpcomingOrders(int UserId)
        {
            UpcomingOrderResponse orderHistoryResponse;
            using (HttpClient httpClient = new HttpClient(new NativeMessageHandler()))
            {
                var json = await httpClient.GetAsync(String.Format(Config.GetUpcomingOrders, UserId));
                var response = await json.Content.ReadAsStringAsync();
                orderHistoryResponse = JsonConvert.DeserializeObject<UpcomingOrderResponse>(response);
            }
            return orderHistoryResponse;
        }

        public async static Task<PastOrderResponse> GetPastOrders(int UserId)
        {
            PastOrderResponse orderHistoryResponse;
            using (HttpClient httpClient = new HttpClient(new NativeMessageHandler()))
            {
                var json = await httpClient.GetAsync(String.Format(Config.GetPastOrders, UserId));
                var response = await json.Content.ReadAsStringAsync();
                orderHistoryResponse = JsonConvert.DeserializeObject<PastOrderResponse>(response);
            }
            return orderHistoryResponse;
        }
    }
}
