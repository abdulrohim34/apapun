using System;
using System.Collections.Generic;
using System.Text;

namespace GroceryStore.Models
{
    public class City
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class CityResponse
    {
        public int status { get; set; }
        public string message { get; set; }
        public IList<City> data { get; set; }
    }
}
