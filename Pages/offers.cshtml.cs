using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace india.ttlholidays.com.Pages
{
    public class offerModel : PageModel
    {
        public string bgimage = "assets/img/innerpages/breadcrumb-bg3.jpg";
        public int OfferCount { get; set; }
        public JsonElement OffersRoot { get; set; } 
        public async Task OnGetAsync()
        {
            var apiUrl = "https://docket.ttlholidays.com/api/india/get_offers_listing.php"; 
            using var client = new HttpClient();
            try
            {
                var json = await client.GetStringAsync(apiUrl);
                json = System.Text.Encoding.UTF8.GetString(System.Text.Encoding.Default.GetBytes(json));
                var root = JsonDocument.Parse(json).RootElement;
                OfferCount = root.TryGetProperty("count", out var countProp) ? countProp.GetInt32() : 0;
                OffersRoot = root.GetProperty("offers");
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("API call failed: " + ex.Message);
            }
        }
    }
}
