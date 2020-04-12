using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using GroceryStore.Helpers;
using GroceryStore.Logic;
using GroceryStore.Models;
using GroceryStore.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GroceryStore.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddAddressPage : ContentPage
    {
        string _pageTitle = "Add Address";
        UserAddress UserAddress = null;
        AddressVM ViewModel;
        public List<string> AddressType;

        public AddAddressPage()
        {
            InitializeComponent();
            ViewModel = new AddressVM();
            Title = "Add Address";
        }

        public AddAddressPage(UserAddress UserAddress)
        {
            InitializeComponent();
            ViewModel = new AddressVM();
            this.UserAddress = UserAddress;
            Title = "Update Address";
            if (UserAddress != null)
            {
                addressLbl.Text = "Save Address";
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Send((App)Application.Current, "NavigationBar", _pageTitle);
            try
            {
                Config.ShowDialog();
                AddressType = new List<string>(){
                    "Home","Office","Other"
                };
                //address_type.ItemsSource = AddressType;


                Dictionary<string, string> data = new Dictionary<string, string>
                {
                    {"temp", "1"}
                    };
                var countries = await UserLogic.GetCountry(data);
                countries.data.Insert(0, new Country() { id = 0, name = "Select Country" });

                state.ItemsSource = new List<State>() { new State() { id = 0, name = "Select State" } };
                city.ItemsSource = new List<City>() { new City() { id = 0, name = "Select City" } };

                //var states = await UserLogic.GetState(data);
                //var cities = await UserLogic.GetCity(data);
                //var cities = await UserLogic.GetCityList();
                //var areas = await UserLogic.GetAreaList(3378);
                //city.ItemsSource = cities.data.ToList();
                country.ItemsSource = countries.data.ToList();
                //states.data.Insert(0, new State() { id = 0, name = "Select State" });
                //area_details.ItemsSource = areas.data.ToList();
                //var apartment = await UserLogic.GetApartmentList(areas.data.FirstOrDefault().id);
                //apartment.data.Insert(0, new Apartment() { id = 0, name = "Select Apartment" });
                //apartment_name.ItemsSource = apartment.data.ToList();

                //areas.data.Insert(0, new Area() { id = 0, name = "Select Area" });
                //area_details.ItemsSource = areas.data.ToList();

                if (UserAddress != null)
                {
                    var CountryId = countries.data.Where(a => a.name == UserAddress.country).First();
                    Dictionary<string, string> stateData = new Dictionary<string, string>
                    {
                        {"country_id", CountryId.id.ToString()}
                    };
                    var states = await UserLogic.GetState(stateData);
                    state.ItemsSource = states.data.ToList();
                    var StateId = states.data.Where(a => a.name == UserAddress.state).First();

                    Dictionary<string, string> cityData = new Dictionary<string, string>
                    {
                        {"state_id", StateId.id.ToString()}
                    };
                    var cities = await UserLogic.GetCity(cityData);
                    city.ItemsSource = cities.data.ToList();

                    //var areaData = areas.data.Where(a => a.name == UserAddress.area_details).FirstOrDefault();
                    var CountryIndex = countries.data.ToList().FindIndex(a => a.name == UserAddress.country);
                    var StateIndex = states.data.ToList().FindIndex(a => a.name == UserAddress.state);
                    var CityIndex = cities.data.ToList().FindIndex(a => a.name == UserAddress.city);
                    //var apartmentIndex = apartment.data.ToList().FindIndex(a => a.name == UserAddress.apartment_name);
                    var addressTypeIndex = AddressType.FindIndex(a => a == UserAddress.address_type);
                    //var cityIndex = cities.data.ToList().FindIndex(a => a.name == UserAddress.city);
                    house_no.Text = UserAddress.house_no;
                    street_details.Text = UserAddress.street_details;
                    landmark_details.Text = UserAddress.landmark_details;
                    pincode.Text = UserAddress.pincode;
                    //default_address.IsChecked = (UserAddress.default_address == 0) ? false : true;

                    country.SelectedIndex = CountryIndex;
                    state.SelectedIndex = StateIndex;
                    city.SelectedIndex = CityIndex;
                    //area_details.SelectedIndex = areaIndex;
                    //apartment_name.SelectedIndex = apartmentIndex;
                    //address_type.SelectedIndex = addressTypeIndex;
                    addressLbl.Text = "Save Address";
                }
                else
                {
                    country.SelectedIndex = 0;
                    state.SelectedIndex = 0;
                    //areas.data.Insert(0, new Area() { id = 0, name = "Select Area" });
                    city.SelectedIndex = 0;
                    //area_details.SelectedIndex = 0;
                    //apartment_name.SelectedIndex = 0;
                }
                Config.HideDialog();
            }
            catch (Exception ex)
            {
                Config.HideDialog();
                Config.ErrorStore("AddAddressPage-OnAppearing", ex.Message);
                Config.ErrorSnackbarMessage(Config.ApiErrorMessage);
            }
        }

        private async void addAddressTap_Tapped(object sender, EventArgs e)
        {

            try
            {
                if (string.IsNullOrWhiteSpace(house_no.Text))
                {
                    Config.ErrorSnackbarMessage(ValidationMessages.HouseNoRequired);
                }
                else if (string.IsNullOrWhiteSpace(street_details.Text))
                {
                    Config.ErrorSnackbarMessage(ValidationMessages.StreetDetailRequired);
                }
                else if (country.SelectedIndex == 0)
                {
                    Config.ErrorSnackbarMessage(ValidationMessages.CountryRequired);
                }
                else if (state.SelectedIndex == 0)
                {
                    Config.ErrorSnackbarMessage(ValidationMessages.StateRequired);
                }
                else if (city.SelectedIndex == 0)
                {
                    Config.ErrorSnackbarMessage(ValidationMessages.CityRequired);
                }
                else if (string.IsNullOrWhiteSpace(pincode.Text))
                {
                    Config.ErrorSnackbarMessage(ValidationMessages.PincodeRequired);
                }
                else if (pincode.Text.Length < 5)
                {
                    Config.ErrorSnackbarMessage(ValidationMessages.PincodeMinimum);
                }
                else
                {
                    Config.ShowDialog();
                    UserAddress userAddress = new UserAddress
                    {
                        //address_type = address_type.Items[address_type.SelectedIndex],
                        //address_type = address_type.SelectedItem.ToString(),
                        //apartment_name = apartment_name.Items[apartment_name.SelectedIndex],
                        //area_details = area_details.Items[area_details.SelectedIndex],
                        country = country.Items[country.SelectedIndex],
                        state = state.Items[state.SelectedIndex],
                        city = city.Items[city.SelectedIndex],
                        house_no = house_no.Text,
                        landmark_details = landmark_details.Text,
                        street_details = street_details.Text,
                        pincode = pincode.Text,
                        user_id = int.Parse(Application.Current.Properties["user_id"].ToString()),
                        name = Application.Current.Properties["name"].ToString(),
                        mobile_number = Application.Current.Properties["mobile_number"].ToString(),
                        //default_address = default_address.IsChecked ? 1 : 0,
                        default_address = 1,
                    };

                    var response = new GeneralResponse();
                    if (UserAddress != null)
                    {
                        userAddress.id = UserAddress.id;
                        response = await UserLogic.SaveAddress(userAddress);
                        Config.HideDialog();
                    }
                    else
                    {
                        response = await UserLogic.AddAddress(userAddress);
                        Config.HideDialog();
                    }
                    if (response.status == 200)
                    {
                        Config.HideDialog();
                        //if (default_address.IsChecked)
                        //{
                        //    Application.Current.Properties["full_address"] = response.full_address;
                        //}
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        Config.HideDialog();
                        Config.ErrorSnackbarMessage(response.message);
                    }
                    Config.HideDialog();
                }
            }
            catch (Exception ex)
            {
                Config.HideDialog();
                Config.ErrorStore("AddAddressPage-addAddressTap_Tapped", ex.Message);
                Config.ErrorSnackbarMessage(Config.ApiErrorMessage);
            }
        }

        private void city_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = sender as Picker;
            var item = picker.SelectedItem;
            //if (UserAddress == null)
            //{
            //    var cities = await UserLogic.GetCityList();
            //    cities.data.Insert(0, new City() { id = 0, name = "Select City" });
            //    city.ItemsSource = cities.data.ToList();
            //    city.SelectedIndex = 0;
            //}
        }


        private async void country_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Config.ShowDialog();
                var picker = (Picker)sender;
                var item = (Country)picker.SelectedItem;
                if (item.id != 0)
                {
                    Dictionary<string, string> data = new Dictionary<string, string>
                    {
                        {"country_id", item.id.ToString()}
                    };

                    var states = await UserLogic.GetState(data);
                    states.data.Insert(0, new State() { id = 0, name = "Select State" });
                    state.ItemsSource = states.data.ToList();
                    if (UserAddress == null)
                    {
                        Config.HideDialog();
                        state.SelectedIndex = 0;
                    }
                    else
                    {
                        Config.HideDialog();
                        state.SelectedIndex = states.data.ToList().FindIndex(a => a.name == UserAddress.state);
                    }
                    Config.HideDialog();
                }
                Config.HideDialog();
            }
            catch (Exception ex)
            {
                Config.HideDialog();
                //Config.ErrorSnackbarMessage(Config.ApiErrorMessage);
            }
        }

        private async void state_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Config.ShowDialog();
                var picker = (Picker)sender;
                var item = (State)picker.SelectedItem;
                if (item.id != 0)
                {
                    Dictionary<string, string> data = new Dictionary<string, string>
                    {
                        {"state_id", item.id.ToString()}
                    };

                    var cities = await UserLogic.GetCity(data);
                    cities.data.Insert(0, new City() { id = 0, name = "Select City" });
                    city.ItemsSource = cities.data.ToList();
                    if (UserAddress == null)
                    {
                        Config.HideDialog();
                        city.SelectedIndex = 0;
                    }
                    else
                    {
                        Config.HideDialog();
                        city.SelectedIndex = cities.data.ToList().FindIndex(a => a.name == UserAddress.city);
                    }
                    Config.HideDialog();
                }
                Config.HideDialog();
            }
            catch (Exception ex)
            {
                Config.HideDialog();
                //Config.ErrorSnackbarMessage(Config.ApiErrorMessage);
            }
        }

    }
}