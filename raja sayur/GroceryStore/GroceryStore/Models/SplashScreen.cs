using System;
using GroceryStore.Views;
using Xamarin.Forms;

namespace GroceryStore.Models
{
    public class SplashScreen : ContentPage
    {
        Image splashImage;
        public SplashScreen()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            var sub = new StackLayout();
            splashImage = new Image
            {
                Source = "splash_screen.jpg",
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
            };
            //AbsoluteLayout.SetLayoutFlags(splashImage,
            //    AbsoluteLayoutFlags.PositionProportional);
            //AbsoluteLayout.SetLayoutBounds(splashImage,
            //new Rectangle(0.5, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

            sub.Children.Add(splashImage);

            this.BackgroundColor = Color.FromHex("#e96125");
            this.Content = sub;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //await splashImage.ScaleTo(80, 200);
            //await splashImage.ScaleTo(100, 1500, Easing.Linear);
            //await splashImage.ScaleTo(150, 1200, Easing.Linear);
            Application.Current.MainPage = new MainPage();
        }
    }
}
