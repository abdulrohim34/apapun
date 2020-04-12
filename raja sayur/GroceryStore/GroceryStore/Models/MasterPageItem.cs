using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;

namespace GroceryStore.Models
{
    public class MasterPageItem : INotifyPropertyChanged
    {
        public string Title { get; set; }
        public string Icon { get; set; }
        public Type TargetType { get; set; }


        public string ItemColor { get; set; }
        public bool Selected { get; set; }

        // I think you should not return "Color" type (for strong MVVM) but, for example, a value that you can convert in XAML with a IValueConverter...
        public Color TextColor
        {
            get
            {
                if (Selected)
                    return Color.White;
                else
                    return Color.Black;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
