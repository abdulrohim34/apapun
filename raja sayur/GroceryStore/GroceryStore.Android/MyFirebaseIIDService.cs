using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Firebase.Iid;

namespace GroceryStore.Droid
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    public class MyFirebaseIIDService : FirebaseInstanceIdService
    {
        const string TAG = "MyFirebaseIIDService";
        public override void OnTokenRefresh()
        {
            try
            {
                var refreshedToken = FirebaseInstanceId.Instance.Token;
                Log.Debug(TAG, "Refreshed token: " + refreshedToken);
                SendRegistrationToServer(refreshedToken);
            }
            catch (Exception ex)
            {
                GroceryStore.Helpers.Config.ErrorStore("MyFirebaseIIDService-OnTokenRefresh", ex.Message);
            }
        }
        async void SendRegistrationToServer(string token)
        {
            try
            {
                App.AndroidDeviceToken = token;
                App.Current.Properties["device_token"] = App.AndroidDeviceToken;
                await GroceryStore.Logic.UserLogic.UpdateDeviceToken(token);
                // Add custom implementation, as needed.
            }
            catch (Exception ex)
            {
                GroceryStore.Helpers.Config.ErrorStore("MyFirebaseIIDService-SendRegistrationToServer", ex.Message);
            }
        }
    }
}