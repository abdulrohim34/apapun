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
    public class CartLogic
    {
        public static async Task<AddCart> AddToCart(Dictionary<string, string> data)
        {
            AddCart addCart = new AddCart();
            using (HttpClient httpClient = new HttpClient(new NativeMessageHandler()))
            {
                var jsonData = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(Config.AddToCart, jsonData);
                var json = await response.Content.ReadAsStringAsync();
                addCart = JsonConvert.DeserializeObject<AddCart>(json);
            }
            return addCart;
        }

        public static async Task<AddCart> UpdateCart(Dictionary<string, string> data)
        {
            AddCart addCart = new AddCart();
            using (HttpClient httpClient = new HttpClient(new NativeMessageHandler()))
            {
                var jsonData = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(Config.UpdateCart, jsonData);
                var json = await response.Content.ReadAsStringAsync();
                addCart = JsonConvert.DeserializeObject<AddCart>(json);
            }

            return addCart;
        }

        public static async Task<OrderResponse> DeleteCartItem(Dictionary<string, int> data)
        {
            OrderResponse cart = new OrderResponse();
            using (HttpClient httpClient = new HttpClient(new NativeMessageHandler()))
            {
                var jsonData = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(Config.DeleteCartItem, jsonData);
                var json = await response.Content.ReadAsStringAsync();
                cart = JsonConvert.DeserializeObject<OrderResponse>(json);
            }
            return cart;
        }

        public static async Task<CartResponse> UpdateCartItemQuantity(Dictionary<string, int> data)
        {
            CartResponse cart = new CartResponse();
            using (HttpClient httpClient = new HttpClient(new NativeMessageHandler()))
            {
                var jsonData = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(Config.DeleteCartItem, jsonData);
                var json = await response.Content.ReadAsStringAsync();
                cart = JsonConvert.DeserializeObject<CartResponse>(json);
            }
            return cart;
        }

        public static async Task<CartResponse> GetCartItems(int user_id)
        {
            CartResponse cart = new CartResponse();
            using (HttpClient httpClient = new HttpClient(new NativeMessageHandler()))
            {
                var response = await httpClient.GetAsync(String.Format(Config.GetCartItems, user_id));
                var json = await response.Content.ReadAsStringAsync();
                cart = JsonConvert.DeserializeObject<CartResponse>(json);
            }
            return cart;
        }

        public static async Task<CartDateResponse> UpdateDate(int CartId, string Type, string Date, string UserId)
        {
            Config.ShowDialog();
            CartDateResponse cart = new CartDateResponse();
            try
            {
                DateTime oDate = Convert.ToDateTime(Date);
                //System.Diagnostics.Debug.WriteLine(oDate);
                string ConvertedDate = oDate.ToString("yyyy/MM/dd");
                //System.Diagnostics.Debug.WriteLine(ConvertedDate);
                using (HttpClient httpClient = new HttpClient(new NativeMessageHandler()))
                {
                    var response = await httpClient.GetAsync(String.Format(Config.UpdateDate, CartId, Type, ConvertedDate, UserId));
                    var json = await response.Content.ReadAsStringAsync();
                    cart = JsonConvert.DeserializeObject<CartDateResponse>(json);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("MTG5");
                System.Diagnostics.Debug.WriteLine(ex.Message);
                Config.ErrorStore("CartLogic-UpdateDate", ex.Message);
                Config.HideDialog();
            }
            Config.HideDialog();
            return cart;
        }

        public static async Task<OrderStatusResponse> PlaceOrder(Dictionary<string, string> data)
        {
            OrderStatusResponse orderResponse;
            using (HttpClient httpClient = new HttpClient(new NativeMessageHandler()))
            {
                var jsonData = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(Config.PlaceOrder, jsonData);
                var json = await response.Content.ReadAsStringAsync();
                orderResponse = JsonConvert.DeserializeObject<OrderStatusResponse>(json);
            }

            return orderResponse;
        }

        public static async Task<OrderDetailResponse> RescheduleOrderItem(Dictionary<string, string> data)
        {
            OrderDetailResponse detailResponse;
            using (HttpClient httpClient = new HttpClient(new NativeMessageHandler()))
            {
                var jsonData = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(Config.RescheduleOrderItem, jsonData);
                var json = await response.Content.ReadAsStringAsync();
                detailResponse = JsonConvert.DeserializeObject<OrderDetailResponse>(json);
            }
            return detailResponse;
        }

        public static async Task<UserCartResponse> CartCount(string userId)
        {
            UserCartResponse detailResponse;
            using (HttpClient httpClient = new HttpClient(new NativeMessageHandler()))
            {
                var response = await httpClient.GetAsync(String.Format(Config.GetCartCount, userId));
                var json = await response.Content.ReadAsStringAsync();
                detailResponse = JsonConvert.DeserializeObject<UserCartResponse>(json);
            }
            return detailResponse;
        }

        public static async Task<UserCartResponse> ApplyCoupon(string userId, string couponCodeId)
        {
            UserCartResponse detailResponse;
            using (HttpClient httpClient = new HttpClient(new NativeMessageHandler()))
            {
                var response = await httpClient.GetAsync(String.Format(Config.ApplyCoupon, userId, couponCodeId));
                var json = await response.Content.ReadAsStringAsync();
                detailResponse = JsonConvert.DeserializeObject<UserCartResponse>(json);
            }
            return detailResponse;
        }

        public static async Task<UserCartResponse> RemoveCoupon(string userId, string couponCodeId)
        {
            UserCartResponse detailResponse;
            using (HttpClient httpClient = new HttpClient(new NativeMessageHandler()))
            {
                var response = await httpClient.GetAsync(String.Format(Config.RemoveCoupon, userId, couponCodeId));
                var json = await response.Content.ReadAsStringAsync();
                detailResponse = JsonConvert.DeserializeObject<UserCartResponse>(json);
            }
            return detailResponse;
        }
    }
}
