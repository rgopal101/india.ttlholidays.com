using System;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace india.ttlholidays.com.Pages
{
    public class destinationModel : PageModel
    {
        private readonly IConfiguration _config;

        // ✅ Constructor injection for IConfiguration
        public destinationModel(IConfiguration config)
        {
            _config = config;
        }
        
        public string bgimage = "assets/img/innerpages/breadcrumb-bg3.jpg";
        public int PackageCount { get; set; }
        public JsonElement CityRoot { get; set; }
        public string IMGURL { get; set; } = string.Empty;
        public string APIURL { get; set; } = string.Empty;
        public string Pagetitle { get; set; } = string.Empty;
        public async Task OnGetAsync(string? destination)
        {
            IMGURL = _config["AppSettings:IMGURL"] ?? string.Empty;
            APIURL = _config["AppSettings:APIURL"] ?? string.Empty;
            var apiUrl = $"{APIURL}get_destination_listing.php?destination={System.Net.WebUtility.UrlEncode(destination)}";

            using var client = new HttpClient();
            try
            {
                var json = await client.GetStringAsync(apiUrl);
                json = System.Text.Encoding.UTF8.GetString(System.Text.Encoding.Default.GetBytes(json));
                var root = JsonDocument.Parse(json).RootElement;
                //Pagetitle= root.TryGetProperty("destination", out var titleProp) ? titleProp.GetString(): string.Empty;
                PackageCount = root.TryGetProperty("count", out var countProp) ? countProp.GetInt32() : 0;
                CityRoot = root.GetProperty("citydata");

            }
            catch (Exception ex)
            {
                Console.WriteLine("API call failed: " + ex.Message);
            }
        }
    }
}
