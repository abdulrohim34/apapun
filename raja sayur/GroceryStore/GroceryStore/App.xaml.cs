using System;
using DLToolkit.Forms.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using GroceryStore.Views;
using GroceryStore.Models;
using Plugin.FirebasePushNotification;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Push;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter;
using Device = Xamarin.Forms.Device;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace GroceryStore
{
    public partial class App : Application
    {
        public static User user = new User();
        public static string AndroidDeviceToken = "";
        public static string XamarinDeviceToken = "";
        public App()
        {
            InitializeComponent();
            FlowListView.Init();
            MainPage = new MasterTemplate();
            //MainPage = new CouponPage();
        }

        protected override void OnStart()
        {
            AppCenter.Start("android=805e2710-46e9-40b7-bd3f-4548d919856f;" + "ios=095273fe-f55f-4c93-ba7b-71d06f2c2859", typeof(Analytics), typeof(Crashes));
            // Handle when your app starts
            System.Diagnostics.Debug.WriteLine("MTG1");
            if (Device.RuntimePlatform == Device.Android || Device.RuntimePlatform == Device.iOS)
            //if (Device.OS == TargetPlatform.iOS)
            {
                CrossFirebasePushNotification.Current.OnTokenRefresh += (s, p) =>
                {
                    System.Diagnostics.Debug.WriteLine("MTG2");
                    System.Diagnostics.Debug.WriteLine($"TOKEN : {p.Token}");
                    XamarinDeviceToken = p.Token;
                    AndroidDeviceToken = p.Token;
                    Current.Properties["device_token"] = p.Token;
                };
                CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
                {

                    System.Diagnostics.Debug.WriteLine("Received");
                };
                CrossFirebasePushNotification.Current.OnNotificationOpened += (s, p) =>
                {
                    System.Diagnostics.Debug.WriteLine("Opened");
                    foreach (var data in p.Data)
                    {
                        System.Diagnostics.Debug.WriteLine($"{data.Key} : {data.Value}");
                    }
                };
            }
            else
            {
                Current.Properties["device_token"] = "";
            }
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
