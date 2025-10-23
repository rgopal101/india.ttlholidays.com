using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace india.ttlholidays.com.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMemoryCache _cache;

        public IndexModel(
            ILogger<IndexModel> logger,
            IConfiguration config,
            IHttpClientFactory httpClientFactory,
            IMemoryCache cache)
        {
            _logger = logger;
            _config = config;
            _httpClientFactory = httpClientFactory;
            _cache = cache;
        }

        public JsonElement OffersRoot { get; set; }
        public JsonElement IndiaRoot { get; set; }
        public JsonElement InternationalRoot { get; set; }
        public JsonElement PackageRoot { get; set; }

        public string IMGURL { get; set; } = string.Empty;
        public string APIURL { get; set; } = string.Empty;

        public async Task OnGetAsync()
        {
            IMGURL = _config["AppSettings:IMGURL"] ?? string.Empty;
            APIURL = _config["AppSettings:APIURL"] ?? string.Empty;

            // ✅ Run all API calls in parallel for faster loading
            var offersTask = GetCachedJsonAsync($"{APIURL}get_offers_listing.php", "OffersRoot");
            var indiaTask = GetCachedJsonAsync($"{APIURL}get_destination_listing.php?destination=India", "IndiaRoot");
            var internationalTask = GetCachedJsonAsync($"{APIURL}get_destination_listing.php?destination=International", "InternationalRoot");
            var packageTask = GetCachedJsonAsync($"{APIURL}get_package_listing.php?limit=10", "PackageRoot");

            await Task.WhenAll(offersTask, indiaTask, internationalTask, packageTask);

            try
            {
                OffersRoot = offersTask.Result.GetProperty("offers");
            }
            catch { }

            try
            {
                IndiaRoot = indiaTask.Result.GetProperty("citydata");
            }
            catch { }

            try
            {
                InternationalRoot = internationalTask.Result.GetProperty("citydata");
            }
            catch { }

            try
            {
                PackageRoot = packageTask.Result.GetProperty("packages");
            }
            catch { }
        }

        /// <summary>
        /// ✅ Cached + shared HttpClient + async + error-handled API fetch
        /// </summary>
        private async Task<JsonElement> GetCachedJsonAsync(string url, string cacheKey)
        {
            if (_cache.TryGetValue(cacheKey, out JsonElement cachedData))
                return cachedData;

            var client = _httpClientFactory.CreateClient();

            try
            {
                var json = await client.GetStringAsync(url);
                json = System.Text.Encoding.UTF8.GetString(System.Text.Encoding.Default.GetBytes(json));

                var root = JsonDocument.Parse(json).RootElement;

                // Cache the response for 10 minutes
                _cache.Set(cacheKey, root, TimeSpan.FromMinutes(10));

                return root;
            }
            catch (Exception ex)
            {
                _logger.LogError("API call failed for {CacheKey}: {Error}", cacheKey, ex.Message);
                return JsonDocument.Parse("{}").RootElement;
            }
        }
    }
}
