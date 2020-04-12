using System;
using System.Collections.Generic;
using System.Text;

namespace GroceryStore.Models
{
    public class OrderHistory
    {
        public class UpcomingOrder
        {
            public string order_code { get; set; }
            public string order_total { get; set; }
            public string total_without_tax { get; set; }
            public string total_with_tax { get; set; }
            public int cart_id { get; set; }
            public string product_id { get; set; }
            public string product_variation_id { get; set; }
            public string quantity { get; set; }
            public string product_name { get; set; }
            public string image { get; set; }
            public string brand_name { get; set; }
            public string from_date { get; set; }
            public string to_date { get; set; }
            public string full_address { get; set; }
        }

        public class UpcomingOrderResponse
        {
            public int status { get; set; }
            public string message { get; set; }
            public IList<UpcomingOrder> data { get; set; }
        }

        public class PastOrder
        {
            public string order_code { get; set; }
            public string order_total { get; set; }
            public string total_without_tax { get; set; }
            public string total_with_tax { get; set; }
            public int cart_id { get; set; }
            public string product_id { get; set; }
            public string product_variation_id { get; set; }
            public string quantity { get; set; }
            public string product_name { get; set; }
            public string image { get; set; }
            public string brand_name { get; set; }
            public string from_date { get; set; }
            public string to_date { get; set; }
            public string full_address { get; set; }
        }

        public class PastOrderResponse
        {
            public int status { get; set; }
            public string message { get; set; }
            public IList<PastOrder> data { get; set; }
        }
    }
}
