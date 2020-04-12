using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GroceryStore.Helpers;
using GroceryStore.Models;
using Newtonsoft.Json;
using ModernHttpClient;

namespace GroceryStore.Logic
{
    public class UserLogic
    {
        public static async Task<GeneralResponse> AddAddress(UserAddress userAddress)
        {
            GeneralResponse generalResponse;
            using (HttpClient httpClient = new HttpClient(new NativeMessageHandler()))
            {
                var stringData = JsonConvert.SerializeObject(userAddress);
                var jsonData = new StringContent(stringData, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(Config.AddAddress, jsonData);
                var json = await response.Content.ReadAsStringAsync();
                generalResponse = JsonConvert.DeserializeObject<GeneralResponse>(json);
            }

            return generalResponse;
        }

        public static async Task<GeneralResponse> SaveAddress(UserAddress userAddress)
        {
            GeneralResponse generalResponse;
            using (HttpClient httpClient = new HttpClient(new NativeMessageHandler()))
            {
                var stringData = JsonConvert.SerializeObject(userAddress);
                var jsonData = new StringContent(stringData, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(Config.SaveAddress, jsonData);
                var json = await response.Content.ReadAsStringAsync();
                generalResponse = JsonConvert.DeserializeObject<GeneralResponse>(json);
            }

            return generalResponse;
        }


        // get country list
        public static async Task<CountryResponse> GetCountry(Dictionary<string, string> data)
        {
            CountryResponse response;
            using (HttpClient client = new HttpClient(new NativeMessageHandler()))
            {
                string Data = JsonConvert.SerializeObject(data);
                var content = new StringContent(Data, Encoding.UTF8, "application/json");
                var serverResponse = await client.PostAsync(Config.GetCountry, content);
                var json = await serverResponse.Content.ReadAsStringAsync();
                Config.ResponseError = json;
                response = JsonConvert.DeserializeObject<CountryResponse>(json);
            }
            return response;
        }

        // get state list
        public static async Task<StateResponse> GetState(Dictionary<string, string> data)
        {
            StateResponse response;
            using (HttpClient client = new HttpClient(new NativeMessageHandler()))
            {
                string Data = JsonConvert.SerializeObject(data);
                var content = new StringContent(Data, Encoding.UTF8, "application/json");
                var serverResponse = await client.PostAsync(Config.GetState, content);
                var json = await serverResponse.Content.ReadAsStringAsync();
                Config.ResponseError = json;
                response = JsonConvert.DeserializeObject<StateResponse>(json);
            }
            return response;
        }


        // get city list
        public static async Task<CityResponse> GetCity(Dictionary<string, string> data)
        {
            CityResponse response;
            using (HttpClient client = new HttpClient(new NativeMessageHandler()))
            {
                string Data = JsonConvert.SerializeObject(data);
                var content = new StringContent(Data, Encoding.UTF8, "application/json");
                var serverResponse = await client.PostAsync(Config.GetCity, content);
                var json = await serverResponse.Content.ReadAsStringAsync();
                Config.ResponseError = json;
                response = JsonConvert.DeserializeObject<CityResponse>(json);
            }
            return response;
        }

        public static async Task<CityResponse> GetCityList()
        {
            CityResponse cities;
            using (HttpClient httpClient = new HttpClient(new NativeMessageHandler()))
            {
                var response = await httpClient.GetAsync(Config.GetCity);
                var json = await response.Content.ReadAsStringAsync();
                cities = JsonConvert.DeserializeObject<CityResponse>(json);
            }
            return cities;
        }

        public static async Task<AreaResponse> GetAreaList(int CityId)
        {
            AreaResponse areas;
            using (HttpClient httpClient = new HttpClient(new NativeMessageHandler()))
            {
                var response = await httpClient.GetAsync(string.Format(Config.GetArea, CityId));
                var json = await response.Content.ReadAsStringAsync();
                areas = JsonConvert.DeserializeObject<AreaResponse>(json);
            }
            return areas;
        }

        public static async Task<ApartmentResponse> GetApartmentList(int AreaId)
        {
            ApartmentResponse locations;
            using (HttpClient httpClient = new HttpClient(new NativeMessageHandler()))
            {
                var response = await httpClient.GetAsync(string.Format(Config.GetApartment, AreaId));
                var json = await response.Content.ReadAsStringAsync();
                locations = JsonConvert.DeserializeObject<ApartmentResponse>(json);
            }
            return locations;
        }

        public static async Task<UserAddressResponse> GetAddressList(int UserId)
        {
            UserAddressResponse addressResponse;
            using (HttpClient httpClient = new HttpClient(new NativeMessageHandler()))
            {
                var response = await httpClient.GetAsync(string.Format(Config.GetAddress, UserId));
                var json = await response.Content.ReadAsStringAsync();
                addressResponse = JsonConvert.DeserializeObject<UserAddressResponse>(json);
            }
            return addressResponse;
        }

        public static async Task<GeneralResponse> DeleteAddress(int AddressId)
        {
            GeneralResponse generalResponse;
            using (HttpClient httpClient = new HttpClient(new NativeMessageHandler()))
            {
                var response = await httpClient.GetAsync(string.Format(Config.DeleteAddress, AddressId));
                var json = await response.Content.ReadAsStringAsync();
                generalResponse = JsonConvert.DeserializeObject<GeneralResponse>(json);
            }
            return generalResponse;
        }

        public static async Task<DefaultAddressResponse> SetDefaultAddress(int AddressId, int UserId)
        {
            DefaultAddressResponse generalResponse;
            using (HttpClient httpClient = new HttpClient(new NativeMessageHandler()))
            {
                var response = await httpClient.GetAsync(string.Format(Config.SetDefaultAddress, AddressId, UserId));
                var json = await response.Content.ReadAsStringAsync();
                generalResponse = JsonConvert.DeserializeObject<DefaultAddressResponse>(json);
            }
            return generalResponse;
        }

        public static async Task<GeneralResponse> Logout(int UserId)
        {
            GeneralResponse generalResponse;
            using (HttpClient httpClient = new HttpClient(new NativeMessageHandler()))
            {
                var response = await httpClient.GetAsync(string.Format(Config.Logout, UserId));
                var json = await response.Content.ReadAsStringAsync();
                generalResponse = JsonConvert.DeserializeObject<GeneralResponse>(json);
            }
            return generalResponse;
        }

        public static async Task<GeneralResponse> UpdateDeviceToken(string DeviceToken)
        {
            GeneralResponse generalResponse = new GeneralResponse();
            try
            {
                if (App.Current.Properties.ContainsKey("user_id"))
                {
                    string UserId = App.Current.Properties["user_id"].ToString();
                    if (!string.IsNullOrEmpty(UserId))
                    {
                        using (HttpClient httpClient = new HttpClient(new NativeMessageHandler()))
                        {
                            var response = await httpClient.GetAsync(string.Format(Config.UpdateDeviceToken, UserId, DeviceToken));
                            var json = await response.Content.ReadAsStringAsync();
                            generalResponse = JsonConvert.DeserializeObject<GeneralResponse>(json);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Config.ErrorStore("UserLogic-UpdateDeviceToken", ex.Message);
            }
            return generalResponse;
        }
    }


    public class Country
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class CountryResponse
    {
        public int status { get; set; }
        public string message { get; set; }
        public IList<Country> data { get; set; }
    }

    public class State
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class StateResponse
    {
        public int status { get; set; }
        public string message { get; set; }
        public IList<State> data { get; set; }
    }
}
