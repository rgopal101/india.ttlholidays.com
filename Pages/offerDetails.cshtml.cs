using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System;

namespace india.ttlholidays.com.Pages
{
    public class offerDetailsModel : PageModel
    {
        public string? title { get; set; }
        public string? description { get; set; }
        public string? imagePath { get; set; }
        public string? shortDesc { get; set; }
        public string? dateTime { get; set; }
       public JsonElement OffersRoot { get; set; } 

        public async Task OnGetAsync(string? offer_name)
        {
            
                // Load the offers listing if no offer name is provided
                await LoadOffersListAsync();
             
                // Load details of a specific offer
                await LoadOfferDetailsAsync(offer_name);
             
        }

        private async Task LoadOfferDetailsAsync(string offerName)
        {
            var apiUrl = $"https://docket.ttlholidays.com/api/india/get_offers_detail.php?url={System.Net.WebUtility.UrlEncode(offerName)}";

            using var client = new HttpClient();
            try
            {
                var response = await client.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();

                using var document = JsonDocument.Parse(json);
                var root = document.RootElement;

                title = GetJsonValue(root, "Title");
                description = GetJsonValue(root, "description");
                imagePath = GetJsonValue(root, "imagePath");
                shortDesc = GetJsonValue(root, "shortDesc");
                dateTime = GetJsonValue(root, "dateTime");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Offer detail API error: " + ex.Message);
            }
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
                Console.WriteLine("API call failed: " + ex.Message);
            }
        }

        private static string? GetJsonValue(JsonElement root, string propName)
        {
            foreach (var prop in root.EnumerateObject())
            {
                if (string.Equals(prop.Name, propName, StringComparison.OrdinalIgnoreCase))
                    return prop.Value.ToString();
            }
            return null;
        }
    }
}
