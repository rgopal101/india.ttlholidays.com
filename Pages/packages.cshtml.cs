using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace india.ttlholidays.com.Pages
{
    public class packagesModel : PageModel
    {
        private readonly IConfiguration _config;

        // ✅ Constructor injection for IConfiguration
        public packagesModel(IConfiguration config)
        {
            _config = config;
        }
        
        public string bgimage = "/assets/img/innerpages/breadcrumb-bg3.jpg";
        public int PackageCount { get; set; }
        public JsonElement PackageRoot { get; set; }
        public string IMGURL { get; set; } = string.Empty;
        public string APIURL { get; set; } = string.Empty;
        public string pageTitle { get; set; } = string.Empty; 


        public async Task OnGetAsync(string? cityname)
        {
            IMGURL = _config["AppSettings:IMGURL"] ?? string.Empty;
            APIURL = _config["AppSettings:APIURL"] ?? string.Empty;
            pageTitle = cityname;
            var apiUrl = $"{APIURL}get_package_listing.php?cityname={System.Net.WebUtility.UrlEncode(cityname)}";
            using var client = new HttpClient();
            try
            {
                var json = await client.GetStringAsync(apiUrl);
                json = System.Text.Encoding.UTF8.GetString(System.Text.Encoding.Default.GetBytes(json));
                var root = JsonDocument.Parse(json).RootElement;
                bgimage = root.TryGetProperty("cityimg", out var cityImgProp) ? cityImgProp.GetString (): "";
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
