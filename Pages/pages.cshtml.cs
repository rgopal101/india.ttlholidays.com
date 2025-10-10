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
    public class pagesModel : PageModel
    {
        private readonly IConfiguration _config;

        // ✅ Constructor injection for IConfiguration
        public pagesModel(IConfiguration config)
        {
            _config = config;
        }

        // ----------------------
        // Page properties
        // ----------------------
        public string pageId { get; set; } = string.Empty;
        public string? pageUrl { get; set; }
        public string? metaTags { get; set; }
        public string? metaDesc { get; set; }
        public string? pageTitle { get; set; }
        public string? pageContent { get; set; }
        public JsonElement OffersRoot { get; set; }

        // ✅ Global variables (auto-filled from appsettings.json)
        public string IMGURL { get; set; } = string.Empty;
        public string APIURL { get; set; } = string.Empty;

        // ----------------------
        // Main GET logic
        // ----------------------
        public async Task OnGetAsync(string? pagename)
        {
            // Load global variables from config
            IMGURL = _config["AppSettings:IMGURL"] ?? string.Empty;
            APIURL = _config["AppSettings:APIURL"] ?? string.Empty;

            // Default page name fallback
            if (string.IsNullOrEmpty(pagename))
                pagename = "aboutus";

            var apiUrl = $"{APIURL}get_page_detail.php?url={System.Net.WebUtility.UrlEncode(pagename)}";

            using var client = new HttpClient();
            try
            {
                var response = await client.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();

                using var document = JsonDocument.Parse(json);
                var root = document.RootElement;

                // Assign values safely
                pageId = GetJsonValue(root, "pageId");
                pageUrl = GetJsonValue(root, "pageUrl");
                metaTags = GetJsonValue(root, "metaTags");
                metaDesc = GetJsonValue(root, "metaDesc");
                pageTitle = GetJsonValue(root, "pageTitle");
                pageContent = GetJsonValue(root, "pageContent");

                if (!string.IsNullOrEmpty(pageContent))
                {
                    pageContent = Regex.Replace(pageContent,"(?i)<img([^>]+)src=[\"']\\.\\.\\/([^\"']+)[\"']", m => $"<img{m.Groups[1].Value}src=\"{IMGURL}{m.Groups[2].Value}\"",RegexOptions.IgnoreCase);
                    pageContent = Regex.Replace(pageContent, @"<div\s+class\s*=\s*[""']container[""'][^>]*>(.*?)<\/div>","$1", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Page API error: " + ex.Message);
                pageTitle = "Error loading page";
                pageContent = "Unable to load content at the moment.";
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
