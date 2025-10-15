using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace india.ttlholidays.com.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        // Example property to store JSON data
        public System.Text.Json.JsonElement? JsonData { get; set; }

        public void OnGet()
        {
            // Example: initializing JsonData
            // JsonData = ... fetch JSON from API
        }
    }
}
