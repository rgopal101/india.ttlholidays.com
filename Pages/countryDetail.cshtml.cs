using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace india.ttlholidays.com.Pages
{
    public class countryDetailModel : PageModel
    {
        [BindProperty]
        public string SearchQuery { get; set; } = string.Empty;

        public List<Destination> CountryDestinations { get; set; } = new();

        public void OnGet([FromQuery] string? searchQuery)
        {
            SearchQuery = searchQuery ?? string.Empty;
            BindData(SearchQuery);
        }

        public void OnPost()
        {
            BindData(SearchQuery);
        }

        private void BindData(string searchQuery = "")
        {
            CountryDestinations = new List<Destination>
            {
                new Destination
                {
                    Name = "Barcelona",
                    ImageUrls = new List<string> { "img/destination/h3-barcelona-resumption.webp", "img/destination/h3-barcelona1.webp", "img/destination/h3-barcelona2.webp", "img/destination/h3-barcelona3.webp", "img/destination/h3-barcelona4.webp" },
                    Description = "",
                    Itinerary = new List<string> {},
                    WhatToDo = new List<string> {},
                    Places = new List<string> {},
                    Price = 0m,
                    Inclusions = new List<string> {},
                    Exclusions = new List<string> {},
                },
                new Destination
                {
                    Name = "",
                    ImageUrls = new List<string> {},
                    Description = "",
                    Itinerary = new List<string> {},
                    WhatToDo = new List<string> {},
                    Places = new List<string> {},
                    Price = 0m,
                    Inclusions = new List<string> {},
                    Exclusions = new List<string> {},
                },
                new Destination
                {
                    Name = "",
                    ImageUrls = new List<string> {},
                    Description = "",
                    Itinerary = new List<string> {},
                    WhatToDo = new List<string> {},
                    Places = new List<string> {},
                    Price = 0m,
                    Inclusions = new List<string> {},
                    Exclusions = new List<string> {},
                },
                new Destination
                {
                    Name = "",
                    ImageUrls = new List<string> {},
                    Description = "",
                    Itinerary = new List<string> {},
                    WhatToDo = new List<string> {},
                    Places = new List<string> {},
                    Price = 0m,
                    Inclusions = new List<string> {},
                    Exclusions = new List<string> {},
                },
                new Destination
                {
                    Name = "",
                    ImageUrls = new List<string> {},
                    Description = "",
                    Itinerary = new List<string> {},
                    WhatToDo = new List<string> {},
                    Places = new List<string> {},
                    Price = 0m,
                    Inclusions = new List<string> {},
                    Exclusions = new List<string> {},
                },
                new Destination
                {
                    Name = "",
                    ImageUrls = new List<string> {},
                    Description = "",
                    Itinerary = new List<string> {},
                    WhatToDo = new List<string> {},
                    Places = new List<string> {},
                    Price = 0m,
                    Inclusions = new List<string> {},
                    Exclusions = new List<string> {},
                },
                new Destination
                {
                    Name = "",
                    ImageUrls = new List<string> {},
                    Description = "",
                    Itinerary = new List<string> {},
                    WhatToDo = new List<string> {},
                    Places = new List<string> {},
                    Price = 0m,
                    Inclusions = new List<string> {},
                    Exclusions = new List<string> {},
                },
                new Destination
                {
                    Name = "",
                    ImageUrls = new List<string> {},
                    Description = "",
                    Itinerary = new List<string> {},
                    WhatToDo = new List<string> {},
                    Places = new List<string> {},
                    Price = 0m,
                    Inclusions = new List<string> {},
                    Exclusions = new List<string> {},
                },
                new Destination
                {
                    Name = "",
                    ImageUrls = new List<string> {},
                    Description = "",
                    Itinerary = new List<string> {},
                    WhatToDo = new List<string> {},
                    Places = new List<string> {},
                    Price = 0m,
                    Inclusions = new List<string> {},
                    Exclusions = new List<string> {},
                },
                new Destination
                {
                    Name = "",
                    ImageUrls = new List<string> {},
                    Description = "",
                    Itinerary = new List<string> {},
                    WhatToDo = new List<string> {},
                    Places = new List<string> {},
                    Price = 0m,
                    Inclusions = new List<string> {},
                    Exclusions = new List<string> {},
                },
                new Destination
                {
                    Name = "",
                    ImageUrls = new List<string> {},
                    Description = "",
                    Itinerary = new List<string> {},
                    WhatToDo = new List<string> {},
                    Places = new List<string> {},
                    Price = 0m,
                    Inclusions = new List<string> {},
                    Exclusions = new List<string> {},
                },
                new Destination
                {
                    Name = "",
                    ImageUrls = new List<string> {},
                    Description = "",
                    Itinerary = new List<string> {},
                    WhatToDo = new List<string> {},
                    Places = new List<string> {},
                    Price = 0m,
                    Inclusions = new List<string> {},
                    Exclusions = new List<string> {},
                },
                new Destination
                {
                    Name = "",
                    ImageUrls = new List<string> {},
                    Description = "",
                    Itinerary = new List<string> {},
                    WhatToDo = new List<string> {},
                    Places = new List<string> {},
                    Price = 0m,
                    Inclusions = new List<string> {},
                    Exclusions = new List<string> {},
                },
                new Destination
                {
                    Name = "",
                    ImageUrls = new List<string> {},
                    Description = "",
                    Itinerary = new List<string> {},
                    WhatToDo = new List<string> {},
                    Places = new List<string> {},
                    Price = 0m,
                    Inclusions = new List<string> {},
                    Exclusions = new List<string> {},
                },
                new Destination
                {
                    Name = "",
                    ImageUrls = new List<string> {},
                    Description = "",
                    Itinerary = new List<string> {},
                    WhatToDo = new List<string> {},
                    Places = new List<string> {},
                    Price = 0m,
                    Inclusions = new List<string> {},
                    Exclusions = new List<string> {},
                },
                new Destination
                {
                    Name = "",
                    ImageUrls = new List<string> {},
                    Description = "",
                    Itinerary = new List<string> {},
                    WhatToDo = new List<string> {},
                    Places = new List<string> {},
                    Price = 0m,
                    Inclusions = new List<string> {},
                    Exclusions = new List<string> {},
                },
                new Destination
                {
                    Name = "",
                    ImageUrls = new List<string> {},
                    Description = "",
                    Itinerary = new List<string> {},
                    WhatToDo = new List<string> {},
                    Places = new List<string> {},
                    Price = 0m,
                    Inclusions = new List<string> {},
                    Exclusions = new List<string> {},
                },
                new Destination
                {
                    Name = "",
                    ImageUrls = new List<string> {},
                    Description = "",
                    Itinerary = new List<string> {},
                    WhatToDo = new List<string> {},
                    Places = new List<string> {},
                    Price = 0m,
                    Inclusions = new List<string> {},
                    Exclusions = new List<string> {},
                },
                new Destination
                {
                    Name = "",
                    ImageUrls = new List<string> {},
                    Description = "",
                    Itinerary = new List<string> {},
                    WhatToDo = new List<string> {},
                    Places = new List<string> {},
                    Price = 0m,
                    Inclusions = new List<string> {},
                    Exclusions = new List<string> {},
                },
                new Destination
                {
                    Name = "",
                    ImageUrls = new List<string> {},
                    Description = "",
                    Itinerary = new List<string> {},
                    WhatToDo = new List<string> {},
                    Places = new List<string> {},
                    Price = 0m,
                    Inclusions = new List<string> {},
                    Exclusions = new List<string> {},
                },
                new Destination
                {
                    Name = "",
                    ImageUrls = new List<string> {},
                    Description = "",
                    Itinerary = new List<string> {},
                    WhatToDo = new List<string> {},
                    Places = new List<string> {},
                    Price = 0m,
                    Inclusions = new List<string> {},
                    Exclusions = new List<string> {},
                },
                new Destination
                {
                    Name = "",
                    ImageUrls = new List<string> {},
                    Description = "",
                    Itinerary = new List<string> {},
                    WhatToDo = new List<string> {},
                    Places = new List<string> {},
                    Price = 0m,
                    Inclusions = new List<string> {},
                    Exclusions = new List<string> {},
                },
                new Destination
                {
                    Name = "",
                    ImageUrls = new List<string> {},
                    Description = "",
                    Itinerary = new List<string> {},
                    WhatToDo = new List<string> {},
                    Places = new List<string> {},
                    Price = 0m,
                    Inclusions = new List<string> {},
                    Exclusions = new List<string> {},
                },
                new Destination
                {
                    Name = "",
                    ImageUrls = new List<string> {},
                    Description = "",
                    Itinerary = new List<string> {},
                    WhatToDo = new List<string> {},
                    Places = new List<string> {},
                    Price = 0m,
                    Inclusions = new List<string> {},
                    Exclusions = new List<string> {},
                },
                new Destination
                {
                    Name = "",
                    ImageUrls = new List<string> {},
                    Description = "",
                    Itinerary = new List<string> {},
                    WhatToDo = new List<string> {},
                    Places = new List<string> {},
                    Price = 0m,
                    Inclusions = new List<string> {},
                    Exclusions = new List<string> {},
                },
                new Destination
                {
                    Name = "",
                    ImageUrls = new List<string> {},
                    Description = "",
                    Itinerary = new List<string> {},
                    WhatToDo = new List<string> {},
                    Places = new List<string> {},
                    Price = 0m,
                    Inclusions = new List<string> {},
                    Exclusions = new List<string> {},
                },
                new Destination
                {
                    Name = "",
                    ImageUrls = new List<string> {},
                    Description = "",
                    Itinerary = new List<string> {},
                    WhatToDo = new List<string> {},
                    Places = new List<string> {},
                    Price = 0m,
                    Inclusions = new List<string> {},
                    Exclusions = new List<string> {},
                },
                new Destination
                {
                    Name = "",
                    ImageUrls = new List<string> {},
                    Description = "",
                    Itinerary = new List<string> {},
                    WhatToDo = new List<string> {},
                    Places = new List<string> {},
                    Price = 0m,
                    Inclusions = new List<string> {},
                    Exclusions = new List<string> {},
                },
                new Destination
                {
                    Name = "",
                    ImageUrls = new List<string> {},
                    Description = "",
                    Itinerary = new List<string> {},
                    WhatToDo = new List<string> {},
                    Places = new List<string> {},
                    Price = 0m,
                    Inclusions = new List<string> {},
                    Exclusions = new List<string> {},
                },
                new Destination
                {
                    Name = "",
                    ImageUrls = new List<string> {},
                    Description = "",
                    Itinerary = new List<string> {},
                    WhatToDo = new List<string> {},
                    Places = new List<string> {},
                    Price = 0m,
                    Inclusions = new List<string> {},
                    Exclusions = new List<string> {},
                },
                new Destination
                {
                    Name = "",
                    ImageUrls = new List<string> {},
                    Description = "",
                    Itinerary = new List<string> {},
                    WhatToDo = new List<string> {},
                    Places = new List<string> {},
                    Price = 0m,
                    Inclusions = new List<string> {},
                    Exclusions = new List<string> {},
                },
                new Destination
                {
                    Name = "",
                    ImageUrls = new List<string> {},
                    Description = "",
                    Itinerary = new List<string> {},
                    WhatToDo = new List<string> {},
                    Places = new List<string> {},
                    Price = 0m,
                    Inclusions = new List<string> {},
                    Exclusions = new List<string> {},
                },
                new Destination
                {
                    Name = "",
                    ImageUrls = new List<string> {},
                    Description = "",
                    Itinerary = new List<string> {},
                    WhatToDo = new List<string> {},
                    Places = new List<string> {},
                    Price = 0m,
                    Inclusions = new List<string> {},
                    Exclusions = new List<string> {},
                },
                new Destination
                {
                    Name = "",
                    ImageUrls = new List<string> {},
                    Description = "",
                    Itinerary = new List<string> {},
                    WhatToDo = new List<string> {},
                    Places = new List<string> {},
                    Price = 0m,
                    Inclusions = new List<string> {},
                    Exclusions = new List<string> {},
                },
                new Destination
                {
                    Name = "",
                    ImageUrls = new List<string> {},
                    Description = "",
                    Itinerary = new List<string> {},
                    WhatToDo = new List<string> {},
                    Places = new List<string> {},
                    Price = 0m,
                    Inclusions = new List<string> {},
                    Exclusions = new List<string> {},
                },
                new Destination
                {
                    Name = "",
                    ImageUrls = new List<string> {},
                    Description = "",
                    Itinerary = new List<string> {},
                    WhatToDo = new List<string> {},
                    Places = new List<string> {},
                    Price = 0m,
                    Inclusions = new List<string> {},
                    Exclusions = new List<string> {},
                },
                new Destination
                {
                    Name = "",
                    ImageUrls = new List<string> {},
                    Description = "",
                    Itinerary = new List<string> {},
                    WhatToDo = new List<string> {},
                    Places = new List<string> {},
                    Price = 0m,
                    Inclusions = new List<string> {},
                    Exclusions = new List<string> {},
                },
                new Destination
                {
                    Name = "",
                    ImageUrls = new List<string> {},
                    Description = "",
                    Itinerary = new List<string> {},
                    WhatToDo = new List<string> {},
                    Places = new List<string> {},
                    Price = 0m,
                    Inclusions = new List<string> {},
                    Exclusions = new List<string> {},
                },
                new Destination
                {
                    Name = "",
                    ImageUrls = new List<string> {},
                    Description = "",
                    Itinerary = new List<string> {},
                    WhatToDo = new List<string> {},
                    Places = new List<string> {},
                    Price = 0m,
                    Inclusions = new List<string> {},
                    Exclusions = new List<string> {},
                },
                new Destination
                {
                    Name = "",
                    ImageUrls = new List<string> {},
                    Description = "",
                    Itinerary = new List<string> {},
                    WhatToDo = new List<string> {},
                    Places = new List<string> {},
                    Price = 0m,
                    Inclusions = new List<string> {},
                    Exclusions = new List<string> {},
                },
                new Destination
                {
                    Name = "",
                    ImageUrls = new List<string> {},
                    Description = "",
                    Itinerary = new List<string> {},
                    WhatToDo = new List<string> {},
                    Places = new List<string> {},
                    Price = 0m,
                    Inclusions = new List<string> {},
                    Exclusions = new List<string> {},
                },
                new Destination
                {
                    Name = "",
                    ImageUrls = new List<string> {},
                    Description = "",
                    Itinerary = new List<string> {},
                    WhatToDo = new List<string> {},
                    Places = new List<string> {},
                    Price = 0m,
                    Inclusions = new List<string> {},
                    Exclusions = new List<string> {},
                },
                new Destination
                {
                    Name = "",
                    ImageUrls = new List<string> {},
                    Description = "",
                    Itinerary = new List<string> {},
                    WhatToDo = new List<string> {},
                    Places = new List<string> {},
                    Price = 0m,
                    Inclusions = new List<string> {},
                    Exclusions = new List<string> {},
                },
                new Destination
                {
                    Name = "",
                    ImageUrls = new List<string> {},
                    Description = "",
                    Itinerary = new List<string> {},
                    WhatToDo = new List<string> {},
                    Places = new List<string> {},
                    Price = 0m,
                    Inclusions = new List<string> {},
                    Exclusions = new List<string> {},
                },
                new Destination
                {
                    Name = "",
                    ImageUrls = new List<string> {},
                    Description = "",
                    Itinerary = new List<string> {},
                    WhatToDo = new List<string> {},
                    Places = new List<string> {},
                    Price = 0m,
                    Inclusions = new List<string> {},
                    Exclusions = new List<string> {},
                },
                new Destination
                {
                    Name = "",
                    ImageUrls = new List<string> {},
                    Description = "",
                    Itinerary = new List<string> {},
                    WhatToDo = new List<string> {},
                    Places = new List<string> {},
                    Price = 0m,
                    Inclusions = new List<string> {},
                    Exclusions = new List<string> {},
                },
                new Destination
                {
                    Name = "",
                    ImageUrls = new List<string> {},
                    Description = "",
                    Itinerary = new List<string> {},
                    WhatToDo = new List<string> {},
                    Places = new List<string> {},
                    Price = 0m,
                    Inclusions = new List<string> {},
                    Exclusions = new List<string> {},
                },
                new Destination
                {
                    Name = "",
                    ImageUrls = new List<string> {},
                    Description = "",
                    Itinerary = new List<string> {},
                    WhatToDo = new List<string> {},
                    Places = new List<string> {},
                    Price = 0m,
                    Inclusions = new List<string> {},
                    Exclusions = new List<string> {},
                },
                new Destination
                {
                    Name = "",
                    ImageUrls = new List<string> {},
                    Description = "",
                    Itinerary = new List<string> {},
                    WhatToDo = new List<string> {},
                    Places = new List<string> {},
                    Price = 0m,
                    Inclusions = new List<string> {},
                    Exclusions = new List<string> {},
                },
                new Destination
                {
                    Name = "",
                    ImageUrls = new List<string> {},
                    Description = "",
                    Itinerary = new List<string> {},
                    WhatToDo = new List<string> {},
                    Places = new List<string> {},
                    Price = 0m,
                    Inclusions = new List<string> {},
                    Exclusions = new List<string> {},
                },
                new Destination
                {
                    Name = "",
                    ImageUrls = new List<string> {},
                    Description = "",
                    Itinerary = new List<string> {},
                    WhatToDo = new List<string> {},
                    Places = new List<string> {},
                    Price = 0m,
                    Inclusions = new List<string> {},
                    Exclusions = new List<string> {},
                },
                new Destination
                {
                    Name = "",
                    ImageUrls = new List<string> {},
                    Description = "",
                    Itinerary = new List<string> {},
                    WhatToDo = new List<string> {},
                    Places = new List<string> {},
                    Price = 0m,
                    Inclusions = new List<string> {},
                    Exclusions = new List<string> {},
                },
                new Destination
                {
                    Name = "",
                    ImageUrls = new List<string> {},
                    Description = "",
                    Itinerary = new List<string> {},
                    WhatToDo = new List<string> {},
                    Places = new List<string> {},
                    Price = 0m,
                    Inclusions = new List<string> {},
                    Exclusions = new List<string> {},
                },
                new Destination
                {
                    Name = "",
                    ImageUrls = new List<string> {},
                    Description = "",
                    Itinerary = new List<string> {},
                    WhatToDo = new List<string> {},
                    Places = new List<string> {},
                    Price = 0m,
                    Inclusions = new List<string> {},
                    Exclusions = new List<string> {},
                },
                new Destination
                {
                    Name = "",
                    ImageUrls = new List<string> {},
                    Description = "",
                    Itinerary = new List<string> {},
                    WhatToDo = new List<string> {},
                    Places = new List<string> {},
                    Price = 0m,
                    Inclusions = new List<string> {},
                    Exclusions = new List<string> {},
                },
                new Destination
                {
                    Name = "",
                    ImageUrls = new List<string> {},
                    Description = "",
                    Itinerary = new List<string> {},
                    WhatToDo = new List<string> {},
                    Places = new List<string> {},
                    Price = 0m,
                    Inclusions = new List<string> {},
                    Exclusions = new List<string> {},
                },
                new Destination
                {
                    Name = "",
                    ImageUrls = new List<string> {},
                    Description = "",
                    Itinerary = new List<string> {},
                    WhatToDo = new List<string> {},
                    Places = new List<string> {},
                    Price = 0m,
                    Inclusions = new List<string> {},
                    Exclusions = new List<string> {},
                },
                new Destination
                {
                    Name = "",
                    ImageUrls = new List<string> {},
                    Description = "",
                    Itinerary = new List<string> {},
                    WhatToDo = new List<string> {},
                    Places = new List<string> {},
                    Price = 0m,
                    Inclusions = new List<string> {},
                    Exclusions = new List<string> {},
                },
            };

            if (!string.IsNullOrEmpty(searchQuery))
            {
                CountryDestinations = CountryDestinations
                    .Where(d => d.Name.Contains(searchQuery, System.StringComparison.OrdinalIgnoreCase) ||
                                d.Description.Contains(searchQuery, System.StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
        }

        public class Destination
        {
            public string Name { get; set; } = string.Empty;
            public List<string> ImageUrls { get; set; } = new();
            public string Description { get; set; } = string.Empty;
            public List<string> Itinerary { get; set; } = new();
            public List<string> WhatToDo { get; set; } = new();
            public List<string> Places { get; set; } = new();
            public decimal Price { get; set; }
            public List<string> Inclusions { get; set; } = new();
            public List<string> Exclusions { get; set; } = new();
        }
    }
}
