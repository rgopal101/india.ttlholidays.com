using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace india.ttlholidays.com.Pages
{
    public class VisaModel : PageModel   
    {
        [BindProperty]
        public string SearchQuery { get; set; } = string.Empty;

        public List<Destination> TouristVisaDestinations { get; set; } = new();
        public List<Destination> StudyVisaDestinations { get; set; } = new();

        public void OnGet()
        {
            BindData();
        }

        public void OnPost()
        {
            BindData(SearchQuery);
        }

        private void BindData(string searchQuery = "")
        {
            var touristVisaDestinations = new List<Destination>
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
            },
            new Destination
            {
                Name = "Australia",
                ImageUrl = "assets/img/destination/h2-melbourne-beach.jpg",
                Days = "7 Days",
                Description = "Australia is known for its diverse landscapes, from the Great Barrier Reef to the Outback.",
                Checklist = new List<string> { "Passport", "Visa Application", "Photographs" },
                DocumentsRequired = new List<string> { "Passport", "Visa Application", "Photographs", "Bank Statement" },
                ProcessingDays = 30,
                Universities = new List<string> { "University of Sydney", "University of Melbourne" },
                ApplicationCost = 140.00m,
                TotalFundRequired = 40000.00m
            },
            new Destination
            {
                Name = "Egypt",
                ImageUrl = "assets/img/destination/h3-egypt-pyramids-camels.jpg",
                Days = "5 Days",
                Description = "Egypt is famous for its ancient pyramids and rich history.",
                Checklist = new List<string> { "Passport", "Visa Application", "Photographs" },
                DocumentsRequired = new List<string> { "Passport", "Visa Application", "Photographs", "Bank Statement" },
                ProcessingDays = 10,
                Universities = new List<string> { "Cairo University", "Alexandria University" },
                ApplicationCost = 70.00m,
                TotalFundRequired = 15000.00m
            },
            new Destination
            {
                Name = "USA",
                ImageUrl = "assets/img/destination/h3-washington.jpg",
                Days = "18 Days",
                Description = "The United States of America is a popular destination for tourists and students alike.",
                Checklist = new List<string> { "Passport", "Visa Application", "Photographs" },
                DocumentsRequired = new List<string> { "Passport", "Visa Application", "Photographs", "Bank Statement" },
                ProcessingDays = 30,
                Universities = new List<string> { "Harvard", "MIT", "Stanford" },
                ApplicationCost = 160.00m,
                TotalFundRequired = 50000.00m
            },
            new Destination
            {
                Name = "Denmark",
                ImageUrl = "assets/img/destination/AMS.jpg",
                Days = "10 Days",
                Description = "Denmark is known for its design, architecture, and quality of life.",
                Checklist = new List<string> { "Passport", "Visa Application", "Photographs" },
                DocumentsRequired = new List<string> { "Passport", "Visa Application", "Photographs", "Bank Statement" },
                ProcessingDays = 20,
                Universities = new List<string> { "University of Copenhagen", "Technical University of Denmark" },
                ApplicationCost = 100.00m,
                TotalFundRequired = 30000.00m
            },
            new Destination
            {
                Name = "Abu Dhabi",
                ImageUrl = "assets/img/destination/h3-abu-dhabi-grand-mosque.jpg",
                Days = "8 Days",
                Description = "Abu Dhabi is known for its modern architecture and cultural heritage.",
                Checklist = new List<string> { "Passport", "Visa Application", "Photographs" },
                DocumentsRequired = new List<string> { "Passport", "Visa Application", "Photographs", "Bank Statement" },
                ProcessingDays = 15,
                Universities = new List<string> { "Khalifa University", "New York University Abu Dhabi" },
                ApplicationCost = 120.00m,
                TotalFundRequired = 20000.00m
            },
            new Destination
            {
                Name = "China",
                ImageUrl = "assets/img/destination/h3-beijing-city.jpg",
                Days = "22 Days",
                Description = "China is a vast country with a rich history and diverse landscapes.",
                Checklist = new List<string> { "Passport", "Visa Application", "Photographs" },
                DocumentsRequired = new List<string> { "Passport", "Visa Application", "Photographs", "Bank Statement" },
                ProcessingDays = 30,
                Universities = new List<string> { "Tsinghua University", "Peking University" },
                ApplicationCost = 150.00m,
                TotalFundRequired = 40000.00m
            },
            new Destination
            {
                Name = "Greece",
                ImageUrl = "assets/img/destination/h3-santorini-greece.jpg",
                Days = "6 Days",
                Description = "Greece is known for its beautiful islands, ancient ruins, and Mediterranean cuisine.",
                Checklist = new List<string> { "Passport", "Visa Application", "Photographs" },
                DocumentsRequired = new List<string> { "Passport", "Visa Application", "Photographs", "Bank Statement" },
                ProcessingDays = 15,
                Universities = new List<string> { "National and Kapodistrian University of Athens", "Aristotle University of Thessaloniki" },
                ApplicationCost = 90.00m,
                TotalFundRequired = 20000.00m
            }
            };

            var studyVisaDestinations = new List<Destination>
            { 
               
               new Destination
            {
                Name = "England",
                ImageUrl = "assets/img/visa/englandstudy.jpg",
                Days = "30 Days",
                Description = "England is a popular destination for international students.",
                Checklist = new List<string> { "Passport", "Visa Application", "Photographs" },
                DocumentsRequired = new List<string> { "Passport", "Visa Application", "Photographs", "Bank Statement" },
                ProcessingDays = 20,
                Universities = new List<string> { "Oxford", "Cambridge", "Imperial College London" },
                ApplicationCost = 150.00m,
                TotalFundRequired = 40000.00m
            },
            new Destination
            {
                Name = "Germany",
                ImageUrl = "assets/img/visa/germanystudy.jpg",
                Days = "22 Days",
                Description = "Germany is known for its engineering, research, and higher education.",
                Checklist = new List<string> { "Passport", "Visa Application", "Photographs" },
                DocumentsRequired = new List<string> { "Passport", "Visa Application", "Photographs", "Bank Statement" },
                ProcessingDays = 25,
                Universities = new List<string> { "Technical University of Munich", "Heidelberg University" },
                ApplicationCost = 120.00m,
                TotalFundRequired = 35000.00m
            },
            new Destination
            {
                Name = "USA",
                ImageUrl = "assets/img/visa/usastudy.jpg",
                Days = "55 Days",
                Description = "The United States of America is a popular destination for tourists and students alike.",
                Checklist = new List<string> { "Passport", "Visa Application", "Photographs" },
                DocumentsRequired = new List<string> { "Passport", "Visa Application", "Photographs", "Bank Statement" },
                ProcessingDays = 30,
                Universities = new List<string> { "Harvard", "MIT", "Stanford" },
                ApplicationCost = 160.00m,
                TotalFundRequired = 50000.00m
            },
            new Destination
            {
                Name = "Australia",
                ImageUrl = "assets/img/visa/australiastudy.png",
                Days = "40 Days",
                Description = "Australia is known for its diverse landscapes, from the Great Barrier Reef to the Outback.",
                Checklist = new List<string> { "Passport", "Visa Application", "Photographs" },
                DocumentsRequired = new List<string> { "Passport", "Visa Application", "Photographs", "Bank Statement" },
                ProcessingDays = 30,
                Universities = new List<string> { "University of Sydney", "University of Melbourne" },
                ApplicationCost = 140.00m,
                TotalFundRequired = 40000.00m
            }
            };

            if (!string.IsNullOrEmpty(searchQuery))
            {
                touristVisaDestinations = touristVisaDestinations
                    .Where(d => d.Name.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                studyVisaDestinations = studyVisaDestinations
                    .Where(d => d.Name.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            TouristVisaDestinations = touristVisaDestinations;
            StudyVisaDestinations = studyVisaDestinations;
        }

        public class Destination
        {
            public string Name { get; set; } = "";
            public string ImageUrl { get; set; } = "";
            public string Days { get; set; } = "";
            public string Description { get; set; } = "";
            public List<string> Checklist { get; set; } = new();
            public List<string> DocumentsRequired { get; set; } = new();
            public int ProcessingDays { get; set; }
            public List<string> Universities { get; set; } = new();
            public decimal ApplicationCost { get; set; }
            public decimal TotalFundRequired { get; set; }
        }
    }
}
