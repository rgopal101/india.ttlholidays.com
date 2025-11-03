using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static india.ttlholidays.com.Pages.VisaModel;
using System.Collections.Generic;
using System.Linq;

namespace india.ttlholidays.com.Pages
{
    public class countryDetailModel : PageModel
    {
        // Property to hold destination data for the view
        public List<Destination> DestinationDetails { get; set; } = new();

        public void OnGet(string searchQuery = "")
        {
            BindData(searchQuery);
        }

        private void BindData(string searchQuery = "")
        {
            var allDestinations = new List<Destination>
            {
                new Destination
                {
                    Name = "Switzerland",
                    ImageUrl = "assets/img/trip/dest_2_1.jpg",
                    Days = "12 Days",
                    Description = "Switzerland is known for its mountains, lakes, and picturesque landscapes.",
                    Checklist = new List<string> { "Passport", "Visa Application", "Photographs" },
                    DocumentsRequired = new List<string> { "Passport", "Visa Application", "Photographs", "Bank Statement" },
                    ProcessingDays = 20,
                    Universities = new List<string> { "ETH Zurich", "University of Geneva" },
                    ApplicationCost = 100.00m,
                    TotalFundRequired = 30000.00m
                },
                new Destination
                {
                    Name = "Spain",
                    ImageUrl = "assets/img/destination/h3-barcelona-resumption.jpg",
                    Days = "10 Days",
                    Description = "Barcelona is famous for its architecture, beaches, and vibrant nightlife.",
                    Checklist = new List<string> { "Passport", "Visa Application", "Photographs" },
                    DocumentsRequired = new List<string> { "Passport", "Visa Application", "Photographs", "Bank Statement" },
                    ProcessingDays = 15,
                    Universities = new List<string> { "University of Barcelona", "Pompeu Fabra University" },
                    ApplicationCost = 90.00m,
                    TotalFundRequired = 25000.00m
                },
                new Destination
                {
                    Name = "Netherlands",
                    ImageUrl = "assets/img/trip/dest_2_3.jpg",
                    Days = "15 Days",
                    Description = "Netherlands is known for its canals, tulips, and rich history.",
                    Checklist = new List<string> { "Passport", "Visa Application", "Photographs" },
                    DocumentsRequired = new List<string> { "Passport", "Visa Application", "Photographs", "Bank Statement" },
                    ProcessingDays = 20,
                    Universities = new List<string> { "University of Amsterdam", "VU University Amsterdam" },
                    ApplicationCost = 100.00m,
                    TotalFundRequired = 30000.00m
                },
                new Destination
                {
                    Name = "France",
                    ImageUrl = "assets/img/trip/dest_2_4.jpg",
                    Days = "8 Days",
                    Description = "Paris is famous for its art, culture, and romantic atmosphere.",
                    Checklist = new List<string> { "Passport", "Visa Application", "Photographs" },
                    DocumentsRequired = new List<string> { "Passport", "Visa Application", "Photographs", "Bank Statement" },
                    ProcessingDays = 15,
                    Universities = new List<string> { "Sorbonne University", "Ecole Polytechnique" },
                    ApplicationCost = 90.00m,
                    TotalFundRequired = 25000.00m
                },
                new Destination
                {
                    Name = "Maldives",
                    ImageUrl = "assets/img/trip/dest_2_5.jpg",
                    Days = "12 Days",
                    Description = "The Maldives is a tropical paradise with beautiful beaches and crystal-clear waters.",
                    Checklist = new List<string> { "Passport", "Visa Application", "Photographs" },
                    DocumentsRequired = new List<string> { "Passport", "Visa Application", "Photographs", "Bank Statement" },
                    ProcessingDays = 10,
                    Universities = new List<string> { "Maldives National University" },
                    ApplicationCost = 50.00m,
                    TotalFundRequired = 10000.00m
                }
            };

            // 🔍 Filter by search query
            if (!string.IsNullOrEmpty(searchQuery))
            {
                allDestinations = allDestinations
                    .Where(d => d.Name.Contains(searchQuery, System.StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // ✅ Assign filtered results to property for Razor view
            DestinationDetails = allDestinations;
        }
    }
}