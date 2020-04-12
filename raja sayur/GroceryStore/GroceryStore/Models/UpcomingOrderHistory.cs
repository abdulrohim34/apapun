using System;
using System.Collections.Generic;
using System.Text;

namespace GroceryStore.Models
{
    class UpcomingOrderHistory
    {
        public class OrderHistoryResponse
        {
            public int status { get; set; }
            public string message { get; set; }
            public IList<OrderHistory> data { get; set; }
        }
    }
}
