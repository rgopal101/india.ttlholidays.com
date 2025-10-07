using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace india.ttlholidays.com.Pages
{
    public class pagesModel : PageModel
    {
        public string pageId { get; set; } = string.Empty;
        public string? pageUrl { get; set; }
        public string? metaTags { get; set; }
        public string? metaDesc { get; set; }
        public string? pageTitle { get; set; }
        public string? pageContent { get; set; }
        public JsonElement OffersRoot { get; set; }

        public async Task OnGetAsync(string? pagename)
        {
            if (string.IsNullOrEmpty(pagename))
                pagename = "aboutus"; // default fallback

            var apiUrl = $"https://docket.ttlholidays.com/api/india/get_page_detail.php?url={System.Net.WebUtility.UrlEncode(pagename)}";

            using var client = new HttpClient();
            try
            {
                var response = await client.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();

                using var document = JsonDocument.Parse(json);
                var root = document.RootElement;
                pageId = GetJsonValue(root, "pageId");
                pageUrl = GetJsonValue(root, "pageUrl");
                metaTags = GetJsonValue(root, "metaTags");
                metaDesc = GetJsonValue(root, "metaDesc"); 
                pageTitle = GetJsonValue(root, "pageTitle");
                pageContent = GetJsonValue(root, "pageContent");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Offer detail API error: " + ex.Message);
            }
        }

        private string? GetJsonValue(JsonElement element, string propertyName)
        {
            if (element.TryGetProperty(propertyName, out JsonElement value))
            {
                return value.ValueKind == JsonValueKind.String ? value.GetString() : value.ToString();
            }
            return null;
        }
    }
}
