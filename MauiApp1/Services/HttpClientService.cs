using MauiApp1.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MauiApp1.Services
{
    public class HttpClientService
    {
        private readonly HttpClient _httpClient;

        public HttpClientService(HttpClient httpClient)
        {
            // Ignore SSL certificate validation (for testing purposes)
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };
            _httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri("http://localhost:7054/")
            };
        }

        public async Task<bool> SetConnectionStringAsync(string connectionString)
        {
            var content = new StringContent(JsonSerializer.Serialize(new { ConnectionString = connectionString }), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/Database/SetConnectionString", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<Inventory>?> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Inventory>>("/api/Inventory");
        }

        public async Task<Inventory?> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Inventory>($"/api/Inventory/{id}");
        }

        public async Task AddProductAsync(Inventory inventory)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/Inventory", inventory);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateProductAsync(int id, Inventory inventory)
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/Inventory/{id}", inventory);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteProductAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/Inventory/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
