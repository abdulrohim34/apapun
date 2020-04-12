using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Acr.UserDialogs;
using ModernHttpClient;
using Newtonsoft.Json;
using Plugin.Connectivity;

namespace GroceryStore.Helpers
{
    public class Config
    {
        public const string ApiErrorMessage = "Something went wrong";
        public const string BaseUrl = "http://rajasayurindonesia.com/myadmin/";
        public const string ApiUrl = BaseUrl + "api/";
        public const string Login = "login";
        public const string Register = "userRegister";
        public const string GetCategoryList = ApiUrl + "categoryList?user_id={0}";
        public const string GetCouponList = ApiUrl + "couponList";
        public const string GetProductList = ApiUrl + "productList?category_id={0}&user_id={1}";
        public const string GetProductDetail = ApiUrl + "productList?product_id={0}";
        public const string GetSearchProductList = ApiUrl + "search?text={0}&user_id={1}";
        public const string UpdateDate = ApiUrl + "updateDate?cart_id={0}&type={1}&date={2}&user_id={3}";
        public const string AddToCart = ApiUrl + "addToCart";
        public const string UpdateCart = ApiUrl + "updateCart";
        public const string DeleteCartItem = ApiUrl + "deleteCartItems?cart_id={0}&product_id={0}";
        public const string PlaceOrder = ApiUrl + "CreateOrder";
        public const string GetCartItems = ApiUrl + "checkOut?user_id={0}";
        public const string AddAddress = ApiUrl + "createNewAddress";
        public const string SaveAddress = ApiUrl + "updateAddress";
        public const string GetAddress = ApiUrl + "getUserAddress?user_id={0}";
        public const string DeleteAddress = ApiUrl + "deleteAddress?address_id={0}";
        public const string SetDefaultAddress = ApiUrl + "setDefaultAddress?address_id={0}&user_id={1}";

        public const string GetCountry = ApiUrl + "countryList";
        public const string GetState = ApiUrl + "stateList";
        public const string GetCity = ApiUrl + "cityList";

        public const string GetArea = ApiUrl + "getAreas?city_id={0}";
        public const string GetApartment = ApiUrl + "getLocations?prnt_id={0}";
        public const string AddFavourite = ApiUrl + "addFavouriteProduct";
        public const string DeleteFavourite = ApiUrl + "deleteFavouriteProduct";
        public const string GetFavouriteProducts = ApiUrl + "getFavouriteProduct?user_id={0}";
        public const string ChangePassword = ApiUrl + "changePassword";
        public const string ForgotPassword = ApiUrl + "forgotPassword";
        public const string UpdatePassword = ApiUrl + "updatePassword";
        public const string GetUpcomingOrders = ApiUrl + "getUpcomingOrders?user_id={0}";
        public const string GetPastOrders = ApiUrl + "getPastOrders?user_id={0}";
        public const string GetOrderDetail = ApiUrl + "getOrderDelivery?cart_id={0}&status={1}";
        public const string UpdateProfile = ApiUrl + "updateProfile";
        public const string GetNotification = ApiUrl + "getNotification?user_id={0}";
        public const string GetFaq = ApiUrl + "getFaqs";
        public const string Logout = ApiUrl + "logout?user_id={0}";
        public const string RescheduleOrderItem = ApiUrl + "rescheduleOrderItem";
        public const string UpdateDeviceToken = ApiUrl + "updateDeviceToken?user_id={0}&device_token={1}";
        public const string GetSlider = ApiUrl + "getSliders";
        public const string GetCartCount = ApiUrl + "getCartCount?user_id={0}";
        public const string ApplyCoupon = ApiUrl + "couponApply?user_id={0}&coupon_code_id={1}";
        public const string RemoveCoupon = ApiUrl + "removeCoupon?user_id={0}&coupon_code_id={1}";
        public const string ErrorLogApi = ApiUrl + "addErrorLog";

        public static string ResponseError = "ApiError";
        public static string RequestDataError = "";

        public Boolean CheckInternetConectivity()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void ShowDialog()
        {
            UserDialogs.Instance.ShowLoading();
        }

        public static void HideDialog()
        {
            UserDialogs.Instance.HideLoading();
        }

        public static void SnackbarMessage(string message)
        {
            var toastConfig = new ToastConfig(message);
            toastConfig.SetDuration(3000);
            toastConfig.SetBackgroundColor(System.Drawing.Color.FromArgb(113, 191, 68));
            UserDialogs.Instance.Toast(toastConfig);
        }

        public static void ErrorSnackbarMessage(string message)
        {
            var toastConfig = new ToastConfig(message);
            toastConfig.SetDuration(3000);
            toastConfig.MessageTextColor = System.Drawing.Color.FromArgb(255, 255, 255);
            toastConfig.SetBackgroundColor(System.Drawing.Color.FromArgb(244, 67, 54));
            UserDialogs.Instance.Toast(toastConfig);
        }
        // send error log to server
        public static async Task<ErrorResponse> ErrorLog(Dictionary<string, string> data)
        {
            data.Add("error", Config.ResponseError);
            ErrorResponse response = new ErrorResponse();
            try
            {
                using (HttpClient client = new HttpClient(new NativeMessageHandler()))
                {
                    string Data = JsonConvert.SerializeObject(data);
                    HttpContent content = new StringContent(Data);
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    //var content = new StringContent(Data, Encoding.UTF8, "application/json");
                    var serverResponse = await client.PostAsync(Config.ErrorLogApi, content);
                    var json = await serverResponse.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<ErrorResponse>(json);
                }
            }
            catch (Exception ex)
            {

            }
            return response;
        }

        // save error to server
        public static async void ErrorStore(string method, string exception)
        {
            Dictionary<string, string> ErrorData = new Dictionary<string, string>
            {
                {"method_name", method},
                {"exception_error", exception}
            };
            await Config.ErrorLog(ErrorData);
        }

        public class ErrorResponse
        {
            public int status { get; set; }
            public string message { get; set; }
        }
    }

    public class Message
    {
        public const string addressBlank = "Please choose address to place order";
    }
}
