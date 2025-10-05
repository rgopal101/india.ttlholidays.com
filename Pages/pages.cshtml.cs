using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace india.ttlholidays.com.Pages
{
    public class pagesModel : PageModel
    {
        public string PageName { get; set; } = string.Empty; 
        public void OnGet(string? pagename)
        {
            PageName = pagename ?? "about";
        }
    }
}
