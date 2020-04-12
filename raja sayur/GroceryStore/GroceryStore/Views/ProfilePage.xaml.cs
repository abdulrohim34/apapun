using GroceryStore.Helpers;
using GroceryStore.Models;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using GroceryStore.ViewModels;
using System.Text.RegularExpressions;
using ModernHttpClient;

namespace GroceryStore.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        string _pageTitle = "Edit Profile";
        public ProfilePage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            CustomNavigationBarVM.PageName = "profile";
            CustomNavigationBarVM.MenuIcon = "back.png";
            MessagingCenter.Send((App)Application.Current, "NavigationBar", _pageTitle);

            name.Text = Application.Current.Properties["name"].ToString();
            email.Text = Application.Current.Properties["email"].ToString();
            mobile_number.Text = Application.Current.Properties["mobile_number"].ToString();
            string profile = Application.Current.Properties["profile_picture"].ToString();
            if (CheckFileExist(profile))
            {
                profileImage.Source = profile;
            }
        }

        public bool CheckFileExist(string url)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "HEAD";

            bool exists;
            try
            {
                request.GetResponse();
                exists = true;
            }
            catch
            {
                exists = false;
            }
            return exists;
        }

        private async void updateProfileTap_Tapped(object sender, EventArgs e)
        {
            try
            {
                string pattern = "^[A-Za-z ]+$";
                Regex regex = new Regex(pattern);

                if (string.IsNullOrWhiteSpace(name.Text) || string.IsNullOrEmpty(name.Text))
                {
                    Config.ErrorSnackbarMessage(ValidationMessages.NameRequired);
                }

                if (regex.IsMatch(name.Text) == false)
                {
                    Config.ErrorSnackbarMessage(ValidationMessages.NameLetters);
                    return;
                }

                else if (string.IsNullOrWhiteSpace(email.Text) || string.IsNullOrEmpty(email.Text))
                {
                    Config.ErrorSnackbarMessage(ValidationMessages.EmailRequired);
                }
                else if (string.IsNullOrWhiteSpace(mobile_number.Text) || string.IsNullOrEmpty(mobile_number.Text))
                {
                    Config.ErrorSnackbarMessage(ValidationMessages.PhoneNumberRequired);
                }
                else if (mobile_number.Text.Length < 10)
                {
                    Config.ErrorSnackbarMessage(ValidationMessages.PhoneNumberMinimum);
                }
                else if (mobile_number.Text.Length > 10)
                {
                    Config.ErrorSnackbarMessage(ValidationMessages.PhoneNumberMaximum);
                }
                else
                {
                    Config.ShowDialog();
                    Dictionary<string, string> user = new Dictionary<string, string>();
                    user.Add("user_id", Application.Current.Properties["user_id"].ToString());
                    user.Add("name", name.Text);
                    user.Add("email", email.Text);

                    var response = await User.UpdateProfile(user);
                    if (response.status == "200")
                    {
                        Config.HideDialog();
                        Application.Current.Properties["name"] = response.data.name;
                        Application.Current.Properties["email"] = response.data.email;
                        Application.Current.Properties["order_message"] = response.refer_message;
                        if (response.user_address != null)
                        {
                            Application.Current.Properties["full_address"] = response.user_address.full_address;
                            Application.Current.Properties["address_type"] = response.user_address.address_type;
                        }
                        else
                        {
                            Application.Current.Properties["full_address"] = "";
                            Application.Current.Properties["address_type"] = "";
                        }
                        Config.SnackbarMessage(response.message);
                    }
                    else
                    {
                        Config.HideDialog();
                        Config.ErrorSnackbarMessage(response.message);
                    }
                }
            }
            catch (Exception ex)
            {
                Config.ErrorStore("ProfilePage-updateProfileTap_Tapped", ex.Message);
                Config.HideDialog();
                Config.ErrorSnackbarMessage(Config.ApiErrorMessage);
            }
        }

        private async void profileUploadTap_Tapped(object sender, EventArgs e)
        {
            try
            {
                await CrossMedia.Current.Initialize();

                var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
                var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);

                if (!CrossMedia.Current.IsPickPhotoSupported && !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await DisplayAlert("Error", "This is not supported on your device", "Ok");
                    return;
                }

                if (cameraStatus != PermissionStatus.Granted || storageStatus != PermissionStatus.Granted)
                {
                    var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Camera, Permission.Storage });
                    cameraStatus = results[Permission.Camera];
                    storageStatus = results[Permission.Storage];
                }

                if (cameraStatus == PermissionStatus.Granted && storageStatus == PermissionStatus.Granted)
                { //var mediaOptions = new StoreCameraMediaOptions()
                  //{
                  //    PhotoSize = PhotoSize.Medium
                  //};
                    var mediaOptions = new PickMediaOptions()
                    {
                        PhotoSize = PhotoSize.Medium
                    };

                    var selectedImageFile = await CrossMedia.Current.PickPhotoAsync(mediaOptions);

                    if (selectedImageFile == null)
                    {
                        //await DisplayAlert("Error", "There was an error when trying to get your image, please try again", "Ok");
                        return;
                    }

                    profileImage.Source = ImageSource.FromStream(() => selectedImageFile.GetStream());

                    var content = new MultipartFormDataContent();
                    content.Add(new StreamContent(selectedImageFile.GetStream()),
                        "\"profile_picture\"",
                        $"\"{selectedImageFile.Path}\"");
                    content.Add(new StringContent(Application.Current.Properties["user_id"].ToString()), "user_id");

                    using (var httpClient = new HttpClient(new NativeMessageHandler()))
                    {
                        Config.ShowDialog();
                        var uploadServiceBaseAddress = Config.UpdateProfile;
                        var httpResponseMessage = await httpClient.PostAsync(uploadServiceBaseAddress, content);
                        var json = await httpResponseMessage.Content.ReadAsStringAsync();
                        var response = JsonConvert.DeserializeObject<RegisterResponse>(json);
                        if (response.status == "200")
                        {
                            Application.Current.Properties["profile_picture"] = response.data.profile_picture;
                            Config.HideDialog();
                        }
                        else
                        {
                            Config.HideDialog();
                        }
                        Config.HideDialog();
                    }
                }
                else
                {
                    await DisplayAlert("Permissions Denied", "Unable to take photos.", "OK");
                    //On iOS you may want to send your user to the settings screen.
                    //CrossPermissions.Current.OpenAppSettings();
                }

            }
            catch (Exception ex)
            {
                Config.ErrorStore("ProfilePage-profileUploadTap_Tapped", ex.Message);
                Config.HideDialog();
            }
        }
    }
}