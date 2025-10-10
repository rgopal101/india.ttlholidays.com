using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
namespace india.ttlholidays.com.Pages
{
    public class packageDetailsModel : PageModel
    {
        private readonly IConfiguration _config;

        // ✅ Constructor injection for IConfiguration
        public packageDetailsModel(IConfiguration config)
        {
            _config = config;
        }


        public string packageId { get; set; } = string.Empty;

        public string packageName { get; set; } = string.Empty;
        public string categoryId { get; set; } = string.Empty;
        public string subCategoryId { get; set; } = string.Empty;
        public string cityId { get; set; } = string.Empty;
        public string imagePath { get; set; } = string.Empty;
        public string metaKeyword { get; set; } = string.Empty;
        public string metaDescription { get; set; } = string.Empty;
        public string pageTitl { get; set; } = string.Empty;
        public string url { get; set; } = string.Empty;
        public string packagePrice { get; set; } = string.Empty;
        public string packageDesc { get; set; } = string.Empty;
        public string special { get; set; } = string.Empty;
        public string topRated { get; set; } = string.Empty;
        public string itinerary { get; set; } = string.Empty;
        public string inclusion { get; set; } = string.Empty;
        public string packageType { get; set; } = string.Empty;

        public string rootdata { get; set; }

        public  decimal packagePriceValue = 0;

        public string IMGURL { get; set; } = string.Empty;
        public string APIURL { get; set; } = string.Empty;

        // ----------------------
        // Main GET logic
        // ----------------------
        public async Task OnGetAsync(string? pkg_name)
        {
            // Load global variables from config
            IMGURL = _config["AppSettings:IMGURL"] ?? string.Empty;
            APIURL = _config["AppSettings:APIURL"] ?? string.Empty;

            // Default page name fallback
            if (string.IsNullOrEmpty(pkg_name))
                pkg_name = "aboutus";

            var apiUrl = $"{APIURL}get_package_detail.php?url={System.Net.WebUtility.UrlEncode(pkg_name)}";

            using var client = new HttpClient();
            try
            {
                var response = await client.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                using var document = JsonDocument.Parse(json);
                var root = document.RootElement;
                rootdata = json;
                packageId = GetJsonValue(root, "packageId");
                packageName = GetJsonValue(root, "packageName");
                categoryId = GetJsonValue(root, "categoryId");
                subCategoryId = GetJsonValue(root, "subCategoryId");
                cityId = GetJsonValue(root, "cityId");
                imagePath = GetJsonValue(root, "imagePath");
                imagePath = IMGURL + imagePath;
                metaKeyword = GetJsonValue(root, "metaKeyword");
                metaDescription = GetJsonValue(root, "metaDescription");
                pageTitl = GetJsonValue(root, "pageTitl");
                url = GetJsonValue(root, "url");
                packagePrice = GetJsonValue(root, "packagePrice");


                if (root.TryGetProperty("packagePrice", out JsonElement priceElement))
                {
                    // Get the string value
                    string priceStr = priceElement.GetString() ?? "0";

                    // Parse to decimal
                    decimal packagePrice = 0;
                    decimal.TryParse(priceStr, out packagePrice);

                    // Increase by 10%
                    packagePriceValue = packagePrice + (packagePrice * 0.10m);

                    // Optional: round to 2 decimals
                    packagePriceValue = Math.Round(packagePriceValue);

                    Console.WriteLine($"Original: {packagePrice}, +10%: {packagePriceValue}");
                }


                packageDesc = GetJsonValue(root, "packageDesc");
                special = GetJsonValue(root, "special");
                topRated = GetJsonValue(root, "topRated");
                itinerary = GetJsonValue(root, "itinerary");
                inclusion = GetJsonValue(root, "inclusion");
                packageType = GetJsonValue(root, "packageType"); 

                if (!string.IsNullOrEmpty(packageDesc))
                {
                    packageDesc = Regex.Replace(packageDesc, "(?i)<img([^>]+)src=[\"']\\.\\.\\/([^\"']+)[\"']", m => $"<img{m.Groups[1].Value}src=\"{IMGURL}{m.Groups[2].Value}\"", RegexOptions.IgnoreCase);
                    packageDesc = Regex.Replace(packageDesc, @"<div\s+class\s*=\s*[""']container[""'][^>]*>(.*?)<\/div>", "$1", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Page API error: " + ex.Message);
                pageTitl = "Error loading page";
                packageDesc = "Unable to load content at the moment.";
            }
        }

        // ----------------------
        // Helper function to read JSON values
        // ----------------------
        private string GetJsonValue(JsonElement element, string propertyName)
        {
            if (element.TryGetProperty(propertyName, out JsonElement value))
            {
                if (value.ValueKind == JsonValueKind.String)
                    return value.GetString() ?? string.Empty;

                return value.ToString();
            }
            return string.Empty;
        }

    }
}
