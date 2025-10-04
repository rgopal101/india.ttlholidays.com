using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace india.ttlholidays.com.Pages
{
    public class FlightDetailModel : PageModel
    {
        public string Country { get; set; } = string.Empty;
        public void OnGet(string? country)
        {
            Country = country ?? "India";
        }
    }
}
