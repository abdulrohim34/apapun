using GroceryStore.Helpers;
using GroceryStore.Logic;
using GroceryStore.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace GroceryStore.ViewModels
{
    public class AddressVM : INotifyPropertyChanged
    {
        public AddressVM()
        {
            //CountryList();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<UserAddress> userAddressList;

        public ObservableCollection<UserAddress> UserAddressList
        {
            get { return userAddressList; }
            set { userAddressList = value; OnPropertyChanged("UserAddressList"); }
        }

        private UserAddress userAddress;

        public UserAddress UserAddress
        {
            get { return userAddress; }
            set { userAddress = value; OnPropertyChanged("UserAddress"); }
        }

        private int Id;

        public int id
        {
            get { return Id; }
            set { Id = value; OnPropertyChanged("id"); }
        }

        private int UserId;

        public int user_id
        {
            get { return UserId; }
            set { UserId = value; OnPropertyChanged("user_id"); }
        }

        private string Name;

        public string name
        {
            get { return Name; }
            set { Name = value; OnPropertyChanged("name"); }
        }

        private string MobileNumber;

        public string mobile_number
        {
            get { return MobileNumber; }
            set { MobileNumber = value; OnPropertyChanged("mobile_number"); }
        }

        private string HouseNo;

        public string house_no
        {
            get { return HouseNo; }
            set { HouseNo = value; OnPropertyChanged("house_no"); }
        }

        private string ApartmentName;

        public string apartment_name
        {
            get { return ApartmentName; }
            set { ApartmentName = value; OnPropertyChanged("apartment_name"); }
        }

        private string LandmarkDetails;

        public string landmark_details
        {
            get { return LandmarkDetails; }
            set { LandmarkDetails = value; OnPropertyChanged("landmark_details"); }
        }

        private string AreaDetails;

        public string area_details
        {
            get { return AreaDetails; }
            set { AreaDetails = value; OnPropertyChanged("area_details"); }
        }

        private string StreetDetails;

        public string street_details
        {
            get { return StreetDetails; }
            set { StreetDetails = value; OnPropertyChanged("street_details"); }
        }

        private string AddressType;

        public string address_type
        {
            get { return AddressType; }
            set { AddressType = value; OnPropertyChanged("address_type"); }
        }

        private string FullAddress;

        public string full_address
        {
            get { return FullAddress; }
            set { FullAddress = value; OnPropertyChanged("full_address"); }
        }

        private int DefaultAddress;

        public int default_address
        {
            get { return DefaultAddress; }
            set { DefaultAddress = value; OnPropertyChanged("default_address"); }
        }

        private string City;

        public string city
        {
            get { return City; }
            set { City = value; OnPropertyChanged("city"); }
        }

        private string Pincode;

        public string pincode
        {
            get { return Pincode; }
            set { Pincode = value; OnPropertyChanged("pincode"); }
        }

        private bool _isDefault;
        public bool IsDefault
        {
            get
            {
                if (default_address == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            set { _isDefault = value; }
        }


        private CountryResponse _countryResponse;

        public CountryResponse CountryResponse
        {
            get => _countryResponse;
            set
            {
                _countryResponse = value;
                OnPropertyChanged(nameof(CountryResponse));
            }
        }

        private StateResponse _stateResponse;

        public StateResponse StateResponse
        {
            get => _stateResponse;
            set
            {
                _stateResponse = value;
                OnPropertyChanged(nameof(StateResponse));
            }
        }

        private CityResponse _cityResponse;

        public CityResponse CityResponse
        {
            get => _cityResponse;
            set
            {
                _cityResponse = value;
                OnPropertyChanged(nameof(CityResponse));
            }
        }

        public static string SelectedCountry { get; set; }
        public static string SelectedState { get; set; }
        public static string SelectedCity { get; set; }

        private int _countrySelectedIndex;

        public int CountrySelectedIndex
        {
            get { return _countrySelectedIndex; }
            set
            {
                _countrySelectedIndex = value;
                OnPropertyChanged(nameof(CountrySelectedIndex));
            }
        }

        private int _stateSelectedIndex;

        public int StateSelectedIndex
        {
            get { return _stateSelectedIndex; }
            set
            {
                _stateSelectedIndex = value;
                OnPropertyChanged(nameof(StateSelectedIndex));
            }
        }

        private int _citySelectedIndex;

        public int CitySelectedIndex
        {
            get { return _citySelectedIndex; }
            set
            {
                _citySelectedIndex = value;
                OnPropertyChanged(nameof(CitySelectedIndex));
            }
        }


        // get country list
        async void CountryList()
        {
            try
            {

                Config.ShowDialog();
                Dictionary<string, string> data = new Dictionary<string, string>
                {
                    {"temp", "1"}
                };
                var response = await UserLogic.GetCountry(data);
                if (response.status == 200)
                {
                    Config.HideDialog();
                    CountryResponse = response;
                    CountryResponse.data.Insert(0, new Country() { id = 0, name = "Select Country" });

                    CountrySelectedIndex = 0;
                    //StateList(CountryResponse.branch_country.ToString());
                    //System.Diagnostics.Debug.WriteLine("-12-"+ CountryResponse.branch_country.ToString());
                }
                else
                {
                    Config.HideDialog();
                    Config.ErrorSnackbarMessage(response.message);
                }
            }
            catch (Exception ex)
            {
                Config.HideDialog();
                Config.ErrorSnackbarMessage(Config.ApiErrorMessage);
                System.Diagnostics.Debug.WriteLine("MTS-ClientVM-CountryList", ex.Message);
            }
        }

        // get state list
        public async void StateList(string selectedCountryId = null)
        {
            System.Diagnostics.Debug.WriteLine("-1-");
            try
            {

                if (selectedCountryId != null)
                {
                    Config.ShowDialog();
                    Dictionary<string, string> data = new Dictionary<string, string>
                    {
                        {"country_id", selectedCountryId}
                    };
                    var response = await UserLogic.GetState(data);
                    if (response.status == 200)
                    {
                        Config.HideDialog();
                        StateResponse = response;
                        StateResponse.data.Insert(0, new State() { id = 0, name = "Select State" });
                        StateSelectedIndex = 0;
                        OnPropertyChanged(nameof(StateSelectedIndex));
                    }
                    else
                    {
                        Config.HideDialog();
                        Config.ErrorSnackbarMessage(response.message);
                    }
                }
                else
                {
                    StateResponse = new StateResponse()
                    { status = 200, data = new List<State>() { new State() { name = "No state found" } } };
                    StateSelectedIndex = 0;
                    OnPropertyChanged(nameof(StateSelectedIndex));
                }
            }
            catch (Exception ex)
            {
                Config.HideDialog();
                Config.ErrorSnackbarMessage(Config.ApiErrorMessage);
                System.Diagnostics.Debug.WriteLine("MTS-ClientVM-StateList", ex.Message);
            }
        }

        // get city list
        public async void CityList(string selectedStateId = null)
        {
            try
            {

                if (selectedStateId != null)
                {
                    Config.ShowDialog();
                    Dictionary<string, string> data = new Dictionary<string, string>
                    {
                        {"state_id", selectedStateId}
                    };
                    var response = await UserLogic.GetCity(data);
                    if (response.status == 200)
                    {
                        Config.HideDialog();
                        CityResponse = response;
                        CityResponse.data.Insert(0, new City() { id = 0, name = "Select City" });
                        CitySelectedIndex = 0;
                        OnPropertyChanged(nameof(CitySelectedIndex));
                    }
                    else
                    {
                        Config.HideDialog();
                        Config.ErrorSnackbarMessage(response.message);
                    }
                }
                else
                {
                    CityResponse = new CityResponse()
                    { status = 200, data = new List<City>() { new City() { name = "No city found" } } };
                    CitySelectedIndex = 0;
                    OnPropertyChanged(nameof(CitySelectedIndex));
                }
            }
            catch (Exception ex)
            {
                Config.HideDialog();
                Config.ErrorSnackbarMessage(Config.ApiErrorMessage);
                System.Diagnostics.Debug.WriteLine("MTS-ClientVM-CityList", ex.Message);
            }
        }

        // get selected country
        public ICommand CountrySelectedCommand
        {
            get
            {
                return new Command((item) =>
                {
                    if (item is Country selectedCountry && selectedCountry.id != 0)
                    {
                        StateList(selectedCountry.id.ToString());
                        System.Diagnostics.Debug.WriteLine("-11-" + selectedCountry.id.ToString());
                        SelectedCountry = selectedCountry.name;
                    }
                });
            }
        }

        // get selected state
        public ICommand StateSelectedCommand
        {
            get
            {
                return new Command((item) =>
                {
                    if (item is State selectedState && selectedState.id != 0)
                    {
                        CityList(selectedState.id.ToString());
                        SelectedState = selectedState.name;
                        OnPropertyChanged(nameof(StateSelectedIndex));
                    }
                });
            }
        }

        // get selected city
        public ICommand CitySelectedCommand
        {
            get
            {
                return new Command((item) =>
                {
                    if (item is City selectedCity && selectedCity.id != 0)
                    {
                        SelectedCity = selectedCity.name;
                    }
                });
            }
        }
    }
}
