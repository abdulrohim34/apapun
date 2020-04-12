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
    public class OrderDetail
    {
        Dictionary<string, string> StatusTitle;

        public OrderDetail()
        {
            StatusTitle = new Dictionary<string, string>();
            StatusTitle.Add("AC", "Pending");
            StatusTitle.Add("PN", "Pending");
            StatusTitle.Add("CM", "Delivered");
            StatusTitle.Add("FL", "UnDelivered");
            StatusTitle.Add("CL", "Canceled");
        }

        public int id { get; set; }
        public int cart_id { get; set; }
        public string order_date { get; set; }
        private string _status;

        public string status
        {
            get { return StatusTitle[_status]; }
            set { _status = value; }
        }

        private string _status_color;

        public string status_color
        {
            get { if (status == "CM") { return "Green"; } else { return "Red"; } }
            set { _status_color = value; }
        }

        public bool IsVisible { get; set; }

        public async static Task<OrderDetailResponse> GetOrderDetail(int CartId, string status)
        {
            OrderDetailResponse orderDetailResponse;
            using (HttpClient httpClient = new HttpClient(new NativeMessageHandler()))
            {
                var json = await httpClient.GetAsync(String.Format(Config.GetOrderDetail, CartId, status));
                var response = await json.Content.ReadAsStringAsync();
                orderDetailResponse = JsonConvert.DeserializeObject<OrderDetailResponse>(response);
            }
            return orderDetailResponse;
        }
    }

    public class OrderDetailResponse
    {
        public int status { get; set; }
        public string message { get; set; }
        public IList<OrderDetail> data { get; set; }
    }
}
