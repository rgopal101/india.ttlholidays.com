using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace india.ttlholidays.com.Pages
{
    public class VisaDetailModel : PageModel
    {
        public string Country { get; set; } = string.Empty;
        public string bgimage = "assets/img/innerpages/breadcrumb-bg7.jpg"; 
        public void OnGet(string? country)
        {
            Country = country ?? "India";
        }
    }
}
