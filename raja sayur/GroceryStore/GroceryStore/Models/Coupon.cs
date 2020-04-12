using System;
using System.Collections.Generic;

namespace GroceryStore.Models
{
    public class Coupon
    {
        public int id { get; set; }
        public string coupon_code { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string terms { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public string min_amount { get; set; }
        public string max_amount { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
        public string discount_type { get; set; }
        public string amount_type { get; set; }
        public string discount_value { get; set; }
        public int no_of_applies { get; set; }
        public string status { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
    }

    public class CouponResponse
    {
        public int status { get; set; }
        public string message { get; set; }
        public IList<Coupon> data { get; set; }
    }
}
