using System;
using System.Collections.Generic;
using System.Text;

namespace GroceryStore.Models
{
    public class Apartment
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class ApartmentResponse
    {
        public int status { get; set; }
        public string message { get; set; }
        public IList<Apartment> data { get; set; }
    }
}
