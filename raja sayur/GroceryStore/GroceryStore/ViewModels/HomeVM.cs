using GroceryStore.Models;
using GroceryStore.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace GroceryStore.ViewModels
{
    public class HomeVM : INotifyPropertyChanged
    {
        public CategoryCommand CategoryCommand { get; set; }
        public static string MyCartCounter;
        public static string MyFavCounter;

        public string cartCounter;
        public string CartCounter
        {
            get { return cartCounter; }
            set { cartCounter = value; OnPropertyChanged("CartCounter"); }
        }

        private Category category;

        public Category Category
        {
            get { return category; }
            set
            {
                category = value;
                OnPropertyChanged("Category");
            }
        }

        private ObservableCollection<Product> featureds;
        public ObservableCollection<Product> Featureds
        {
            get { return featureds; }
            set { featureds = value; OnPropertyChanged("Featureds"); }
        }

        private ObservableCollection<Product> quicks;
        public ObservableCollection<Product> Quicks
        {
            get { return quicks; }
            set { quicks = value; OnPropertyChanged("Quicks"); }
        }

        private ObservableCollection<Product> offered;
        public ObservableCollection<Product> Offered
        {
            get { return offered; }
            set { offered = value; OnPropertyChanged("Offered"); }
        }

        private int Id;
        public int id
        {
            get { return Id; }
            set
            {
                Id = value;
                Category = new Category()
                {
                    Id = this.id,
                    Name = this.name,
                    Image = this.image,
                    Status = this.status,
                };
                OnPropertyChanged("id");
            }
        }

        private string Name;

        public string name
        {
            get { return Name; }
            set
            {
                Name = value;
                Category = new Category()
                {
                    Id = this.id,
                    Name = this.name,
                    Image = this.image,
                    Status = this.status,
                };
                OnPropertyChanged("name");
            }
        }

        private Product featured;

        public Product Featured
        {
            get { return featured; }
            set { featured = value; OnPropertyChanged("Featured"); }
        }


        private string Image;

        public string image
        {
            get { return Image; }
            set
            {
                Image = value;
                Category = new Category()
                {
                    Id = this.id,
                    Name = this.name,
                    Image = this.image,
                    Status = this.status,
                };
                OnPropertyChanged("image");
            }
        }

        private string Status;

        public string status
        {
            get { return Status; }
            set
            {
                Status = value;
                Category = new Category()
                {
                    Id = this.id,
                    Name = this.name,
                    Image = this.image,
                    Status = this.status,
                };
                OnPropertyChanged("status");
            }
        }


        public HomeVM()
        {
            Category = new Category();
            CategoryCommand = new CategoryCommand(this);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public void GetProduct()
        {

        }
    }
}
