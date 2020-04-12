using System;
using System.Collections.Generic;
using System.Text;

namespace GroceryStore.Models
{
    public class RegisterResponse
    {
        public string status { get; set; }
        public string message { get; set; }
        public string cart_count { get; set; }
        public string fav_count { get; set; }
        public User data { get; set; }
        public UserAddress user_address { get; set; }
        public string refer_message { get; set; }
    }
}
