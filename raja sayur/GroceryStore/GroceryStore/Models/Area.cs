using System;
using System.Collections.Generic;
using System.Text;

namespace GroceryStore.Models
{
    public class Area
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class AreaResponse
    {
        public int status { get; set; }
        public string message { get; set; }
        public IList<Area> data { get; set; }
    }
}
