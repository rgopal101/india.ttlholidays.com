using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace india.ttlholidays.com.ViewComponents
{
    public class PackageMenuViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var apiUrl = "https://docket.ttlholidays.com/api/india/get_package_menu.php";
            using var client = new HttpClient();

            try
            {
                var json = await client.GetStringAsync(apiUrl);
                var root = JsonDocument.Parse(json).RootElement;

                // Expecting: { "count": 56, "packages": [ ... ] }
                if (root.TryGetProperty("packages", out JsonElement packages))
                {
                    return View(packages);
                }

                // fallback
                return View(null);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching packages: " + ex.Message);
                return View(null);
            }
        }
    }
}
