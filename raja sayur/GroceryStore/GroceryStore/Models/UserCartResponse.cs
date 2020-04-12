using System;
namespace GroceryStore.Models
{
    public class UserCartResponse
    {
        public int status { get; set; }
        public string message { get; set; }
        public int cart_count { get; set; }
        public int fav_count { get; set; }
    }
}
