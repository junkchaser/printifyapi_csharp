using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
/*
 * Summary: 
 *   This class provides a list of functions that corresponds with the PrintifyAPI endpoints
 * 
 * Author: Junkchaser
 * Date: 22/05/2024
 * 
 * Description:
 *   Please check the official printify api docs: https://developers.printify.com/#v1-api-reference
 *  
 */
public class PrintifyApi
{
    private readonly HttpClient _client;

    public PrintifyApi(string accessToken)
    {
        _client = new HttpClient();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
    }

    // Shops
    public async Task<string> GetShopsAsync()
    {
        return await GetAsync("/v1/shops.json");
    }

    public async Task<string> ConnectShopAsync(string shopId, string externalShopId, bool enabled, string externalName)
    {
        var body = new
        {
            externalShopId,
            externalStatus = new { enabled },
            externalName
        };
        return await PostAsync($"/v1/shops/{shopId}/connection.json", body);
    }

    public async Task<string> UpdateShopAsync(string shopId, string externalShopId, bool enabled, string externalName)
    {
        var body = new
        {
            externalShopId,
            externalStatus = new { enabled },
            externalName
        };
        return await PutAsync($"/v1/shops/{shopId}/connection.json", body);
    }

    // Orders
    public async Task<string> GetOrdersAsync(string shopId)
    {
        return await GetAsync($"/v1/shops/{shopId}/orders.json");
    }

    public async Task<string> GetOrderByIdAsync(string shopId, string orderId)
    {
        return await GetAsync($"/v1/shops/{shopId}/orders/{orderId}.json");
    }

    public async Task<string> CreateOrderAsync(string shopId, object order)
    {
        return await PostAsync($"/v1/shops/{shopId}/orders.json", order);
    }

    public async Task<string> SubmitPullOrdersDataFeedAsync(string shopId, string url, string username, string password, object[] headers)
    {
        var body = new
        {
            url,
            auth = new
            {
                basic = new { username, password },
                headers
            }
        };
        return await PostAsync($"/v1/shops/{shopId}/orders-pull-feed.json", body);
    }

    // Products
    public async Task<string> GetProductsAsync(string shopId)
    {
        return await GetAsync($"/v1/shops/{shopId}/products.json");
    }

    public async Task<string> GetProductByIdAsync(string shopId, string productId)
    {
        return await GetAsync($"/v1/shops/{shopId}/products/{productId}.json");
    }

    public async Task<string> CreateProductAsync(string shopId, object product)
    {
        return await PostAsync($"/v1/shops/{shopId}/products.json", product);
    }

    public async Task<string> UpdateProductAsync(string shopId, string productId, object product)
    {
        return await PutAsync($"/v1/shops/{shopId}/products/{productId}.json", product);
    }

    public async Task<string> DeleteProductAsync(string shopId, string productId)
    {
        return await DeleteAsync($"/v1/shops/{shopId}/products/{productId}.json");
    }

    private async Task<string> GetAsync(string endpoint)
    {
        var response = await _client.GetAsync(endpoint);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    private async Task<string> PostAsync(string endpoint, object body)
    {
        var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
        var response = await _client.PostAsync(endpoint, content);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    private async Task<string> PutAsync(string endpoint, object body)
    {
        var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
        var response = await _client.PutAsync(endpoint, content);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    private async Task<string> DeleteAsync(string endpoint)
    {
        var response = await _client.DeleteAsync(endpoint);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}
