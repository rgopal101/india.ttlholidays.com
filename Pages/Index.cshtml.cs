using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System;
namespace india.ttlholidays.com.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }
        public JsonElement OffersRoot { get; set; }

        public async Task OnGetAsync()
        {
            await LoadOffersListAsync();
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

    }
}