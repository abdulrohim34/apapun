using GroceryStore.Helpers;
using GroceryStore.Models;
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
    public partial class ForgotPasswordPage : ContentPage
    {
        public ForgotPasswordPage()
        {
            InitializeComponent();
        }

        private async void submit_Clicked(object sender, EventArgs e)
        {
            Config.ShowDialog();
            try
            {
                if (string.IsNullOrWhiteSpace(email.Text))
                {
                    Config.HideDialog();
                    Config.SnackbarMessage(ValidationMessages.MobileNumberRequired);
                }
                else
                {
                    Dictionary<string, string> valuePairs = new Dictionary<string, string>();
                    valuePairs.Add("email", email.Text);
                    valuePairs.Add("user_type", "user");

                    var response = await User.ForgotPassword(valuePairs);
                    if (response.status == 200)
                    {
                        Config.HideDialog();
                        Config.SnackbarMessage(response.message);
                        await Navigation.PopModalAsync();
                    }
                    else
                    {
                        Config.HideDialog();
                        await DisplayAlert("Error", response.message, "Ok");
                    }
                }
            }
            catch (Exception)
            {
                Config.HideDialog();
                await DisplayAlert("Alert", Config.ApiErrorMessage, "Ok");
            }
        }

        private void CrossPage_Tapped(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}