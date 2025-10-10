using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace india.ttlholidays.com.Pages
{
    public class HolidaysModel : PageModel
    {
        public string bgimage = "assets/img/innerpages/breadcrumb-bg3.jpg";
        public int PackageCount { get; set; }
        public JsonElement PackageRoot { get; set; }
        public async Task OnGetAsync()
        {
            var apiUrl = "https://docket.ttlholidays.com/api/india/get_package_listing.php";
            using var client = new HttpClient();
            try
            {
                var json = await client.GetStringAsync(apiUrl);
                json = System.Text.Encoding.UTF8.GetString(System.Text.Encoding.Default.GetBytes(json));
                var root = JsonDocument.Parse(json).RootElement;
                PackageCount = root.TryGetProperty("count", out var countProp) ? countProp.GetInt32() : 0;
                PackageRoot = root.GetProperty("packages");

            }
            catch (Exception ex)
            {
                Console.WriteLine("API call failed: " + ex.Message);
            }
        }
    }
}
