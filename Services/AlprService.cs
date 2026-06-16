using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using mYPMS.Models;

namespace mYPMS.Services
{
    public interface IAlprService
    {
        Task<List<PlateInfo>> GetLatestPlatesAsync(int limit = 20);
        Task<PlateInfo?> GetLatestPlateAsync();
        Task<bool> IsAliveAsync();
    }

    public class AlprService : IAlprService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public AlprService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _baseUrl = Environment.GetEnvironmentVariable("ALPR_BASE_URL") ?? "http://localhost:8000";
            _httpClient.BaseAddress = new Uri(_baseUrl);
            _httpClient.Timeout = TimeSpan.FromSeconds(10);
        }

        public async Task<List<PlateInfo>> GetLatestPlatesAsync(int limit = 20)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/plates/latest?limit={limit}");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<PlateResponse>();
                    return result?.Data ?? new List<PlateInfo>();
                }
                return new List<PlateInfo>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting plates from API: {ex.Message}");
                return new List<PlateInfo>();
            }
        }

        public async Task<PlateInfo?> GetLatestPlateAsync()
        {
            var plates = await GetLatestPlatesAsync(1);
            return plates.Count > 0 ? plates[0] : null;
        }

        public async Task<bool> IsAliveAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/health");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }

    public class PlateResponse
    {
        public bool Success { get; set; }
        public List<PlateInfo> Data { get; set; } = new();
        public int Total { get; set; }
    }

    public class PlateInfo
    {
        public string Plate { get; set; } = string.Empty;
        public double Confidence { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CameraIdentifier { get; set; } = string.Empty;
        public string SnapshotPath { get; set; } = string.Empty;
    }
}