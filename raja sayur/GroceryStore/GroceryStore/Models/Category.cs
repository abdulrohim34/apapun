using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GroceryStore.Models
{
    public class Category : INotifyPropertyChanged
    {
        private int id;
        public int Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        private string image;

        public string Image
        {
            get { return image; }
            set
            {
                image = value;
                OnPropertyChanged("Image");
            }
        }

        private string status;

        public string Status
        {
            get { return status; }
            set
            {
                status = value;
                OnPropertyChanged("Status");
            }
        }

        private string created_at;

        public string CreatedAt
        {
            get { return created_at; }
            set
            {
                CreatedAt = value;
                OnPropertyChanged("CreatedAt");
            }
        }

        private string updated_at;

        public string UpdatedAt
        {
            get { return updated_at; }
            set
            {
                UpdatedAt = value;
                OnPropertyChanged("UpdatedAt");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class CategoryResponse
    {
        public string status { get; set; }
        public string message { get; set; }
        public string fav_count { get; set; }
        public string cart_count { get; set; }
        public List<Category> data { get; set; }
        public IList<Slider> sliders { get; set; }
        public IList<Product> featured { get; set; }
        public IList<Product> quick_products { get; set; }
        public IList<Product> offered_products { get; set; }
        public decimal delivery_charges { get; set; }
        public decimal minimum_order_amount { get; set; }
    }
}
