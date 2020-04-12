using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using GroceryStore.Helpers;
using GroceryStore.Models;
using ModernHttpClient;

namespace GroceryStore.Logic
{
    public class ProductLogic
    {
        public static async Task<ProductResponce> GetProducts(int CategoryId, string UserId)
        {
            ProductResponce productResponce;
            using (HttpClient httpClient = new HttpClient(new NativeMessageHandler()))
            {
                var url = string.Format(Config.GetProductList, CategoryId, UserId);
                var response = await httpClient.GetAsync(url);
                var json = await response.Content.ReadAsStringAsync();
                productResponce = JsonConvert.DeserializeObject<ProductResponce>(json);
            }

            return productResponce;
        }

        public static async Task<Product> GetProduct(int ProductId)
        {
            Product product = new Product();
            using (HttpClient httpClient = new HttpClient(new NativeMessageHandler()))
            {
                var url = string.Format(Config.GetProductDetail, ProductId);
                var response = await httpClient.GetAsync(url);
                var json = await response.Content.ReadAsStringAsync();

                var productDetail = JsonConvert.DeserializeObject<ProductDetailResponce>(json);
                product = productDetail.data as Product;
            }

            return product;
        }

        public static async Task<SearchProductResponce> GetSearchProducts(string text, string UserId)
        {
            SearchProductResponce productResponce;
            using (HttpClient httpClient = new HttpClient(new NativeMessageHandler()))
            {
                var url = string.Format(Config.GetSearchProductList, text, UserId);
                var response = await httpClient.GetAsync(url);
                var json = await response.Content.ReadAsStringAsync();
                productResponce = JsonConvert.DeserializeObject<SearchProductResponce>(json);
            }

            return productResponce;
        }

        public static async Task<FavouriteProductResponce> AddFavouriteProduct(Dictionary<string, int> data)
        {
            FavouriteProductResponce favouriteProduct;
            using (HttpClient httpClient = new HttpClient(new NativeMessageHandler()))
            {
                var jsonData = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(Config.AddFavourite, jsonData);
                var json = await response.Content.ReadAsStringAsync();
                favouriteProduct = JsonConvert.DeserializeObject<FavouriteProductResponce>(json);
            }
            return favouriteProduct;
        }

        public static async Task<FavouriteProductListResponse> GetFavouriteProducts(int user_id)
        {
            FavouriteProductListResponse favouriteProduct;
            using (HttpClient httpClient = new HttpClient(new NativeMessageHandler()))
            {
                var url = string.Format(Config.GetFavouriteProducts, user_id);
                var response = await httpClient.GetAsync(url);
                var json = await response.Content.ReadAsStringAsync();
                favouriteProduct = JsonConvert.DeserializeObject<FavouriteProductListResponse>(json);
            }
            return favouriteProduct;
        }

        public static async Task<DeleteFavouriteProductResponce> DeleteFavouriteProduct(Dictionary<string, string> data)
        {
            DeleteFavouriteProductResponce deleteFavouriteProduct;
            using (HttpClient httpClient = new HttpClient(new NativeMessageHandler()))
            {
                var jsonData = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(Config.DeleteFavourite, jsonData);
                var json = await response.Content.ReadAsStringAsync();
                deleteFavouriteProduct = JsonConvert.DeserializeObject<DeleteFavouriteProductResponce>(json);
            }
            return deleteFavouriteProduct;
        }
    }
}
