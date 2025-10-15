using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace india.ttlholidays.com.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration _config;

        // ✅ Single constructor for DI
        public IndexModel(ILogger<IndexModel> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        public JsonElement OffersRoot { get; set; }
        public JsonElement IndiaRoot { get; set; }
        public JsonElement InternationalRoot { get; set; }
        public string IMGURL { get; set; } = string.Empty;
        public string APIURL { get; set; } = string.Empty;

        public async Task OnGetAsync()
        {
            await LoadOffersListAsync();
            await LoadIndiaListAsync();
            await LoadInternationalListAsync();
        }

        private async Task LoadOffersListAsync()
        {
            var apiUrl = "https://docket.ttlholidays.com/api/india/get_offers_listing.php";
            using var client = new HttpClient();
            try
            {
                var json = await client.GetStringAsync(apiUrl);
                json = System.Text.Encoding.UTF8.GetString(System.Text.Encoding.Default.GetBytes(json));
                var root = JsonDocument.Parse(json).RootElement;
                OffersRoot = root.GetProperty("offers");
            }
            catch (Exception ex)
            {
                _logger.LogError("API call failed for offers: {0}", ex.Message);
            }
        }

        private async Task LoadIndiaListAsync()
        {
            IMGURL = _config["AppSettings:IMGURL"] ?? string.Empty;
            APIURL = _config["AppSettings:APIURL"] ?? string.Empty;

            var apiUrl = $"{APIURL}get_destination_listing.php?destination={System.Net.WebUtility.UrlEncode("India")}";
            using var client = new HttpClient();
            try
            {
                var json = await client.GetStringAsync(apiUrl);
                json = System.Text.Encoding.UTF8.GetString(System.Text.Encoding.Default.GetBytes(json));
                var root = JsonDocument.Parse(json).RootElement;
                IndiaRoot = root.GetProperty("citydata");
            }
            catch (Exception ex)
            {
                _logger.LogError("API call failed for India destinations: {0}", ex.Message);
            }
        }

        private async Task LoadInternationalListAsync()
        {
            IMGURL = _config["AppSettings:IMGURL"] ?? string.Empty;
            APIURL = _config["AppSettings:APIURL"] ?? string.Empty;

            var apiUrl = $"{APIURL}get_destination_listing.php?destination={System.Net.WebUtility.UrlEncode("International")}";
            using var client = new HttpClient();
            try
            {
                var json = await client.GetStringAsync(apiUrl);
                json = System.Text.Encoding.UTF8.GetString(System.Text.Encoding.Default.GetBytes(json));
                var root = JsonDocument.Parse(json).RootElement;
                InternationalRoot = root.GetProperty("citydata");
            }
            catch (Exception ex)
            {
                _logger.LogError("API call failed for International destinations: {0}", ex.Message);
            }
        }
    }
}
