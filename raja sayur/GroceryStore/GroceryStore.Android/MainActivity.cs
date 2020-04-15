using System;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using ImageCircle;
using ImageCircle.Forms.Plugin.Droid;
using CarouselView.FormsPlugin.Android;
using Plugin.CurrentActivity;
using Plugin.FirebasePushNotification;
using LabelHtml.Forms.Plugin.Droid;
using Acr.UserDialogs;
using Microsoft.AppCenter.Push;
using Lottie.Forms.Droid;
using Xfx;
using SuaveControls.MaterialForms.Android;

namespace GroceryStore.Droid
{
    [Activity(Label = "Sayur Bersama", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = false, ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            RendererInitializer.Init();
            XfxControls.Init();
            //global::Xamarin.Forms.Forms.SetFlags("FastRenderers_Experimental");
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            UserDialogs.Init(this);
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(enableFastRenderer: true);
            ImageCircleRenderer.Init();
            CarouselViewRenderer.Init();
            AnimationViewRenderer.Init();
#pragma warning disable CS0618 // Type or member is obsolete
            Push.SetSenderId("157557116902");
#pragma warning restore CS0618 // Type or member is obsolete
            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);
            HtmlLabelRenderer.Initialize();
            CrossCurrentActivity.Current.Init(this, savedInstanceState);
            CrossCurrentActivity.Current.Init(this, savedInstanceState);
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                Window.ClearFlags(WindowManagerFlags.TranslucentStatus);
                Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
                Window.SetStatusBarColor(Android.Graphics.Color.ParseColor("#088502"));
            }

            //This forces the custom renderers to be used
            //Android.Glide.Forms.Init();

            LoadApplication(new App());
            FirebasePushNotificationManager.ProcessIntent(this, Intent);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}