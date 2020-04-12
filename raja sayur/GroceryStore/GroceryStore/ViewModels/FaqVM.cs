using GroceryStore.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace GroceryStore.ViewModels
{
    public class FaqVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private Faq _faq;

        public Faq Faq
        {
            get { return _faq; }
            set { _faq = value; OnPropertyChanged("Faq"); }
        }

        private ObservableCollection<Faq> _faqList;

        public ObservableCollection<Faq> FaqList
        {
            get { return _faqList; }
            set { _faqList = value; OnPropertyChanged("FaqList"); }
        }

    }
}
