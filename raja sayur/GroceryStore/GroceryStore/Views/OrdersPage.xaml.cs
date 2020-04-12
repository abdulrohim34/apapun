using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GroceryStore.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class OrdersPage : TabbedPage
	{
        string _pageTitle = "Orders";
        public static int move = 0;
        public OrdersPage(int moveId)
        {
            InitializeComponent();
            move = moveId;
            //var pages = Children.GetEnumerator();
            //if (moveId == 2)
            //{
            //    pages.MoveNext(); // First page
            //    pages.MoveNext(); // Second page
            //}
            //CurrentPage = pages.Current;
        }

        public OrdersPage()
        {
            InitializeComponent();
            //var pages = Children.GetEnumerator();
            //if (move == 2)
            //{
            //    pages.MoveNext(); // First page
            //    pages.MoveNext(); // Second page
            //}
            //CurrentPage = pages.Current;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Send((App)Application.Current, "NavigationBar", _pageTitle);
            //var pages = Children.GetEnumerator();
            //if (move == 2)
            //{
            //    pages.MoveNext(); // First page
            //    pages.MoveNext(); // Second page
            //}
            //CurrentPage = pages.Current;
        }
    }
}