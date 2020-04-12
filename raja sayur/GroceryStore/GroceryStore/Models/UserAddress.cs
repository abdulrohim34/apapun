using System;
using System.Collections.Generic;
using System.Text;

namespace GroceryStore.Models
{
    public class UserAddress
    {
        public int id { get; set; }
        public int user_id { get; set; }
        public string name { get; set; }
        public string mobile_number { get; set; }
        public string house_no { get; set; }
        public string apartment_name { get; set; }
        public string landmark_details { get; set; }
        public string area_details { get; set; }
        public string street_details { get; set; }
        public string address_type { get; set; }
        public string full_address { get; set; }
        public int default_address { get; set; }
        public string country { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public string pincode { get; set; }

        private bool _isDefault;
        public bool IsDefault
        {
            get
            {
                if (default_address == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            set { _isDefault = value; }
        }
    }

    public class UserAddressResponse
    {
        public int status { get; set; }
        public string message { get; set; }
        public IList<UserAddress> data { get; set; }
    }

    public class DefaultAddressResponse
    {
        public int status { get; set; }
        public string message { get; set; }
        public IList<UserAddress> data { get; set; }
    }

    public class GeneralResponse
    {
        public int status { get; set; }
        public string message { get; set; }
    }
}
