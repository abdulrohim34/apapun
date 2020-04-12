using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GroceryStore.Helpers;
using Newtonsoft.Json;
using ModernHttpClient;

namespace GroceryStore.Models
{
    public class UserAddressLogin
    {
        public string full_address { get; set; }
        public string mobile_number { get; set; }
    }

    public class ChangePasswordResponse
    {
        public int status { get; set; }
        public string message { get; set; }
    }

    public class UpdatePasswordResponse
    {
        public int status { get; set; }
        public string message { get; set; }
    }

    public class ForgotPassword
    {
        public int status { get; set; }
        public string message { get; set; }
        public int otp { get; set; }
        public string mobile_number { get; set; }
    }

    public class User
    {
        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string mobile_number { get; set; }
        public string password { get; set; }
        public string profile_picture { get; set; }
        public string device_id { get; set; }
        public string device_token { get; set; }
        public string device_type { get; set; }
        public string full_address { get; set; }
        public string address_type { get; set; }
        public string referral_code { get; set; }
        public string is_verified { get; set; }
        public string user_status { get; set; }
        public string user_type { get; set; }
        public string status { get; set; }
        public string country { get; set; }
        public string state { get; set; }
        public string city { get; set; }

        public async Task<RegisterResponse> Register(User user)
        {
            //RegisterResponse apiResponse = new RegisterResponse();
            string data = JsonConvert.SerializeObject(user);
            var content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpClient httpClient = new HttpClient(new NativeMessageHandler());
            var response = await httpClient.PostAsync(Config.ApiUrl + Config.Register, content);
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<RegisterResponse>(json);
            //return apiResponse;
        }

        public async Task<RegisterResponse> Login(User user)
        {
            RegisterResponse registerResponse;
            using (HttpClient httpClient = new HttpClient(new NativeMessageHandler()))
            {
                string data = JsonConvert.SerializeObject(user);
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(Config.ApiUrl + Config.Login, content);
                var json = await response.Content.ReadAsStringAsync();
                registerResponse = JsonConvert.DeserializeObject<RegisterResponse>(json);
            }
            return registerResponse;
        }

        public async static Task<RegisterResponse> UpdateProfile(Dictionary<string, string> user)
        {
            RegisterResponse registerResponse;
            using (HttpClient httpClient = new HttpClient(new NativeMessageHandler()))
            {
                string data = JsonConvert.SerializeObject(user);
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(Config.UpdateProfile, content);
                var json = await response.Content.ReadAsStringAsync();
                registerResponse = JsonConvert.DeserializeObject<RegisterResponse>(json);
            }
            return registerResponse;
        }

        public async static Task<ChangePasswordResponse> ChangePassword(Dictionary<string, string> data)
        {
            ChangePasswordResponse changePasswordResponse;
            using (HttpClient httpClient = new HttpClient(new NativeMessageHandler()))
            {
                string jsondata = JsonConvert.SerializeObject(data);
                var content = new StringContent(jsondata, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(Config.ChangePassword, content);
                var json = await response.Content.ReadAsStringAsync();
                changePasswordResponse = JsonConvert.DeserializeObject<ChangePasswordResponse>(json);
            }
            return changePasswordResponse;
        }

        public async static Task<ForgotPassword> ForgotPassword(Dictionary<string, string> data)
        {
            ForgotPassword forgotPassword;
            using (HttpClient httpClient = new HttpClient(new NativeMessageHandler()))
            {
                string jsondata = JsonConvert.SerializeObject(data);
                var content = new StringContent(jsondata, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(Config.ForgotPassword, content);
                var json = await response.Content.ReadAsStringAsync();
                forgotPassword = JsonConvert.DeserializeObject<ForgotPassword>(json);
            }
            return forgotPassword;
        }

        public async static Task<UpdatePasswordResponse> UpdatePassword(Dictionary<string, string> data)
        {
            UpdatePasswordResponse updatePassword;
            using (HttpClient httpClient = new HttpClient(new NativeMessageHandler()))
            {
                string jsondata = JsonConvert.SerializeObject(data);
                var content = new StringContent(jsondata, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(Config.UpdatePassword, content);
                var json = await response.Content.ReadAsStringAsync();
                updatePassword = JsonConvert.DeserializeObject<UpdatePasswordResponse>(json);
            }
            return updatePassword;
        }
    }

}