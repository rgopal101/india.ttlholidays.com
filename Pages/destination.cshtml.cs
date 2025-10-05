using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace india.ttlholidays.com.Pages
{
    public class destinationModel : PageModel
    {        public string Destination { get; set; } = string.Empty;
        public void OnGet(string? destination)
        {
            Destination = destination ?? "asia";
        }
    }
}
