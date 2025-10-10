using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace india.ttlholidays.com.ViewComponents
{
    public class OffersMenuViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var apiUrl = "https://docket.ttlholidays.com/api/india/get_offers_menu.php";
            using var client = new HttpClient();

            try
            {
                var json = await client.GetStringAsync(apiUrl);
                var root = JsonDocument.Parse(json).RootElement;

                if (root.TryGetProperty("offers", out JsonElement offers))
                    return View(offers);
                else
                    return View(null);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching offers: " + ex.Message);
                return View(null);
            }
        }
    }
}
