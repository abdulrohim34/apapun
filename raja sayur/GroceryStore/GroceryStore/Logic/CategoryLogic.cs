using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GroceryStore.Helpers;
using GroceryStore.Models;
using Newtonsoft.Json;
using ModernHttpClient;
using Xamarin.Forms;

namespace GroceryStore.Logic
{
    public class CategoryLogic
    {
        public static async Task<CategoryResponse> CategoryList()
        {
            CategoryResponse categoryResponse;
            using (HttpClient httpClient = new HttpClient(new NativeMessageHandler()))
            {
                string UserId = string.Empty;
                if (Application.Current.Properties.ContainsKey("user_id"))
                    UserId = Application.Current.Properties["user_id"].ToString();
                var response = await httpClient.GetAsync(String.Format(Config.GetCategoryList, UserId));
                var json = await response.Content.ReadAsStringAsync();
                categoryResponse = JsonConvert.DeserializeObject<CategoryResponse>(json);
            }
            return categoryResponse;
        }

        public static async Task<CouponResponse> CouponList()
        {
            CouponResponse couponResponse;
            using (HttpClient httpClient = new HttpClient(new NativeMessageHandler()))
            {
                var response = await httpClient.GetAsync(Config.GetCouponList);
                var json = await response.Content.ReadAsStringAsync();
                couponResponse = JsonConvert.DeserializeObject<CouponResponse>(json);
            }
            return couponResponse;
        }
    }
}
