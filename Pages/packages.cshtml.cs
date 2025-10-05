using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace india.ttlholidays.com.Pages
{
    public class packagesModel : PageModel
    {
        public string Package { get; set; } = string.Empty;
        public void OnGet(string? package)
        {
            Package = package ?? "Amritsar";
        }
    }
}
