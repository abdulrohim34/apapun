using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroceryStore.Helpers;
using GroceryStore.Models;
using GroceryStore.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GroceryStore.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NotificationsPage : ContentPage
	{
        string _pageTitle = "Notifications";
        //public ObservableCollection<Notifications> notifications;
        public NotificationVM ViewModel;
		public NotificationsPage ()
		{
			InitializeComponent ();
            ViewModel = new NotificationVM();
            BindingContext = ViewModel;
		}

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Send((App)Application.Current, "NavigationBar", _pageTitle);
            try
            {
                if (Application.Current.Properties.ContainsKey("user_id"))
                {
                    Config.ShowDialog();
                    var response = await Notifications.GetNotification(int.Parse(Application.Current.Properties["user_id"].ToString()));
                    if (response.status == 200)
                    {
                        if (response.data.Any())
                        {
                            emptyContent.IsVisible = false;
                            mainContent.IsVisible = true;
                            ViewModel.NotificationList = new ObservableCollection<Notifications>(response.data);
                        }
                        else
                        {
                            EmptyNotifications();
                        }
                    }
                    else
                    {
                        EmptyNotifications();
                    }
                    Config.HideDialog();
                }
                else
                {
                    EmptyNotifications();
                }
            }
            catch (Exception ex)
            {
                Config.ErrorStore("NotificationsPage-OnAppearing", ex.Message);
                Config.HideDialog();
                EmptyNotifications();
                Config.ErrorSnackbarMessage(Config.ApiErrorMessage);
            }
        }
        
        void EmptyNotifications()
        {
            Config.HideDialog();
            emptyContent.IsVisible = true;
            //var size = Device.GetNamedSize(NamedSize.Large, typeof(Label));
            //Label emptyCart = new Label()
            //{
            //    Text = ValidationMessages.EmptyNotifications,
            //    FontAttributes = FontAttributes.Bold,
            //    HorizontalOptions = LayoutOptions.CenterAndExpand,
            //    VerticalOptions = LayoutOptions.CenterAndExpand,
            //    FontSize = size,
            //};
            //Content = emptyCart;
        }
    }
}