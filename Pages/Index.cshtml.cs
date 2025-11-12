using System;
using System.Diagnostics.Metrics;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace india.ttlholidays.com.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMemoryCache _cache;

        [BindProperty]


        public List<Destination> CountryDestinations { get; set; } = new();
        public string SearchQuery { get; set; } = string.Empty;
        public IndexModel(
            ILogger<IndexModel> logger,
            IConfiguration config,
            IHttpClientFactory httpClientFactory,
            IMemoryCache cache)
        {
            _logger = logger;
            _config = config;
            _httpClientFactory = httpClientFactory;
            _cache = cache;
        }

        public JsonElement OffersRoot { get; set; }
        public JsonElement IndiaRoot { get; set; }
        public JsonElement InternationalRoot { get; set; }
        public JsonElement PackageRoot { get; set; }

        public string IMGURL { get; set; } = string.Empty;
        public string APIURL { get; set; } = string.Empty;
 
        public async Task OnGetAsync()
        {
            BindData(SearchQuery);
            IMGURL = _config["AppSettings:IMGURL"] ?? string.Empty;
            APIURL = _config["AppSettings:APIURL"] ?? string.Empty;

            // ✅ Run all API calls in parallel for faster loading
            var offersTask = GetCachedJsonAsync($"{APIURL}get_offers_listing.php", "OffersRoot");
            var indiaTask = GetCachedJsonAsync($"{APIURL}get_destination_listing.php?destination=India", "IndiaRoot");
            var internationalTask = GetCachedJsonAsync($"{APIURL}get_destination_listing.php?destination=International", "InternationalRoot");
            var packageTask = GetCachedJsonAsync($"{APIURL}get_package_listing.php?limit=10", "PackageRoot");

            await Task.WhenAll(offersTask, indiaTask, internationalTask, packageTask);

            try
            {
                OffersRoot = offersTask.Result.GetProperty("offers");
            }
            catch { }

            try
            {
                IndiaRoot = indiaTask.Result.GetProperty("citydata");
            }
            catch { }

            try
            {
                InternationalRoot = internationalTask.Result.GetProperty("citydata");
            }
            catch { }

            try
            {
                PackageRoot = packageTask.Result.GetProperty("packages");
            }
            catch { }
        }

        /// <summary>
        /// ✅ Cached + shared HttpClient + async + error-handled API fetch
        /// </summary>
        private async Task<JsonElement> GetCachedJsonAsync(string url, string cacheKey)
        {
            if (_cache.TryGetValue(cacheKey, out JsonElement cachedData))
                return cachedData;

            var client = _httpClientFactory.CreateClient();

            try
            {
                var json = await client.GetStringAsync(url);
                json = System.Text.Encoding.UTF8.GetString(System.Text.Encoding.Default.GetBytes(json));

                var root = JsonDocument.Parse(json).RootElement;

                // Cache the response for 10 minutes
                _cache.Set(cacheKey, root, TimeSpan.FromMinutes(10));

                return root;
            }
            catch (Exception ex)
            {
                _logger.LogError("API call failed for {CacheKey}: {Error}", cacheKey, ex.Message);
                return JsonDocument.Parse("{}").RootElement;
            }
        }

        private void BindData(string searchQuery)
        {
            CountryDestinations = new List<Destination>
            {
                new Destination
                {
                    Name = "Barcelona",
                    ImageUrls = new List<string> {
                        "/assets/img/destination/h3-barcelona-resumption.webp",
                        "/assets/img/destination/h3-barcelona1.webp",
                        "/assets/img/destination/h3-barcelona2.webp",
                        "/assets/img/destination/h3-barcelona3.webp",
                        "/assets/img/destination/h3-barcelona4.webp"
                    },
                    Description = "Barcelona is famous for its architecture, beaches, and vibrant nightlife. The city is home to iconic landmarks such as the Sagrada Familia, Park Güell, and Casa Batlló. Visitors can enjoy the bustling La Rambla street, relax on Barceloneta Beach, and experience the rich Catalan culture. The city also offers a variety of museums, including the Picasso Museum and the Joan Miró Foundation. Barcelona is a food lover's paradise, with a wide range of tapas bars and restaurants offering traditional Catalan cuisine.",
                    Itinerary = new List<string> {"Day 1: Arrival in Barcelona, check-in at the hotel, explore La Rambla.",
            "Day 2: Visit Sagrada Familia and Park Güell.",
            "Day 3: Relax at Barceloneta Beach.",
            "Day 4: Explore the Gothic Quarter and the Picasso Museum.",
            "Day 5: Visit the Joan Miró Foundation and enjoy local cuisine.",
            "Day 6: Day trip to Montserrat.",
            "Day 7: Shopping and leisure in the city center.",
            "Day 8: Visit the Magic Fountain of Montjuïc.",
            "Day 9: Explore the Olympic Stadium and the surrounding area.",
            "Day 10: Departure."},
                    WhatToDo = new List<string> { "Visit museums",
            "Enjoy local cuisine",
            "Take a flamenco dance class",
            "Explore the Gothic Quarter",
            "Relax on Barceloneta Beach",
            "Visit the Picasso Museum",
            "Visit the Joan Miró Foundation",
            "Day trip to Montserrat"},
                    Places = new List<string> { "Sagrada Familia",
            "Park Güell",
            "Casa Batlló",
            "La Rambla",
            "Barceloneta Beach",
            "Gothic Quarter",
            "Picasso Museum",
            "Joan Miró Foundation",
            "Montserrat"},
                    Price =  1500,
                    Inclusions = new List<string> { "Accommodation for 10 nights in 4-star hotels",
            "Daily breakfast",
            "Airport transfers",
            "Guided tours of all mentioned attractions",
            "Comfortable transportation for all intercity travel"},
                    Exclusions = new List<string> {"International & domestic airfare",
            "Personal expenses such as shopping, tips, and meals not mentioned",
            "Travel insurance",
            "Additional activities not mentioned in the itinerary",
            "Entry tickets for optional attractions"},
                },
                new Destination
                {
                    Name = "Australia",
                    ImageUrls = new List<string> {"/assets/img/destination/h2-melbourne-beach.webp",
            "/assets/img/destination/h2-melbourne1.webp",
            "/assets/img/destination/h2-melbourne2.webp",
            "/assets/img/destination/h2-melbourne3.webp",
            "/assets/img/destination/h2-melbourne4.webp"},
                    Description = "Australia is known for its diverse landscapes, from the Great Barrier Reef to the Outback. The country offers a unique blend of natural wonders, vibrant cities, and a rich cultural heritage. Visitors can explore the iconic Sydney Opera House, relax on the beaches of the Gold Coast, and experience the natural beauty of the Blue Mountains. Australia is also home to a variety of wildlife, including kangaroos, koalas, and platypuses. The country offers a wide range of outdoor activities, such as hiking, snorkeling, and surfing.",
                    Itinerary = new List<string> {"Day 1: Arrival in Sydney, check-in at the hotel, explore the Sydney Opera House.",
            "Day 2: Visit the Sydney Harbour Bridge and the Royal Botanic Garden.",
            "Day 3: Day trip to the Blue Mountains.",
            "Day 4: Travel to the Gold Coast, relax on the beaches.",
            "Day 5: Visit the Great Barrier Reef.",
            "Day 6: Explore the cultural heritage of the Outback.",
            "Day 7: Departure."},
                    WhatToDo = new List<string> { "Visit the Sydney Opera House",
            "Explore the Sydney Harbour Bridge",
            "Day trip to the Blue Mountains",
            "Relax on the Gold Coast beaches",
            "Snorkel the Great Barrier Reef",
            "Explore the Outback",
            "Visit wildlife parks"},
                    Places = new List<string> {"Sydney Opera House",
            "Sydney Harbour Bridge",
            "Blue Mountains",
            "Gold Coast",
            "Great Barrier Reef",
            "Outback"},
                    Price =1800,
                    Inclusions = new List<string> { "Accommodation for 7 nights in 4-star hotels",
            "Daily breakfast",
            "Airport transfers",
            "Guided tours of all mentioned attractions",
            "Comfortable transportation for all intercity travel"},
                    Exclusions = new List<string> {  "Accommodation for 7 nights in 4-star hotels",
            "Daily breakfast",
            "Airport transfers",
            "Guided tours of all mentioned attractions",
            "Comfortable transportation for all intercity travel"},
                },
                new Destination
                {
                    Name =  "Egypt",
                    ImageUrls = new List<string> {"/img/destination/h3-egypt-pyramids-camels.webp",
            "/img/destination/h3-egypt1.webp",
            "/img/destination/h3-egypt2.webp",
            "/img/destination/h3-egypt3.webp",
            "/img/destination/h3-egypt4.webp"},
                    Description =  "Egypt is famous for its ancient pyramids and rich history. The country offers a unique blend of ancient and modern attractions. Visitors can explore the iconic Pyramids of Giza, the Sphinx, and the Egyptian Museum in Cairo. The city of Luxor is known for its temples and tombs, including the Valley of the Kings. Egypt also offers a variety of cultural experiences, such as traditional markets and local cuisine. The country is a perfect destination for history buffs and adventure seekers.",
                    Itinerary = new List<string> {
                        "Day 1: Arrival in Cairo, check-in at the hotel, explore the Egyptian Museum.",
                        "Day 2: Visit the Pyramids of Giza and the Sphinx.",
                        "Day 3: Day trip to Luxor, visit the Valley of the Kings and Karnak Temple.",
                        "Day 4: Explore the city of Aswan, visit the Aswan High Dam and the Temple of Philae.",
                        "Day 5: Departure."
                    },
                    WhatToDo = new List<string> { "Visit the Pyramids of Giza",
            "Explore the Sphinx",
            "Visit the Egyptian Museum",
            "Day trip to Luxor",
            "Visit the Valley of the Kings",
            "Explore Karnak Temple",
            "Visit the Aswan High Dam",
            "Visit the Temple of Philae"},
                    Places = new List<string> { "Pyramids of Giza",
            "Sphinx",
            "Egyptian Museum",
            "Luxor",
            "Valley of the Kings",
            "Karnak Temple",
            "Aswan High Dam",
            "Temple of Philae"},
                    Price = 1600,
                    Inclusions = new List<string> { "Accommodation for 5 nights in 4-star hotels",
            "Daily breakfast",
            "Airport transfers",
            "Guided tours of all mentioned attractions",
            "Comfortable transportation for all intercity travel"},
                    Exclusions = new List<string> { "International & domestic airfare",
            "Personal expenses such as shopping, tips, and meals not mentioned",
            "Travel insurance",
            "Additional activities not mentioned in the itinerary",
            "Entry tickets for optional attractions"},
                },
                new Destination
                {
                    Name =    "USA",
                    ImageUrls = new List<string> {
                        "/img/destination/h3-washington.webp",
                        "/img/destination/h3-washington1.webp",
                        "/img/destination/h3-washington2.webp",
                        "/img/destination/h3-washington3.webp",
                        "/img/destination/h3-washington4.webp"
                    },
                    Description =  "The United States of America is a popular destination for tourists and students alike. The country offers a wide range of attractions, from the iconic landmarks of New York City to the natural beauty of the Grand Canyon. Visitors can explore the historic sites of Washington, D.C., enjoy the beaches of California, and experience the vibrant culture of New Orleans. The USA is also home to world-class universities and a variety of outdoor activities, such as hiking, skiing, and camping.",
                    Itinerary = new List<string> {
                        "Day 1: Arrival in New York City, check-in at the hotel, explore Times Square.",
            "Day 2: Visit the Statue of Liberty and Ellis Island.",
            "Day 3: Explore Central Park and the Metropolitan Museum of Art.",
            "Day 4: Travel to Washington, D.C., visit the National Mall and the White House.",
            "Day 5: Explore the historic sites of Philadelphia.",
            "Day 6: Travel to Boston, visit the Freedom Trail.",
            "Day 7: Explore the natural beauty of the Grand Canyon.",
            "Day 8: Visit the iconic landmarks of Las Vegas.",
            "Day 9: Relax on the beaches of California.",
            "Day 10: Explore the cultural heritage of New Orleans.",
            "Day 11: Visit the historic sites of Charleston.",
            "Day 12: Travel to Miami, enjoy the beaches.",
            "Day 13: Explore the Everglades National Park.",
            "Day 14: Visit the Kennedy Space Center.",
            "Day 15: Explore the cultural heritage of Nashville.",
            "Day 16: Visit the historic sites of Memphis.",
            "Day 17: Explore the natural beauty of Yellowstone National Park.",
            "Day 18: Departure."},
                    WhatToDo = new List<string> { "Visit the Statue of Liberty",
            "Explore Times Square",
            "Visit the National Mall",
            "Explore the White House",
            "Visit the Freedom Trail",
            "Explore the Grand Canyon",
            "Visit Las Vegas",
            "Relax on California beaches",
            "Explore New Orleans",
            "Visit Charleston",
            "Enjoy Miami beaches",
            "Explore the Everglades National Park",
            "Visit the Kennedy Space Center",
            "Explore Nashville",
            "Visit Memphis",
            "Explore Yellowstone National Park"},
                    Places = new List<string> {"New York City",
            "Washington, D.C.",
            "Philadelphia",
            "Boston",
            "Grand Canyon",
            "Las Vegas",
            "California",
            "New Orleans",
            "Charleston",
            "Miami",
            "Everglades National Park",
            "Kennedy Space Center",
            "Nashville",
            "Memphis",
            "Yellowstone National Park"},
                    Price = 1900,
                    Inclusions = new List<string> { "Accommodation for 18 nights in 4-star hotels",
            "Daily breakfast",
            "Airport transfers",
            "Guided tours of all mentioned attractions",
            "Comfortable transportation for all intercity travel"},
                    Exclusions = new List<string> {"International & domestic airfare",
            "Personal expenses such as shopping, tips, and meals not mentioned",
            "Travel insurance",
            "Additional activities not mentioned in the itinerary",
            "Entry tickets for optional attractions"},
                },
                new Destination
                {
                    Name =  "Denmark",
                    ImageUrls = new List<string> { "/assets/img/destination/AMS.webp",
            "/assets/img/destination/AMS1.webp",
            "/assets/img/destination/AMS2.webp",
            "/assets/img/destination/AMS3.webp",
            "/assets/img/destination/AMS4.webp"},
                    Description = "Denmark is known for its design, architecture, and quality of life. The country offers a unique blend of modern and historic attractions. Visitors can explore the iconic Little Mermaid statue in Copenhagen, visit the historic castles of Kronborg and Frederiksborg, and enjoy the vibrant culture of the capital city. Denmark is also home to a variety of outdoor activities, such as cycling, hiking, and sailing. The country is a perfect destination for those interested in design, architecture, and quality of life.",
                    Itinerary = new List<string> {  "Day 1: Arrival in Copenhagen, check-in at the hotel, explore the Little Mermaid statue.",
            "Day 2: Visit the historic castles of Kronborg and Frederiksborg.",
            "Day 3: Explore the Nyhavn harbor and the surrounding area.",
            "Day 4: Visit the National Museum of Denmark.",
            "Day 5: Explore the Tivoli Gardens.",
            "Day 6: Travel to Aarhus, visit the Old Town.",
            "Day 7: Explore the Aarhus Cathedral and the surrounding area.",
            "Day 8: Visit the Louisiana Museum of Modern Art.",
            "Day 9: Explore the cultural heritage of Odense.",
            "Day 10: Departure."},
                    WhatToDo = new List<string> { "Visit the Little Mermaid statue",
            "Explore the Nyhavn harbor",
            "Visit the National Museum of Denmark",
            "Explore the Tivoli Gardens",
            "Visit the Old Town in Aarhus",
            "Explore the Aarhus Cathedral",
            "Visit the Louisiana Museum of Modern Art",
            "Explore Odense"},
                    Places = new List<string> {"Little Mermaid statue",
            "Kronborg Castle",
            "Frederiksborg Castle",
            "Nyhavn harbor",
            "National Museum of Denmark",
            "Tivoli Gardens",
            "Aarhus Old Town",
            "Aarhus Cathedral",
            "Louisiana Museum of Modern Art",
            "Odense"},
                    Price =  1700,
                    Inclusions = new List<string> {"Accommodation for 10 nights in 4-star hotels",
            "Daily breakfast",

            "Airport transfers",
            "Guided tours of all mentioned attractions",
            "Comfortable transportation for all intercity travel"},
                    Exclusions = new List<string> {"International & domestic airfare",
            "Personal expenses such as shopping, tips, and meals not mentioned",
            "Travel insurance",
            "Additional activities not mentioned in the itinerary",
            "Entry tickets for optional attractions"},
                },
                new Destination
                {
                    Name = "Abu Dhabi",
                    ImageUrls = new List<string> {"/assets/img/destination/h3-abu-dhabi-grand-mosque.webp",
            "/assets/img/destination/h3-abu-dhabi1.webp",
            "/assets/img/destination/h3-abu-dhabi2.webp",
            "/assets/img/destination/h3-abu-dhabi3.webp",
            "/assets/img/destination/h3-abu-dhabi4.webp"},
                    Description = "Abu Dhabi is known for its modern architecture and cultural heritage. The city offers a unique blend of modern skyscrapers and traditional cultural sites. Visitors can explore the iconic Sheikh Zayed Grand Mosque, visit the Louvre Abu Dhabi, and enjoy the natural beauty of the Saadiyat Beach. The city also offers a variety of outdoor activities, such as desert safaris and water sports. Abu Dhabi is a perfect destination for those interested in modern architecture and cultural heritage.",


                    Itinerary = new List<string> {"Day 1: Arrival in Abu Dhabi, check-in at the hotel, explore the Sheikh Zayed Grand Mosque.",
            "Day 2: Visit the Louvre Abu Dhabi and the Guggenheim Museum.",
            "Day 3: Relax on Saadiyat Beach.",
            "Day 4: Explore the cultural heritage of Al Ain.",
            "Day 5: Visit the Liwa Oasis.",
            "Day 6: Enjoy a desert safari.",
            "Day 7: Explore the city of Dubai, visit the Burj Khalifa.",
            "Day 8: Departure."},
                    WhatToDo = new List<string> { "Visit the Sheikh Zayed Grand Mosque",
            "Explore the Louvre Abu Dhabi",
            "Visit the Guggenheim Museum",
            "Relax on Saadiyat Beach",
            "Explore Al Ain",
            "Visit the Liwa Oasis",
            "Enjoy a desert safari",
            "Visit the Burj Khalifa in Dubai"
        },
                    Places = new List<string> { "Sheikh Zayed Grand Mosque",
            "Louvre Abu Dhabi",
            "Guggenheim Museum",
            "Saadiyat Beach",
            "Al Ain",
            "Liwa Oasis",
            "Burj Khalifa"},
                    Price = 1400,
                    Inclusions = new List<string> { "Accommodation for 8 nights in 4-star hotels",
            "Daily breakfast",
            "Airport transfers",
            "Guided tours of all mentioned attractions",
            "Comfortable transportation for all intercity travel"},
                    Exclusions = new List<string> { "International & domestic airfare",
            "Personal expenses such as shopping, tips, and meals not mentioned",
            "Travel insurance",
            "Additional activities not mentioned in the itinerary",
            "Entry tickets for optional attractions"},
                },
                new Destination
                {
                    Name = "China",
                    ImageUrls = new List<string> { "/assets/img/destination/h3-beijing-city.webp",
            "/assets/img/destination/h3-beijing1.webp",
            "/assets/img/destination/h3-beijing2.webp",
            "/assets/img/destination/h3-beijing3.webp",
            "/assets/img/destination/h3-beijing4.webp"},
                    Description = "China is a vast country with a rich history and diverse landscapes. The country offers a unique blend of ancient and modern attractions. Visitors can explore the iconic Great Wall of China, visit the Forbidden City in Beijing, and enjoy the natural beauty of the Li River in Guilin. China is also home to a variety of cultural experiences, such as traditional markets and local cuisine. The country is a perfect destination for history buffs and adventure seekers.",
                    Itinerary = new List<string> { "Day 1: Arrival in Beijing, check-in at the hotel, explore the Forbidden City.",
            "Day 2: Visit the Great Wall of China.",
            "Day 3: Explore the Summer Palace and the surrounding area.",
            "Day 4: Visit the Temple of Heaven.",
            "Day 5: Travel to Xi'an, visit the Terracotta Army.",
            "Day 6: Explore the ancient city walls of Xi'an.",
            "Day 7: Travel to Guilin, enjoy the natural beauty of the Li River.",
            "Day 8: Visit the Longsheng Rice Terraces.",
            "Day 9: Travel to Shanghai, visit the Bund and the surrounding area.",
            "Day 10: Explore the Shanghai Museum.",
            "Day 11: Visit the Yu Garden.",
            "Day 12: Travel to Hangzhou, visit the West Lake.",
            "Day 13: Explore the Lingyin Temple.",
            "Day 14: Travel to Chengdu, visit the Giant Panda Breeding Research Base.",
            "Day 15: Explore the Wuhou Shrine.",
            "Day 16: Travel to Chongqing, visit the Three Gorges.",
            "Day 17: Explore the Dazu Rock Carvings.",
            "Day 18: Travel to Huangshan, visit the Huangshan Mountain.",
            "Day 19: Explore the ancient villages of Yixian.",
            "Day 20: Travel to Suzhou, visit the Humble Administrator's Garden.",
            "Day 21: Explore the Tiger Hill Pagoda.",
            "Day 22: Departure."},
                    WhatToDo = new List<string> {
                        "Visit the Forbidden City",
            "Explore the Great Wall of China",
            "Visit the Summer Palace",
            "Explore the Temple of Heaven",
            "Visit the Terracotta Army",
            "Explore the ancient city walls of Xi'an",
            "Enjoy the Li River in Guilin",
            "Visit the Longsheng Rice Terraces",
            "Explore the Bund in Shanghai",
            "Visit the Shanghai Museum",
            "Explore the Yu Garden",
            "Visit the West Lake in Hangzhou",
            "Explore the Lingyin Temple",
            "Visit the Giant Panda Breeding Research Base",
            "Explore the Wuhou Shrine",
            "Visit the Three Gorges",
            "Explore the Dazu Rock Carvings",
            "Visit the Huangshan Mountain",
            "Explore the ancient villages of Yixian",
            "Visit the Humble Administrator's Garden",
            "Explore the Tiger Hill Pagoda"},
                    Places = new List<string> {"Forbidden City",
            "Great Wall of China",
            "Summer Palace",
            "Temple of Heaven",
            "Terracotta Army",
            "Ancient city walls of Xi'an",
            "Li River",
            "Longsheng Rice Terraces",
            "Bund",
            "Shanghai Museum",
            "Yu Garden",
            "West Lake",
            "Lingyin Temple",
            "Giant Panda Breeding Research Base",
            "Wuhou Shrine",
            "Three Gorges",
            "Dazu Rock Carvings",
            "Huangshan Mountain",
            "Ancient villages of Yixian",
            "Humble Administrator's Garden",
            "Tiger Hill Pagoda"},
                    Price = 1900,
                    Inclusions = new List<string> {"Accommodation for 22 nights in 4-star hotels",
            "Daily breakfast",
            "Airport transfers",
            "Guided tours of all mentioned attractions",
            "Comfortable transportation for all intercity travel"},
                    Exclusions = new List<string> { "International & domestic airfare",
            "Personal expenses such as shopping, tips, and meals not mentioned",
            "Travel insurance",
            "Additional activities not mentioned in the itinerary",
            "Entry tickets for optional attractions"},
                },
                new Destination
                {
                    Name ="Greece",
                    ImageUrls = new List<string> {"/assets/img/destination/h3-santorini-greece.webp",
            "/assets/img/destination/h3-santorini1.webp",
            "/assets/img/destination/h3-santorini2.webp",
            "/assets/img/destination/h3-santorini3.webp",
            "/assets/img/destination/h3-santorini4.webp"},
                    Description =  "Greece is known for its beautiful islands, ancient ruins, and Mediterranean cuisine. The country offers a unique blend of natural beauty and historical significance. Visitors can explore the iconic Acropolis in Athens, visit the ancient city of Delphi, and enjoy the natural beauty of the islands of Santorini and Mykonos. Greece is also home to a variety of cultural experiences, such as traditional markets and local cuisine. The country is a perfect destination for those interested in history and natural beauty.",
                    Itinerary = new List<string> { "Day 1: Arrival in Athens, check-in at the hotel, explore the Acropolis.",
            "Day 2: Visit the ancient city of Delphi.",
            "Day 3: Travel to Santorini, enjoy the natural beauty of the island.",
            "Day 4: Explore the ancient ruins of Akrotiri.",
            "Day 5: Travel to Mykonos, visit the Windmills and the Little Venice.",
            "Day 6: Departure."},
                    WhatToDo = new List<string> {"Visit the Acropolis",
            "Explore the ancient city of Delphi",
            "Enjoy the natural beauty of Santorini",
            "Explore the ancient ruins of Akrotiri",
            "Visit the Windmills in Mykonos",
            "Explore Little Venice"},
                    Places = new List<string> {"Acropolis",
            "Ancient city of Delphi",
            "Santorini",
            "Akrotiri",
            "Mykonos",
            "Windmills",
            "Little Venice"},
                    Price =1600,
                    Inclusions = new List<string> { "Accommodation for 6 nights in 4-star hotels",
            "Daily breakfast",
            "Airport transfers",
            "Guided tours of all mentioned attractions",
            "Comfortable transportation for all intercity travel"},
                    Exclusions = new List<string> { "International & domestic airfare",
            "Personal expenses such as shopping, tips, and meals not mentioned",
            "Travel insurance",
            "Additional activities not mentioned in the itinerary",
            "Entry tickets for optional attractions"
        },
                },
               new Destination
{
    Name = "Himalayas",
    ImageUrls = new List<string>
    {
        "/assets/img/destination/Himalaya.webp",
        "/assets/img/destination/Himalaya1.webp",
        "/assets/img/destination/Himalaya2.webp",
        "/assets/img/destination/Himalaya3.webp",
        "/assets/img/destination/Himalaya4.webp"
    },
    Description = "The Himalayas, the highest mountain range in the world, offer breathtaking natural beauty and a unique cultural experience. Visitors can explore the serene monasteries, trek through lush valleys, and witness the stunning snow-capped peaks. The region is also home to diverse flora and fauna, making it a paradise for nature lovers.",
    Itinerary = new List<string>
    {
        "Day 1: Arrival in Kathmandu. Check-in at the hotel. Explore the city's vibrant markets and historical sites. Enjoy a traditional Nepalese dinner.",
        "Day 2: Visit the ancient Pashupatinath Temple and Boudhanath Stupa. Explore these UNESCO World Heritage Sites and learn about their cultural significance.",
        "Day 3: Travel to Pokhara. Visit the Davis Falls and Seti River. Enjoy a boat ride on Phewa Lake and explore the local markets.",
        "Day 4: Explore the International Mountain Museum. Learn about the history and culture of the Himalayas. Visit the World Peace Pagoda.",
        "Day 5: Trek to Sarangkot for panoramic views of the Himalayas. Enjoy a sunrise view over the mountains. Return to Pokhara in the evening.",
        "Day 6: Visit the Annapurna Conservation Area. Explore the area's rich biodiversity and natural beauty. Enjoy a guided nature walk.",
        "Day 7: Day trip to Chitwan National Park. Enjoy a jungle safari and explore the park's diverse flora and fauna. Return to Pokhara in the evening.",
        "Day 8: Explore the cultural heritage of Lumbini. Visit the birthplace of Lord Buddha and explore the monasteries and temples.",
        "Day 9: Travel to Nagarkot. Visit the ancient temples and enjoy panoramic views of the Himalayas. Spend the evening at leisure.",
        "Day 10: Departure. Check-out from the hotel and depart from Kathmandu."
    },
    WhatToDo = new List<string>
    {
        "Visit ancient temples",
        "Explore cultural heritage sites",
        "Trek to Sarangkot",
        "Visit Chitwan National Park",
        "Explore Lumbini",
        "Visit Nagarkot"
    },
    Places = new List<string>
    {
        "Kathmandu",
        "Pashupatinath Temple",
        "Boudhanath Stupa",
        "Pokhara",
        "Davis Falls",
        "Seti River",
        "International Mountain Museum",
        "Sarangkot",
        "Annapurna Conservation Area",
        "Chitwan National Park",
        "Lumbini",
        "Nagarkot"
    },
    Price = 2300,
    Inclusions = new List<string>
    {
        "Accommodation for 10 nights in 3-star hotels",
        "Daily breakfast",
        "Airport transfers",
        "Guided tours of all mentioned attractions",
        "Comfortable transportation for all intercity travel"
    },
    Exclusions = new List<string>
    {
        "International & domestic airfare",
        "Personal expenses such as shopping, tips, and meals not mentioned",
        "Travel insurance",
        "Additional activities not mentioned in the itinerary",
        "Entry tickets for optional attractions"
    }
},

              new Destination
{
    Name = "Maharashtra",
    ImageUrls = new List<string>
    {
        "/assets/img/destination/Maharashtra.webp",
        "/assets/img/destination/Maharashtra1.webp",
        "/assets/img/destination/Maharashtra2.webp",
        "/assets/img/destination/Maharashtra3.webp",
        "/assets/img/destination/Maharashtra4.webp"
    },
    Description = "Maharashtra, the land of the Marathas, is known for its rich history, vibrant culture, and bustling cities. From the bustling streets of Mumbai to the serene beaches of Goa, Maharashtra offers a diverse range of experiences. Visitors can explore the historic forts, ancient caves, and beautiful temples.",
    Itinerary = new List<string>
    {
        "Day 1: Arrival in Mumbai. Check-in at your hotel. Explore the Gateway of India and the Marine Drive. Enjoy a traditional Maharashtrian dinner.",
        "Day 2: Visit the Elephanta Caves. Take a ferry to the caves and explore the ancient rock-cut temples. Spend the afternoon at the Chhatrapati Shivaji Maharaj Vastu Sangrahalaya.",
        "Day 3: Travel to Pune. Visit the Shaniwar Wada and the Aga Khan Palace. Explore the historical significance of these sites.",
        "Day 4: Visit the Sinhagad Fort. Enjoy a trek to the fort and explore its rich history. Return to Pune in the evening.",
        "Day 5: Travel to Aurangabad. Visit the Bibi Ka Maqbara. Explore the architecture of this beautiful mausoleum.",
        "Day 6: Visit the Ajanta and Ellora Caves. Explore these UNESCO World Heritage Sites and marvel at the ancient rock-cut architecture.",
        "Day 7: Travel to Kolhapur. Visit the Mahalakshmi Temple. Explore the temple's rich history and architecture.",
        "Day 8: Visit the Radhanagari Dam. Enjoy a boat ride and explore the surrounding natural beauty. Return to Kolhapur in the evening.",
        "Day 9: Travel to Mumbai. Spend the day exploring the city's markets and enjoying local street food.",
        "Day 10: Departure. Check-out from the hotel and depart from Mumbai."
    },
    WhatToDo = new List<string>
    {
        "Explore the Gateway of India",
        "Visit the Elephanta Caves",
        "Visit Shaniwar Wada",
        "Explore Sinhagad Fort",
        "Visit Bibi Ka Maqbara",
        "Explore Ajanta and Ellora Caves",
        "Visit Mahalakshmi Temple",
        "Visit Radhanagari Dam"
    },
    Places = new List<string>
    {
        "Mumbai",
        "Gateway of India",
        "Elephanta Caves",
        "Pune",
        "Shaniwar Wada",
        "Aga Khan Palace",
        "Sinhagad Fort",
        "Aurangabad",
        "Bibi Ka Maqbara",
        "Ajanta Caves",
        "Ellora Caves",
        "Kolhapur",
        "Mahalakshmi Temple",
        "Radhanagari Dam"
    },
    Price = 1800,
    Inclusions = new List<string>
    {
        "Accommodation for 10 nights in 3-star hotels",
        "Daily breakfast",
        "Airport transfers",
        "Guided tours of all mentioned attractions",
        "Comfortable transportation for all intercity travel"
    },
    Exclusions = new List<string>
    {
        "International & domestic airfare",
        "Personal expenses such as shopping, tips, and meals not mentioned",
        "Travel insurance",
        "Additional activities not mentioned in the itinerary",
        "Entry tickets for optional attractions"
    }
},

              new Destination
{
    Name = "Sikkim",
    ImageUrls = new List<string>
    {
        "/assets/img/destination/Sikkim.webp",
        "/assets/img/destination/Sikkim1.webp",
        "/assets/img/destination/Sikkim2.webp",
        "/assets/img/destination/Sikkim3.webp",
        "/assets/img/destination/Sikkim4.webp"
    },
    Description = "Sikkim, nestled in the Himalayas, is known for its breathtaking natural beauty, rich cultural heritage, and serene monasteries. Visitors can explore the Tsomgo Lake, Nathu La Pass, and the Rumtek Monastery. Sikkim offers a perfect blend of adventure and tranquility.",
    Itinerary = new List<string>
    {
        "Day 1: Arrival in Gangtok. Check-in at your hotel. Explore the Rumtek Monastery and the surrounding area. Enjoy a traditional Sikkimese dinner.",
        "Day 2: Visit the Tsomgo Lake and Nathu La Pass. Enjoy the scenic drive and marvel at the breathtaking views. Return to Gangtok in the evening.",
        "Day 3: Explore the Bakthang Waterfall and the Ban Jhakri Falls. Enjoy a leisurely day in the natural surroundings.",
        "Day 4: Visit the Do-Drul Chorten Stupa. Explore the monasteries and learn about the region's rich cultural heritage.",
        "Day 5: Travel to Pelling. Visit the Pemayangtse Monastery. Explore the monastery's rich history and architecture.",
        "Day 6: Visit the Khecheopalri Lake. Enjoy a peaceful walk around the lake and explore the surrounding natural beauty.",
        "Day 7: Travel to Darjeeling. Visit the Tiger Hill and enjoy the sunrise over the Himalayas. Explore the tea gardens.",
        "Day 8: Visit the Batasia Loop and the Himalayan Mountaineering Institute. Enjoy a leisurely day in Darjeeling.",
        "Day 9: Travel to Kalimpong. Visit the Tharpa Choling Monastery. Explore the monastery and the surrounding area.",
        "Day 10: Departure. Check-out from the hotel and depart from Kalimpong."
    },
    WhatToDo = new List<string>
    {
        "Visit Rumtek Monastery",
        "Explore Tsomgo Lake",
        "Visit Nathu La Pass",
        "Explore Bakthang Waterfall",
        "Visit Ban Jhakri Falls",
        "Visit Do-Drul Chorten Stupa",
        "Visit Pemayangtse Monastery",
        "Explore Khecheopalri Lake",
        "Visit Tiger Hill",
        "Explore Tea Gardens",
        "Visit Batasia Loop",
        "Visit Tharpa Choling Monastery"
    },
    Places = new List<string>
    {
        "Gangtok",
        "Rumtek Monastery",
        "Tsomgo Lake",
        "Nathu La Pass",
        "Bakthang Waterfall",
        "Ban Jhakri Falls",
        "Do-Drul Chorten Stupa",
        "Pelling",
        "Pemayangtse Monastery",
        "Khecheopalri Lake",
        "Darjeeling",
        "Tiger Hill",
        "Batasia Loop",
        "Himalayan Mountaineering Institute",
        "Kalimpong",
        "Tharpa Choling Monastery"
    },
    Price = 2000,
    Inclusions = new List<string>
    {
        "Accommodation for 10 nights in 3-star hotels",
        "Daily breakfast",
        "Airport transfers",
        "Guided tours of all mentioned attractions",
        "Comfortable transportation for all intercity travel"
    },
    Exclusions = new List<string>
    {
        "International & domestic airfare",
        "Personal expenses such as shopping, tips, and meals not mentioned",
        "Travel insurance",
        "Additional activities not mentioned in the itinerary",
        "Entry tickets for optional attractions"
    }
},

               new Destination
{
    Name = "Rishikesh",
    ImageUrls = new List<string>
    {
        "/assets/img/destination/Rishikesh.webp",
        "/assets/img/destination/Rishikesh1.webp",
        "/assets/img/destination/Rishikesh2.webp",
        "/assets/img/destination/Rishikesh3.webp",
        "/assets/img/destination/Rishikesh4.webp"
    },
    Description = "Rishikesh, known as the 'Yoga Capital of the World', is located in the foothills of the Himalayas in Uttarakhand. It is a spiritual hub offering a blend of adventure and tranquility. Visitors can explore the Triveni Ghat, Neelkanth Mahadev Temple, and the Lakshman Jhula.",
    Itinerary = new List<string>
    {
        "Day 1: Arrival in Rishikesh. Check-in at your hotel. Visit the Triveni Ghat and participate in the evening aarti. Enjoy a traditional Indian dinner.",
        "Day 2: Visit the Neelkanth Mahadev Temple. Enjoy the scenic drive and explore the temple's rich history. Return to Rishikesh in the evening.",
        "Day 3: Explore the Lakshman Jhula. Take a walk across the suspension bridge and enjoy the views of the Ganges. Visit the nearby cafes and shops.",
        "Day 4: Visit the Ram Jhula. Explore the Swami Sivananda Ashram and the surrounding area. Enjoy a peaceful evening by the Ganges.",
        "Day 5: Participate in a yoga and meditation session. Spend the day learning about the spiritual practices of Rishikesh.",
        "Day 6: Visit the Beatles Ashram. Explore the historic site and learn about its significance. Return to Rishikesh in the evening.",
        "Day 7: Enjoy a white-water rafting adventure. Spend the day on the Ganges and experience the thrill of rafting.",
        "Day 8: Visit the Gita Bhavan Temple. Explore the temple and its beautiful architecture. Spend the evening at leisure.",
        "Day 9: Explore the local markets and enjoy shopping for traditional Indian handicrafts. Visit the Patanjali Yogpeeth.",
        "Day 10: Departure. Check-out from the hotel and depart from Rishikesh."
    },
    WhatToDo = new List<string>
    {
        "Visit Triveni Ghat",
        "Visit Neelkanth Mahadev Temple",
        "Explore Lakshman Jhula",
        "Visit Ram Jhula",
        "Participate in Yoga and Meditation",
        "Visit Beatles Ashram",
        "Enjoy White-Water Rafting",
        "Visit Gita Bhavan Temple",
        "Explore Local Markets",
        "Visit Patanjali Yogpeeth"
    },
    Places = new List<string>
    {
        "Rishikesh",
        "Triveni Ghat",
        "Neelkanth Mahadev Temple",
        "Lakshman Jhula",
        "Ram Jhula",
        "Swami Sivananda Ashram",
        "Beatles Ashram",
        "Gita Bhavan Temple",
        "Patanjali Yogpeeth"
    },
    Price = 1600,
    Inclusions = new List<string>
    {
        "Accommodation for 10 nights in 3-star hotels",
        "Daily breakfast",
        "Airport transfers",
        "Guided tours of all mentioned attractions",
        "Comfortable transportation for all intercity travel"
    },
    Exclusions = new List<string>
    {
        "International & domestic airfare",
        "Personal expenses such as shopping, tips, and meals not mentioned",
        "Travel insurance",
        "Additional activities not mentioned in the itinerary",
        "Entry tickets for optional attractions"
    }
},

               new Destination
{
    Name = "Gujarat",
    ImageUrls = new List<string>
    {
        "/assets/img/destination/Gujarat.webp",
        "/assets/img/destination/Gujarat1.webp",
        "/assets/img/destination/Gujarat2.webp",
        "/assets/img/destination/Gujarat3.webp",
        "/assets/img/destination/Gujarat4.webp"
    },
    Description = "Gujarat, located on the western coast of India, is known for its rich cultural heritage, historical sites, and vibrant festivals. Visitors can explore the Rann of Kutch, the Somnath Temple, and the Sabarmati Ashram. Gujarat offers a blend of history, culture, and natural beauty.",
    Itinerary = new List<string>
    {
        "Day 1: Arrival in Ahmedabad. Check-in at your hotel. Visit the Sabarmati Ashram and learn about Mahatma Gandhi's life and work. Enjoy a traditional Gujarati dinner.",
        "Day 2: Visit the Adalaj Stepwell. Explore the intricate architecture of this ancient stepwell. Spend the afternoon at the Calico Museum of Textiles.",
        "Day 3: Travel to Vadodara. Visit the Laxmi Vilas Palace. Explore the palace's rich history and architecture. Return to Ahmedabad in the evening.",
        "Day 4: Travel to Rajkot. Visit the Watson Museum. Explore the museum's collection of historical artifacts. Return to Ahmedabad in the evening.",
        "Day 5: Travel to Bhuj. Visit the Kutch Museum. Learn about the region's rich cultural heritage. Return to Ahmedabad in the evening.",
        "Day 6: Travel to Somnath. Visit the Somnath Temple. Explore the temple's rich history and architecture. Return to Ahmedabad in the evening.",
        "Day 7: Travel to Dwarka. Visit the Dwarkadhish Temple. Explore the temple's significance in Hindu mythology. Return to Ahmedabad in the evening.",
        "Day 8: Travel to Porbandar. Visit the birthplace of Mahatma Gandhi. Explore the museum and learn about his early life. Return to Ahmedabad in the evening.",
        "Day 9: Travel to Patan. Visit the Rani Ki Vav. Explore this UNESCO World Heritage Site and marvel at its architecture. Return to Ahmedabad in the evening.",
        "Day 10: Departure. Check-out from the hotel and depart from Ahmedabad."
    },
    WhatToDo = new List<string>
    {
        "Visit Sabarmati Ashram",
        "Explore Adalaj Stepwell",
        "Visit Laxmi Vilas Palace",
        "Visit Watson Museum",
        "Visit Kutch Museum",
        "Visit Somnath Temple",
        "Visit Dwarkadhish Temple",
        "Visit Mahatma Gandhi's Birthplace",
        "Visit Rani Ki Vav"
    },
    Places = new List<string>
    {
        "Ahmedabad",
        "Sabarmati Ashram",
        "Adalaj Stepwell",
        "Calico Museum of Textiles",
        "Vadodara",
        "Laxmi Vilas Palace",
        "Rajkot",
        "Watson Museum",
        "Bhuj",
        "Kutch Museum",
        "Somnath",
        "Somnath Temple",
        "Dwarka",
        "Dwarkadhish Temple",
        "Porbandar",
        "Mahatma Gandhi's Birthplace",
        "Patan",
        "Rani Ki Vav"
    },
    Price = 1900,
    Inclusions = new List<string>
    {
        "Accommodation for 10 nights in 3-star hotels",
        "Daily breakfast",
        "Airport transfers",
        "Guided tours of all mentioned attractions",
        "Comfortable transportation for all intercity travel"
    },
    Exclusions = new List<string>
    {
        "International & domestic airfare",
        "Personal expenses such as shopping, tips, and meals not mentioned",
        "Travel insurance",
        "Additional activities not mentioned in the itinerary",
        "Entry tickets for optional attractions"
    }
},
new Destination
{
    Name = "Delhi",
    ImageUrls = new List<string>
    {
        "/assets/img/destination/Delhi.webp",
        "/assets/img/destination/Delhi1.webp",
        "/assets/img/destination/Delhi2.webp",
        "/assets/img/destination/Delhi3.webp",
        "/assets/img/destination/Delhi4.webp"
    },
    Description = "Delhi, the capital of India, is a city of contrasts, blending ancient history with modern development. Visitors can explore the Red Fort, India Gate, and the Qutub Minar. Delhi offers a mix of historical sites, cultural experiences, and vibrant markets.",
    Itinerary = new List<string>
    {
        "Day 1: Arrival in Delhi. Check-in at your hotel. Visit the Red Fort and the surrounding area. Explore the historical significance of this UNESCO World Heritage Site.",
        "Day 2: Visit the India Gate. Explore the memorial and the surrounding gardens. Spend the afternoon at the National Museum.",
        "Day 3: Travel to Old Delhi. Visit the Jama Masjid and explore the narrow lanes of Chandni Chowk. Enjoy a rickshaw ride through the market.",
        "Day 4: Visit the Qutub Minar. Explore the ancient tower and the surrounding historical sites. Return to Delhi in the evening.",
        "Day 5: Visit the Lotus Temple. Explore the unique architecture of this Bahai temple. Spend the afternoon at the Humayun's Tomb.",
        "Day 6: Visit the Akshardham Temple. Explore the temple's architecture and cultural exhibits. Return to Delhi in the evening.",
        "Day 7: Visit the Rashtrapati Bhavan. Explore the presidential residence and its rich history. Return to Delhi in the evening.",
        "Day 8: Visit the Lodi Gardens. Enjoy a peaceful walk through the gardens and explore the ancient tombs. Return to Delhi in the evening.",
        "Day 9: Explore the local markets and enjoy shopping for traditional Indian handicrafts. Visit the Connaught Place market.",
        "Day 10: Departure. Check-out from the hotel and depart from Delhi."
    },
    WhatToDo = new List<string>
    {
        "Visit Red Fort",
        "Explore India Gate",
        "Visit Jama Masjid",
        "Explore Chandni Chowk",
        "Visit Qutub Minar",
        "Visit Lotus Temple",
        "Visit Humayun's Tomb",
        "Visit Akshardham Temple",
        "Visit Rashtrapati Bhavan",
        "Explore Lodi Gardens",
        "Visit Connaught Place"
    },
    Places = new List<string>
    {
        "Delhi",
        "Red Fort",
        "India Gate",
        "Jama Masjid",
        "Chandni Chowk",
        "Qutub Minar",
        "Lotus Temple",
        "Humayun's Tomb",
        "Akshardham Temple",
        "Rashtrapati Bhavan",
        "Lodi Gardens",
        "Connaught Place"
    },
    Price = 1400,
    Inclusions = new List<string>
    {
        "Accommodation for 10 nights in 3-star hotels",
        "Daily breakfast",
        "Airport transfers",
        "Guided tours of all mentioned attractions",
        "Comfortable transportation for all intercity travel"
    },
    Exclusions = new List<string>
    {
        "International & domestic airfare",
        "Personal expenses such as shopping, tips, and meals not mentioned",
        "Travel insurance",
        "Additional activities not mentioned in the itinerary",
        "Entry tickets for optional attractions"
    }
},

               new Destination
{
    Name = "Kashmir",
    ImageUrls = new List<string>
    {
        "/assets/img/destination/Kashmir.webp",
        "/assets/img/destination/Kashmir1.webp",
        "/assets/img/destination/Kashmir2.webp",
        "/assets/img/destination/Kashmir3.webp",
        "/assets/img/destination/Kashmir4.webp"
    },
    Description = "Kashmir, known as 'Paradise on Earth', is a region of breathtaking natural beauty and rich cultural heritage. Visitors can explore the Dal Lake, Pahalgam, and the Mughal Gardens. Kashmir offers a blend of adventure, tranquility, and cultural experiences.",
    Itinerary = new List<string>
    {
        "Day 1: Arrival in Srinagar. Check-in at your hotel. Visit the Dal Lake and enjoy a shikara ride. Explore the floating markets.",
        "Day 2: Visit the Mughal Gardens. Explore the Nishat Bagh and Shalimar Bagh. Enjoy the scenic views and rich history of these gardens.",
        "Day 3: Travel to Pahalgam. Visit the Betaab Valley. Enjoy the scenic drive and explore the natural beauty of the valley.",
        "Day 4: Visit the Aru Valley. Enjoy a trek to the valley and explore its serene surroundings. Return to Pahalgam in the evening.",
        "Day 5: Travel to Gulmarg. Visit the Gulmarg Ski Resort. Enjoy the scenic drive and explore the resort's facilities.",
        "Day 6: Visit the Kongdoori Lake. Enjoy a peaceful walk around the lake and explore the surrounding natural beauty.",
        "Day 7: Travel to Sonmarg. Visit the Thajiwas Glacier. Enjoy a trek to the glacier and explore its serene surroundings.",
        "Day 8: Visit the Amarnath Cave. Participate in the pilgrimage and explore the cave's significance. Return to Srinagar in the evening.",
        "Day 9: Visit the Hazratbal Shrine. Explore the shrine and its significance in Kashmiri culture. Return to Srinagar in the evening.",
        "Day 10: Departure. Check-out from the hotel and depart from Srinagar."
    },
    WhatToDo = new List<string>
    {
        "Visit Dal Lake",
        "Explore Mughal Gardens",
        "Visit Pahalgam",
        "Explore Betaab Valley",
        "Visit Aru Valley",
        "Visit Gulmarg Ski Resort",
        "Visit Kongdoori Lake",
        "Visit Thajiwas Glacier",
        "Visit Amarnath Cave",
        "Visit Hazratbal Shrine"
    },
    Places = new List<string>
    {
        "Srinagar",
        "Dal Lake",
        "Mughal Gardens",
        "Pahalgam",
        "Betaab Valley",
        "Aru Valley",
        "Gulmarg",
        "Gulmarg Ski Resort",
        "Kongdoori Lake",
        "Sonmarg",
        "Thajiwas Glacier",
        "Amarnath Cave",
        "Hazratbal Shrine"
    },
    Price = 2200,
    Inclusions = new List<string>
    {
        "Accommodation for 10 nights in 3-star hotels",
        "Daily breakfast",
        "Airport transfers",
        "Guided tours of all mentioned attractions",
        "Comfortable transportation for all intercity travel"
    },
    Exclusions = new List<string>
    {
        "International & domestic airfare",
        "Personal expenses such as shopping, tips, and meals not mentioned",
        "Travel insurance",
        "Additional activities not mentioned in the itinerary",
        "Entry tickets for optional attractions"
    }
},
new Destination
{
    Name = "North East India",
    ImageUrls = new List<string>
    {
        "/assets/img/destination/north_east_india.webp",
        "/assets/img/destination/north_east_india1.webp",
        "/assets/img/destination/north_east_india2.webp",
        "/assets/img/destination/north_east_india3.webp",
        "/assets/img/destination/north_east_india4.webp"
    },
    Description = "North East India, a region of diverse cultures and natural beauty, offers a unique blend of tribal heritage and stunning landscapes. Visitors can explore the Kaziranga National Park, visit the ancient temples of Assam, and experience the vibrant culture of Meghalaya. The region is also known for its tea gardens and waterfalls.",
    Itinerary = new List<string>
    {
        "Day 1: Arrival in Guwahati, check-in at the hotel, explore the city.",
        "Day 2: Visit the Kamakhya Temple and the surrounding area.",
        "Day 3: Travel to Kaziranga National Park, visit the park.",
        "Day 4: Explore the tea gardens of Assam.",
        "Day 5: Travel to Shillong, visit the Don Bosco Museum.",
        "Day 6: Explore the Mawsynram and the surrounding area.",
        "Day 7: Visit the Cherrapunji Waterfalls.",
        "Day 8: Travel to Darjeeling, visit the Tea Museum.",
        "Day 9: Explore the Tiger Hill and the surrounding area.",
        "Day 10: Departure."
    },
    WhatToDo = new List<string>
    {
        "Visit the Kamakhya Temple",
        "Explore Kaziranga National Park",
        "Visit the tea gardens of Assam",
        "Visit the Don Bosco Museum",
        "Explore Mawsynram",
        "Visit the Cherrapunji Waterfalls",
        "Visit the Tea Museum",
        "Explore Tiger Hill"
    },
    Places = new List<string>
    {
        "Guwahati",
        "Kamakhya Temple",
        "Kaziranga National Park",
        "Assam Tea Gardens",
        "Shillong",
        "Don Bosco Museum",
        "Mawsynram",
        "Cherrapunji Waterfalls",
        "Darjeeling",
        "Tea Museum",
        "Tiger Hill"
    },
    Price = 1300,
    Inclusions = new List<string>
    {
        "Accommodation for 10 nights in 4-star hotels",
        "Daily breakfast",
        "Airport transfers",
        "Guided tours of all mentioned attractions",
        "Comfortable transportation for all intercity travel"
    },
    Exclusions = new List<string>
    {
        "International & domestic airfare",
        "Personal expenses such as shopping, tips, and meals not mentioned",
        "Travel insurance",
        "Additional activities not mentioned in the itinerary",
        "Entry tickets for optional attractions"
    }
},

               new Destination
{
    Name = "Rajasthan",
    ImageUrls = new List<string>
    {
        "/assets/img/destination/Rajasthan.webp",
        "/assets/img/destination/Rajasthan1.webp",
        "/assets/img/destination/Rajasthan2.webp",
        "/assets/img/destination/Rajasthan3.webp",
        "/assets/img/destination/Rajasthan4.webp"
    },
    Description = "Rajasthan, known for its rich history and vibrant culture, offers a journey through its majestic forts and palaces. Visitors can explore the Amber Palace in Jaipur, visit the Mehrangarh Fort in Jodhpur, and experience the desert life in Jaisalmer. The state is also known for its colorful markets and traditional Rajasthani cuisine.",
    Itinerary = new List<string>
    {
        "Day 1: Arrival in Jaipur. Check-in at the hotel. Spend the afternoon exploring the Amber Palace, known for its stunning architecture and rich history. Enjoy a traditional Rajasthani dinner at a local restaurant.",
        "Day 2: Jaipur City Tour. Visit the City Palace and the surrounding area, including the Hawa Mahal. Explore the Jantar Mantar, an astronomical observatory. Relax in the evening and try some local street food.",
        "Day 3: Travel to Jodhpur. Visit the Mehrangarh Fort, one of the largest forts in India. Take a guided tour to learn about the fort's history and architecture.",
        "Day 4: Jodhpur Exploration. Explore Jaswant Thada, a beautiful cenotaph. Visit the Umaid Bhawan Palace Museum. Relax in the evening and enjoy local cuisine.",
        "Day 5: Travel to Udaipur. Visit the Lake Palace, a stunning palace on an island in Lake Pichola. Take a boat ride on the lake.",
        "Day 6: Udaipur Exploration. Explore the City Palace, known for its intricate architecture. Visit the Saheliyon ki Bari (Garden of the Maids of Honor). Enjoy a traditional Rajasthani dinner.",
        "Day 7: Travel to Jaisalmer. Visit the Jaisalmer Fort, a UNESCO World Heritage Site. Explore the fort's narrow lanes and historic palaces.",
        "Day 8: Jaisalmer Exploration. Explore the Sam Sand Dunes. Enjoy a camel ride and watch the sunset over the dunes. Spend the evening at a local restaurant.",
        "Day 9: Travel to Pushkar. Visit the Brahma Temple, one of the few temples dedicated to Lord Brahma. Explore the bustling Pushkar market.",
        "Day 10: Departure. Check-out from the hotel and depart from Pushkar."
    },
    WhatToDo = new List<string>
    {
        "Visit the Amber Palace",
        "Explore the City Palace in Jaipur",
        "Visit the Mehrangarh Fort",
        "Explore Jaswant Thada",
        "Visit the Lake Palace",
        "Explore the City Palace in Udaipur",
        "Visit the Jaisalmer Fort",
        "Explore the Sam Sand Dunes",
        "Visit the Brahma Temple"
    },
    Places = new List<string>
    {
        "Jaipur",
        "Amber Palace",
        "City Palace (Jaipur)",
        "Jodhpur",
        "Mehrangarh Fort",
        "Jaswant Thada",
        "Udaipur",
        "Lake Palace",
        "City Palace (Udaipur)",
        "Jaisalmer",
        "Jaisalmer Fort",
        "Sam Sand Dunes",
        "Pushkar",
        "Brahma Temple"
    },
    Price = 1700,
    Inclusions = new List<string>
    {
        "Accommodation for 10 nights in 4-star hotels",
        "Daily breakfast",
        "Airport transfers",
        "Guided tours of all mentioned attractions",
        "Comfortable transportation for all intercity travel"
    },
    Exclusions = new List<string>
    {
        "International & domestic airfare",
        "Personal expenses such as shopping, tips, and meals not mentioned",
        "Travel insurance",
        "Additional activities not mentioned in the itinerary",
        "Entry tickets for optional attractions"
    }
},
new Destination
{
    Name = "Dharamshala",
    ImageUrls = new List<string>
    {
        "/assets/img/destination/Dharamshala.webp",
        "/assets/img/destination/Dharamshala1.webp",
        "/assets/img/destination/Dharamshala2.webp",
        "/assets/img/destination/Dharamshala3.webp",
        "/assets/img/destination/Dharamshala4.webp"
    },
    Description = "Dharamshala, located in the Kangra Valley at the foothills of the Dhauladhar range in Himachal Pradesh, is known for its rich cultural heritage and natural beauty. It is also the center of the Tibetan government in exile. Visitors can explore the Dalai Lama Temple, Bhagsunag Waterfall, and the Norbulingka Institute. The region offers a blend of spirituality, adventure, and tranquility.",
    Itinerary = new List<string>
    {
        "Day 1: Arrival in Dharamshala. Check-in at your hotel in McLeod Ganj. Explore the Dalai Lama Temple and the surrounding area. Enjoy a traditional Tibetan dinner at a local restaurant.",
        "Day 2: Visit Bhagsunag Waterfall and Bhagsunag Temple. Take a short hike to the waterfall and enjoy the scenic views. Spend the afternoon exploring the local markets in McLeod Ganj.",
        "Day 3: Trek to Triund. Start early in the morning and enjoy the scenic trek through rhododendron forests. Reach Triund and enjoy panoramic views of the Dhauladhar Range. Return to McLeod Ganj in the evening.",
        "Day 4: Visit the Norbulingka Institute. Explore the workshops and gardens dedicated to preserving Tibetan culture and arts. Spend the afternoon at leisure, enjoying the peaceful ambiance of McLeod Ganj.",
        "Day 5: Travel to Dharamkot. Spend the day exploring the local attractions and enjoying the serene surroundings. Visit the nearby villages and interact with the locals.",
        "Day 6: Day trip to Palampur. Visit the tea gardens and enjoy the picturesque views. Explore the Tashi Jong Monastery and the Bundla Chasm waterfall.",
        "Day 7: Visit the Kangra Fort. Explore the ancient fort and learn about the region's rich history. Spend the evening at leisure in Dharamshala.",
        "Day 8: Explore the local cafes and restaurants in McLeod Ganj. Enjoy a leisurely day trying out different cuisines and relaxing in the peaceful environment.",
        "Day 9: Visit the Masroor Rock Cut Temples. Marvel at the intricate carvings and unique architecture of these ancient temples. Return to Dharamshala in the evening.",
        "Day 10: Departure. Check-out from the hotel and depart from Dharamshala."
    },
    WhatToDo = new List<string>
    {
        "Visit the Dalai Lama Temple",
        "Explore Bhagsunag Waterfall",
        "Trek to Triund",
        "Visit the Norbulingka Institute",
        "Explore Dharamkot",
        "Visit Palampur Tea Gardens",
        "Visit Kangra Fort",
        "Explore Local Cafes and Restaurants",
        "Visit Masroor Rock Cut Temples"
    },
    Places = new List<string>
    {
        "Dharamshala",
        "McLeod Ganj",
        "Dalai Lama Temple",
        "Bhagsunag Waterfall",
        "Bhagsunag Temple",
        "Triund",
        "Norbulingka Institute",
        "Dharamkot",
        "Palampur",
        "Kangra Fort",
        "Masroor Rock Cut Temples"
    },
    Price = 1500,
    Inclusions = new List<string>
    {
        "Accommodation for 10 nights in 3-star hotels",
        "Daily breakfast",
        "Airport transfers",
        "Guided tours of all mentioned attractions",
        "Comfortable transportation for all intercity travel"
    },
    Exclusions = new List<string>
    {
        "International & domestic airfare",
        "Personal expenses such as shopping, tips, and meals not mentioned",
        "Travel insurance",
        "Additional activities not mentioned in the itinerary",
        "Entry tickets for optional attractions"
    }
},
new Destination
{
    Name = "Dharamshala",
    ImageUrls = new List<string>
    {
        "/assets/img/destination/Dharamshala.webp",
        "/assets/img/destination/Dharamshala1.webp",
        "/assets/img/destination/Dharamshala2.webp",
        "/assets/img/destination/Dharamshala3.webp",
        "/assets/img/destination/Dharamshala4.webp"
    },
    Description = "Dharamshala, located in the Kangra Valley at the foothills of the Dhauladhar range in Himachal Pradesh, is known for its rich cultural heritage and natural beauty. It is also the center of the Tibetan government in exile. Visitors can explore the Dalai Lama Temple, Bhagsunag Waterfall, and the Norbulingka Institute. The region offers a blend of spirituality, adventure, and tranquility.",
    Itinerary = new List<string>
    {
        "Day 1: Arrival in Dharamshala. Check-in at your hotel in McLeod Ganj. Explore the Dalai Lama Temple and the surrounding area. Enjoy a traditional Tibetan dinner at a local restaurant.",
        "Day 2: Visit Bhagsunag Waterfall and Bhagsunag Temple. Take a short hike to the waterfall and enjoy the scenic views. Spend the afternoon exploring the local markets in McLeod Ganj.",
        "Day 3: Trek to Triund. Start early in the morning and enjoy the scenic trek through rhododendron forests. Reach Triund and enjoy panoramic views of the Dhauladhar Range. Return to McLeod Ganj in the evening.",
        "Day 4: Visit the Norbulingka Institute. Explore the workshops and gardens dedicated to preserving Tibetan culture and arts. Spend the afternoon at leisure, enjoying the peaceful ambiance of McLeod Ganj.",
        "Day 5: Travel to Dharamkot. Spend the day exploring the local attractions and enjoying the serene surroundings. Visit the nearby villages and interact with the locals.",
        "Day 6: Day trip to Palampur. Visit the tea gardens and enjoy the picturesque views. Explore the Tashi Jong Monastery and the Bundla Chasm waterfall.",
        "Day 7: Visit the Kangra Fort. Explore the ancient fort and learn about the region's rich history. Spend the evening at leisure in Dharamshala.",
        "Day 8: Explore the local cafes and restaurants in McLeod Ganj. Enjoy a leisurely day trying out different cuisines and relaxing in the peaceful environment.",
        "Day 9: Visit the Masroor Rock Cut Temples. Marvel at the intricate carvings and unique architecture of these ancient temples. Return to Dharamshala in the evening.",
        "Day 10: Departure. Check-out from the hotel and depart from Dharamshala."
    },
    WhatToDo = new List<string>
    {
        "Visit the Dalai Lama Temple",
        "Explore Bhagsunag Waterfall",
        "Trek to Triund",
        "Visit the Norbulingka Institute",
        "Explore Dharamkot",
        "Visit Palampur Tea Gardens",
        "Visit Kangra Fort",
        "Explore Local Cafes and Restaurants",
        "Visit Masroor Rock Cut Temples"
    },
    Places = new List<string>
    {
        "Dharamshala",
        "McLeod Ganj",
        "Dalai Lama Temple",
        "Bhagsunag Waterfall",
        "Bhagsunag Temple",
        "Triund",
        "Norbulingka Institute",
        "Dharamkot",
        "Palampur",
        "Kangra Fort",
        "Masroor Rock Cut Temples"
    },
    Price = 1500,
    Inclusions = new List<string>
    {
        "Accommodation for 10 nights in 3-star hotels",
        "Daily breakfast",
        "Airport transfers",
        "Guided tours of all mentioned attractions",
        "Comfortable transportation for all intercity travel"
    },
    Exclusions = new List<string>
    {
        "International & domestic airfare",
        "Personal expenses such as shopping, tips, and meals not mentioned",
        "Travel insurance",
        "Additional activities not mentioned in the itinerary",
        "Entry tickets for optional attractions"
    }
},

              new Destination
{
    Name = "Melbourne",
    ImageUrls = new List<string>
    {
        "/assets/img/destination/melbourne.webp",
        "/assets/img/destination/melbourne1.webp",
        "/assets/img/destination/melbourne2.webp",
        "/assets/img/destination/melbourne3.webp",
        "/assets/img/destination/melbourne4.webp"
    },
    Description = "Melbourne, the cultural capital of Australia, offers a vibrant mix of arts, sports, and coffee culture. Visitors can explore the iconic Federation Square, visit the Melbourne Cricket Ground, and enjoy the city's famous laneway cafes. The city is also known for its beautiful parks and gardens.",
    Itinerary = new List<string>
    {
        "Day 1: Arrival in Melbourne, check-in at the hotel, explore Federation Square.",
        "Day 2: Visit the Melbourne Cricket Ground and the Australian Sports Museum.",
        "Day 3: Explore the Royal Botanic Gardens.",
        "Day 4: Visit the Melbourne Zoo.",
        "Day 5: Explore the Queen Victoria Market.",
        "Day 6: Visit the National Gallery of Victoria.",
        "Day 7: Explore the laneway cafes and street art.",
        "Day 8: Travel to the Great Ocean Road.",
        "Day 9: Explore the Twelve Apostles.",
        "Day 10: Departure."
    },
    WhatToDo = new List<string>
    {
        "Visit Federation Square",
        "Explore the Melbourne Cricket Ground",
        "Visit the Australian Sports Museum",
        "Explore the Royal Botanic Gardens",
        "Visit the Melbourne Zoo",
        "Explore the Queen Victoria Market",
        "Visit the National Gallery of Victoria",
        "Explore laneway cafes and street art",
        "Travel to the Great Ocean Road",
        "Explore the Twelve Apostles"
    },
    Places = new List<string>
    {
        "Federation Square",
        "Melbourne Cricket Ground",
        "Australian Sports Museum",
        "Royal Botanic Gardens",
        "Melbourne Zoo",
        "Queen Victoria Market",
        "National Gallery of Victoria",
        "Great Ocean Road",
        "Twelve Apostles"
    },
    Price = 1600,
    Inclusions = new List<string>
    {
        "Accommodation for 10 nights in 4-star hotels",
        "Daily breakfast",
        "Airport transfers",
        "Guided tours of all mentioned attractions",
        "Comfortable transportation for all intercity travel"
    },
    Exclusions = new List<string>
    {
        "International & domestic airfare",
        "Personal expenses such as shopping, tips, and meals not mentioned",
        "Travel insurance",
        "Additional activities not mentioned in the itinerary",
        "Entry tickets for optional attractions"
    }
},

              new Destination
{
    Name = "The Grampians",
    ImageUrls = new List<string>
    {
        "/assets/img/destination/the_grampians.webp",
        "/assets/img/destination/the_grampians1.webp",
        "/assets/img/destination/the_grampians2.webp",
        "/assets/img/destination/the_grampians3.webp",
        "/assets/img/destination/the_grampians4.webp"
    },
    Description = "The Grampians, a national park in Victoria, offers stunning natural beauty and a variety of outdoor activities. Visitors can explore the rugged mountain ranges, visit the ancient Aboriginal rock art sites, and enjoy the scenic walks. The region is also known for its diverse flora and fauna.",
    Itinerary = new List<string>
    {
        "Day 1: Arrival in Melbourne, travel to The Grampians, check-in at the hotel.",
        "Day 2: Visit the Boroka Lookout and the surrounding area.",
        "Day 3: Explore the Pinnacle and the surrounding area.",
        "Day 4: Visit the MacKenzie Falls.",
        "Day 5: Explore the Aboriginal rock art sites.",
        "Day 6: Visit the Grampians National Park.",
        "Day 7: Explore the Halls Gap Zoo.",
        "Day 8: Travel to Ballarat, visit the Sovereign Hill.",
        "Day 9: Explore the Ballarat Wildlife Park.",
        "Day 10: Departure."
    },
    WhatToDo = new List<string>
    {
        "Visit Boroka Lookout",
        "Explore the Pinnacle",
        "Visit MacKenzie Falls",
        "Explore Aboriginal rock art sites",
        "Visit Grampians National Park",
        "Explore Halls Gap Zoo",
        "Visit Sovereign Hill",
        "Explore Ballarat Wildlife Park"
    },
    Places = new List<string>
    {
        "Melbourne",
        "Boroka Lookout",
        "Pinnacle",
        "MacKenzie Falls",
        "Aboriginal rock art sites",
        "Grampians National Park",
        "Halls Gap Zoo",
        "Ballarat",
        "Sovereign Hill",
        "Ballarat Wildlife Park"
    },
    Price = 1500,
    Inclusions = new List<string>
    {
        "Accommodation for 10 nights in 4-star hotels",
        "Daily breakfast",
        "Airport transfers",
        "Guided tours of all mentioned attractions",
        "Comfortable transportation for all intercity travel"
    },
    Exclusions = new List<string>
    {
        "International & domestic airfare",
        "Personal expenses such as shopping, tips, and meals not mentioned",
        "Travel insurance",
        "Additional activities not mentioned in the itinerary",
        "Entry tickets for optional attractions"
    }
},

              new Destination
{
    Name = "Gold Coast",
    ImageUrls = new List<string>
    {
        "/assets/img/destination/gold_coast.webp",
        "/assets/img/destination/gold_coast1.webp",
        "/assets/img/destination/gold_coast2.webp",
        "/assets/img/destination/gold_coast3.webp",
        "/assets/img/destination/gold_coast4.webp"
    },
    Description = "Discover the stunning Gold Coast, Australia's premier holiday destination known for its golden beaches, thrilling theme parks, and vibrant nightlife. From Surfers Paradise to the lush hinterlands, this tour offers the perfect mix of adventure, relaxation, and natural beauty. Enjoy exciting attractions, wildlife encounters, and breathtaking coastal views.",
    Itinerary = new List<string>
    {
        "Day 01: Arrival & Beachside Exploration. Arrive in Gold Coast and transfer to your hotel. Explore Surfers Paradise Beach and Cavill Avenue. Visit SkyPoint Observation Deck for stunning city views. Overnight stay in Gold Coast.",
        "Day 02: Theme Park Adventure - Movie World. Breakfast at the hotel. Full-day tour at Warner Bros. Movie World. Enjoy thrilling rides, live shows, and movie-themed attractions. Return to the hotel for overnight stay.",
        "Day 03: Marine Fun at Sea World. Breakfast at the hotel. Visit Sea World and explore marine exhibits, dolphin shows, and water rides. Enjoy a relaxing evening at Broadbeach. Overnight stay in Gold Coast.",
        "Day 04: Wildlife & Hinterland Experience. Breakfast at the hotel. Visit Currumbin Wildlife Sanctuary to see kangaroos and koalas. Take a scenic drive to the Gold Coast Hinterland. Explore Springbrook National Park and visit Natural Bridge. Return to Gold Coast for overnight stay.",
        "Day 05: Departure. Breakfast at the hotel. Free time for last-minute shopping or sightseeing. Transfer to the airport for departure."
    },
    WhatToDo = new List<string>
    {
        "Explore Surfers Paradise Beach",
        "Visit Warner Bros. Movie World",
        "Enjoy marine exhibits at Sea World",
        "Visit Currumbin Wildlife Sanctuary",
        "Take a scenic drive to Gold Coast Hinterland",
        "Explore Springbrook National Park",
        "Visit Natural Bridge",
        "Relax at Broadbeach"
    },
    Places = new List<string>
    {
        "Surfers Paradise Beach",
        "Warner Bros. Movie World",
        "Sea World",
        "Currumbin Wildlife Sanctuary",
        "Gold Coast Hinterland",
        "Springbrook National Park",
        "Natural Bridge",
        "Broadbeach"
    },
    Price = 1400,
    Inclusions = new List<string>
    {
        "Accommodation for 5 nights in 4-star hotels",
        "Daily breakfast",
        "Airport transfers",
        "Guided tours of all mentioned attractions",
        "Comfortable transportation for all intercity travel"
    },
    Exclusions = new List<string>
    {
        "International & domestic airfare",
        "Personal expenses such as shopping, tips, and meals not mentioned",
        "Travel insurance",
        "Additional activities not mentioned in the itinerary",
        "Entry tickets for optional attractions"
    }
},
new Destination
{
    Name = "Port Campbell",
    ImageUrls = new List<string>
    {
        "/assets/img/destination/port_campbell.webp",
        "/assets/img/destination/port_campbell1.webp",
        "/assets/img/destination/port_campbell2.webp",
        "/assets/img/destination/port_campbell3.webp",
        "/assets/img/destination/port_campbell4.webp"
    },
    Description = "Port Campbell, located along the Great Ocean Road, offers stunning coastal scenery and natural attractions. Visitors can explore the Twelve Apostles, visit the Loch Ard Gorge, and enjoy the scenic walks. The region is also known for its diverse flora and fauna.",
    Itinerary = new List<string>
    {
        "Day 1: Arrival in Port Campbell, check-in at the hotel, explore the town.",
        "Day 2: Visit the Twelve Apostles and the surrounding area.",
        "Day 3: Explore the Loch Ard Gorge.",
        "Day 4: Visit the London Arch.",
        "Day 5: Explore the Bay of Islands.",
        "Day 6: Visit the Bay of Martyrs.",
        "Day 7: Explore the Port Campbell National Park.",
        "Day 8: Travel to Warrnambool, visit the Flagstaff Hill Maritime Village.",
        "Day 9: Explore the Warrnambool Wildlife Park.",
        "Day 10: Departure."
    },
    WhatToDo = new List<string>
    {
        "Visit the Twelve Apostles",
        "Explore Loch Ard Gorge",
        "Visit London Arch",
        "Explore Bay of Islands",
        "Visit Bay of Martyrs",
        "Explore Port Campbell National Park",
        "Visit Flagstaff Hill Maritime Village",
        "Explore Warrnambool Wildlife Park"
    },
    Places = new List<string>
    {
        "Port Campbell",
        "Twelve Apostles",
        "Loch Ard Gorge",
        "London Arch",
        "Bay of Islands",
        "Bay of Martyrs",
        "Port Campbell National Park",
        "Warrnambool",
        "Flagstaff Hill Maritime Village",
        "Warrnambool Wildlife Park"
    },
    Price = 1500,
    Inclusions = new List<string>
    {
        "Accommodation for 10 nights in 4-star hotels",
        "Daily breakfast",
        "Airport transfers",
        "Guided tours of all mentioned attractions",
        "Comfortable transportation for all intercity travel"
    },
    Exclusions = new List<string>
    {
        "International & domestic airfare",
        "Personal expenses such as shopping, tips, and meals not mentioned",
        "Travel insurance",
        "Additional activities not mentioned in the itinerary",
        "Entry tickets for optional attractions"
    }
},
new Destination
{
    Name = "Golden Triangle",
    ImageUrls = new List<string>
    {
        "/assets/img/destination/golden_triangle.webp",
        "/assets/img/destination/golden_triangle1.png",
        "/assets/img/destination/golden_triangle2.webp",
        "/assets/img/destination/golden_triangle3.webp",
        "/assets/img/destination/golden_triangle4.webp"
    },
    Description = "Golden Triangle Tour Package is a classic journey through three of India's most iconic cities: Delhi, Agra, and Jaipur. Visit historic landmarks such as the Red Fort, Jama Masjid, Taj Mahal, and the Hawa Mahal in Jaipur. Experience the Mughal heritage, Rajasthani architecture, and vibrant culture that define India's rich history. This journey offers a perfect blend of history, architecture, and tradition.",
    Itinerary = new List<string>
    {
        "Day 01: Delhi - Upon arrival at Delhi airport, warm welcome by your private driver and transfer to hotel. Relax and refresh. Overnight stay in Delhi.",
        "Day 02: Delhi - Agra (210 Kms - 4:30 hrs) After breakfast explore Old & New Delhi. Evening drive to Agra. Overnight stay in Agra.",
        "Day 03: Agra - Jaipur (250 Kms - 5:00 hrs) Sunrise visit to the world-famous Taj Mahal. After breakfast, visit Agra Fort. Later, drive to Jaipur via Fatehpur Sikri. Overnight stay in Jaipur.",
        "Day 04: Jaipur - After breakfast, start the Jaipur city tour with Amber Fort, Sheesh Mahal, Maharaja's City Palace, and the Observatory. Drive past Hawa Mahal. Overnight stay in Jaipur.",
        "Day 05: Jaipur - Ajmer / Pushkar (150 Kms - 2:30 hrs) After breakfast, drive to Pushkar via Ajmer. Visit the Dargah in Ajmer, then continue to Pushkar. Visit the Brahma Temple and Pushkar Lake. Overnight stay in Pushkar.",
        "Day 06: Pushkar - Delhi - After breakfast, visit Brahma Temple and drive to Delhi. Drop at Airport or Railway Station for onward journey."
    },
    WhatToDo = new List<string>
    {
        "Visit the Red Fort",
        "Explore Jama Masjid",
        "Visit the Taj Mahal",
        "Explore the Agra Fort",
        "Visit Fatehpur Sikri",
        "Explore the Amber Fort",
        "Visit the Sheesh Mahal",
        "Explore Maharaja's City Palace",
        "Visit the Observatory",
        "Drive past Hawa Mahal",
        "Visit the Brahma Temple in Pushkar",
        "Visit Pushkar Lake and ghats"
    },
    Places = new List<string>
    {
        "Delhi",
        "Red Fort",
        "Jama Masjid",
        "Agra",
        "Taj Mahal",
        "Agra Fort",
        "Fatehpur Sikri",
        "Jaipur",
        "Amber Fort",
        "Sheesh Mahal",
        "Maharaja's City Palace",
        "Observatory",
        "Hawa Mahal",
        "Ajmer",
        "Pushkar",
        "Brahma Temple",
        "Pushkar Lake"
    },
    Price = 1600,
    Inclusions = new List<string>
    {
        "Accommodation for 6 nights in 4-star hotels",
        "Daily breakfast",
        "Airport transfers",
        "Guided tours of all mentioned attractions",
        "Comfortable transportation for all intercity travel"
    },
    Exclusions = new List<string>
    {
        "International & domestic airfare",
        "Personal expenses such as shopping, tips, and meals not mentioned",
        "Travel insurance",
        "Additional activities not mentioned in the itinerary",
        "Entry tickets for optional attractions"
    }
},

               new Destination
{
    Name = "Sydney",
    ImageUrls = new List<string>
    {
        "/assets/img/destination/Sydney.webp",
        "/assets/img/destination/Sydney1.webp",
        "/assets/img/destination/Sydney2.webp",
        "/assets/img/destination/Sydney3.webp",
        "/assets/img/destination/Sydney4.webp"
    },
    Description = "Experience the vibrant city of Sydney, Australia's iconic metropolis, filled with breathtaking attractions, stunning beaches, and rich cultural experiences. From the world-famous Sydney Opera House and Harbour Bridge to the golden sands of Bondi Beach, this tour offers an unforgettable adventure. Enjoy scenic cruises, wildlife encounters, and guided city explorations, making this a perfect getaway.",
    Itinerary = new List<string>
    {
        "Day 01: Arrival & City Exploration - Arrive in Sydney and transfer to your hotel. Visit Circular Quay, the hub of Sydney's waterfront attractions. Explore The Rocks and learn about Sydney's colonial history. Enjoy sunset views from the Sydney Tower Eye. Overnight stay in Sydney.",
        "Day 02: Sydney Icons & Harbour Cruise - Breakfast at the hotel. Visit the Sydney Opera House with a guided tour. Walk across the Sydney Harbour Bridge for stunning views. Take a relaxing Sydney Harbour Cruise. Spend leisure time at Darling Harbour. Overnight stay in Sydney.",
        "Day 03: Blue Mountains & Wildlife Adventure - Early morning breakfast. Drive to the Blue Mountains National Park. Visit the Three Sisters rock formation and Echo Point. Ride the Scenic Railway and Skyway for panoramic views. Explore Featherdale Wildlife Park and interact with kangaroos and koalas. Return to Sydney for overnight stay.",
        "Day 04: Beach & Coastal Fun - Breakfast at the hotel. Enjoy a coastal walk from Bondi to Coogee Beach. Visit Taronga Zoo to see Australian wildlife. Ferry ride to Manly Beach and explore the local area. Evening free for shopping and leisure. Overnight stay in Sydney.",
        "Day 05: Departure - Breakfast at the hotel. Free time for last-minute shopping or sightseeing. Transfer to the airport for departure."
    },
    WhatToDo = new List<string>
    {
        "Visit Sydney Opera House",
        "Explore Sydney Harbour Bridge",
        "Relax at Bondi Beach",
        "Discover Darling Harbour",
        "Hike in Blue Mountains",
        "Interact with wildlife at Taronga Zoo",
        "Stroll through The Rocks",
        "Ferry ride to Manly Beach"
    },
    Places = new List<string>
    {
        "Sydney Opera House",
        "Sydney Harbour Bridge",
        "Bondi Beach",
        "Darling Harbour",
        "Blue Mountains",
        "Taronga Zoo",
        "The Rocks",
        "Manly Beach"
    },
    Price = 1800,
    Inclusions = new List<string>
    {
        "Accommodation for 5 nights in 4-star hotels",
        "Daily breakfast",
        "Airport transfers",
        "Guided tours of all mentioned attractions",
        "Comfortable transportation for all intercity travel"
    },
    Exclusions = new List<string>
    {
        "International & domestic airfare",
        "Personal expenses such as shopping, tips, and meals not mentioned",
        "Travel insurance",
        "Additional activities not mentioned in the itinerary",
        "Entry tickets for optional attractions"
    }
},

               new Destination
{
    Name = "Melbourne",
    ImageUrls = new List<string>
    {
        "/assets/img/destination/Melbourne.webp",
        "/assets/img/destination/Melbourne1.webp",
        "/assets/img/destination/Melbourne2.webp",
        "/assets/img/destination/Melbourne3.webp",
        "/assets/img/destination/Melbourne4.webp"
    },
    Description = "Discover the vibrant city of Melbourne, known for its artistic culture, bustling laneways, iconic landmarks, and stunning coastal drives. From the famous Federation Square and the Yarra River to the picturesque Great Ocean Road, this tour promises an unforgettable experience. Enjoy a mix of history, wildlife encounters, and culinary delights in Australia's cultural capital.",
    Itinerary = new List<string>
    {
        "Day 01: Arrival & City Exploration - Arrive in Melbourne and transfer to your hotel. Visit Federation Square and explore the vibrant laneways. Stroll along the Yarra River and enjoy the city's skyline. Visit Eureka Skydeck for an aerial view of Melbourne. Overnight stay in Melbourne.",
        "Day 02: Melbourne City Tour & Shopping - Breakfast at the hotel. Visit Flinders Street Station and St. Paul's Cathedral. Explore Queen Victoria Market for local delights. Walk through Hosier Lane to see Melbourne's famous street art. Leisure time for shopping at Bourke Street Mall. Overnight stay in Melbourne.",
        "Day 03: Great Ocean Road Adventure - Early morning breakfast. Depart for the Great Ocean Road tour. Stop at the Twelve Apostles and Loch Ard Gorge. Visit Apollo Bay and enjoy stunning coastal views. Return to Melbourne for overnight stay.",
        "Day 04: Phillip Island Wildlife Tour - Breakfast at the hotel. Visit the Moonlit Sanctuary Wildlife Conservation Park. Explore the Nobbies Boardwalk with scenic ocean views. Watch the famous Penguin Parade at sunset. Return to Melbourne for overnight stay.",
        "Day 05: Departure - Breakfast at the hotel. Free time for last-minute shopping or sightseeing. Transfer to the airport for departure."
    },
    WhatToDo = new List<string>
    {
        "Visit Federation Square",
        "Explore Flinders Street Station",
        "Shop at Queen Victoria Market",
        "Walk through Hosier Lane",
        "Drive along Great Ocean Road",
        "Watch Penguin Parade",
        "Visit Moonlit Sanctuary Wildlife Conservation Park",
        "Explore Nobbies Boardwalk"
    },
    Places = new List<string>
    {
        "Federation Square",
        "Flinders Street Station",
        "Queen Victoria Market",
        "Great Ocean Road",
        "Phillip Island",
        "Moonlit Sanctuary Wildlife Conservation Park",
        "Nobbies Boardwalk",
        "Penguin Parade"
    },
    Price = 1700,
    Inclusions = new List<string>
    {
        "Accommodation for 5 nights in 4-star hotels",
        "Daily breakfast",
        "Airport transfers",
        "Guided tours of all mentioned attractions",
        "Comfortable transportation for all intercity travel"
    },
    Exclusions = new List<string>
    {
        "International & domestic airfare",
        "Personal expenses such as shopping, tips, and meals not mentioned",
        "Travel insurance",
        "Additional activities not mentioned in the itinerary",
        "Entry tickets for optional attractions"
    }
},

              new Destination
{
    Name = "Brisbane",
    ImageUrls = new List<string>
    {
        "/assets/img/destination/Brisbane.webp",
        "/assets/img/destination/Brisbane1.webp",
        "/assets/img/destination/Brisbane2.webp",
        "/assets/img/destination/Brisbane3.webp",
        "/assets/img/destination/Brisbane4.webp"
    },
    Description = "Discover the vibrant city of Brisbane, a perfect blend of modern attractions, stunning riverfront views, and cultural heritage. From the breathtaking Story Bridge to the lively South Bank and the scenic Moreton Island, this tour offers an unforgettable experience. Enjoy adventure, wildlife encounters, and leisure in Australia's sunshine capital.",
    Itinerary = new List<string>
    {
        "Day 01: Arrival & City Exploration - Arrive in Brisbane and transfer to your hotel. Visit South Bank Parklands and enjoy the man-made Streets Beach. Take a scenic walk along the Brisbane River. Experience breathtaking sunset views from Mount Coot-tha Lookout. Overnight stay in Brisbane.",
        "Day 02: Brisbane Highlights & Wildlife Experience - Breakfast at the hotel. Visit the Lone Pine Koala Sanctuary and interact with koalas and kangaroos. Explore Roma Street Parkland, a beautiful inner-city retreat. Walk through the historic Brisbane City Hall and King George Square. Free time for shopping at Queen Street Mall. Overnight stay in Brisbane.",
        "Day 03: Moreton Island Adventure - Early morning breakfast. Take a ferry to Moreton Island. Enjoy sandboarding and snorkeling at Tangalooma Wrecks. Relax on the pristine beaches and explore crystal-clear lagoons. Return to Brisbane for overnight stay.",
        "Day 04: River Cruise & Story Bridge Climb - Breakfast at the hotel. Take a relaxing Brisbane River Cruise. Visit the Queensland Art Gallery & Gallery of Modern Art (QAGOMA). Experience the thrilling Story Bridge Climb for spectacular city views. Evening free for personal exploration or leisure activities. Overnight stay in Brisbane.",
        "Day 05: Departure - Breakfast at the hotel. Free time for last-minute shopping or sightseeing. Transfer to the airport for departure."
    },
    WhatToDo = new List<string>
    {
        "Visit South Bank Parklands",
        "Explore Lone Pine Koala Sanctuary",
        "Take a ferry to Moreton Island",
        "Enjoy sandboarding at Tangalooma Wrecks",
        "Take a Brisbane River Cruise",
        "Climb Story Bridge",
        "Visit Queensland Art Gallery & Gallery of Modern Art",
        "Walk through Roma Street Parkland"
    },
    Places = new List<string>
    {
        "South Bank Parklands",
        "Story Bridge",
        "Lone Pine Koala Sanctuary",
        "Mount Coot-tha Lookout",
        "Moreton Island",
        "Brisbane River",
        "Queen Street Mall",
        "Roma Street Parkland"
    },
    Price = 1600,
    Inclusions = new List<string>
    {
        "Accommodation for 5 nights in 4-star hotels",
        "Daily breakfast",
        "Airport transfers",
        "Guided tours of all mentioned attractions",
        "Comfortable transportation for all intercity travel"
    },
    Exclusions = new List<string>
    {
        "International & domestic airfare",
        "Personal expenses such as shopping, tips, and meals not mentioned",
        "Travel insurance",
        "Additional activities not mentioned in the itinerary",
        "Entry tickets for optional attractions"
    }
},

               new Destination
{
    Name = "Perth",
    ImageUrls = new List<string>
    {
        "/assets/img/destination/Perth.webp",
        "/assets/img/destination/Perth1.webp",
        "/assets/img/destination/Perth2.webp",
        "/assets/img/destination/Perth3.webp",
        "/assets/img/destination/Perth4.webp"
    },
    Description = "Explore Perth, the sunniest capital of Australia, known for its beautiful beaches, stunning river views, and unique wildlife. From the breathtaking Kings Park to the stunning Rottnest Island, this tour offers a perfect mix of nature, adventure, and city exploration. Enjoy pristine coastal landscapes, vibrant markets, and a relaxed atmosphere in Western Australia's capital.",
    Itinerary = new List<string>
    {
        "Day 01: Arrival & City Exploration - Arrive in Perth and transfer to your hotel. Visit Kings Park & Botanic Garden for panoramic city views. Explore Elizabeth Quay and enjoy the waterfront attractions. Enjoy a scenic dinner cruise along the Swan River. Overnight stay in Perth.",
        "Day 02: Fremantle & Beachside Leisure - Breakfast at the hotel. Visit Fremantle and explore the historic Fremantle Markets. Tour Fremantle Prison, a UNESCO-listed heritage site. Relax at Cottesloe Beach and enjoy the sunset. Overnight stay in Perth.",
        "Day 03: Rottnest Island Adventure - Early morning breakfast. Ferry ride to Rottnest Island. Explore the island on a guided tour, including quokka encounters. Swim and relax at Pinky Beach or The Basin. Return to Perth for overnight stay.",
        "Day 04: Pinnacles Desert & Wildlife Experience - Breakfast at the hotel. Day trip to The Pinnacles Desert in Nambung National Park. Stop at Cervantes to see the stromatolites. Visit Caversham Wildlife Park to interact with kangaroos and koalas. Return to Perth for overnight stay.",
        "Day 05: Departure - Breakfast at the hotel. Free time for last-minute shopping or sightseeing. Transfer to the airport for departure."
    },
    WhatToDo = new List<string>
    {
        "Visit Kings Park & Botanic Garden",
        "Explore Elizabeth Quay",
        "Take a dinner cruise along Swan River",
        "Visit Fremantle Markets",
        "Tour Fremantle Prison",
        "Relax at Cottesloe Beach",
        "Ferry ride to Rottnest Island",
        "Explore The Pinnacles Desert",
        "Visit Caversham Wildlife Park"
    },
    Places = new List<string>
    {
        "Kings Park & Botanic Garden",
        "Swan River",
        "Elizabeth Quay",
        "Rottnest Island",
        "Fremantle",
        "Cottesloe Beach",
        "The Pinnacles Desert",
        "Caversham Wildlife Park"
    },
    Price = 1700,
    Inclusions = new List<string>
    {
        "Accommodation for 5 nights in 4-star hotels",
        "Daily breakfast",
        "Airport transfers",
        "Guided tours of all mentioned attractions",
        "Comfortable transportation for all intercity travel"
    },
    Exclusions = new List<string>
    {
        "International & domestic airfare",
        "Personal expenses such as shopping, tips, and meals not mentioned",
        "Travel insurance",
        "Additional activities not mentioned in the itinerary",
        "Entry tickets for optional attractions"
    }
},

             new Destination
{
    Name = "Adelaide",
    ImageUrls = new List<string>
    {
        "/assets/img/destination/Adelaide.webp",
        "/assets/img/destination/Adelaide1.webp",
        "/assets/img/destination/Adelaide2.webp",
        "/assets/img/destination/Adelaide3.webp",
        "/assets/img/destination/Adelaide4.webp"
    },
    Description = "Experience the charm of Adelaide, a city known for its elegant architecture, lush parklands, and world-class wineries. From the vibrant Central Market to the picturesque Barossa Valley, this tour offers a perfect mix of cultural exploration, nature, and gourmet experiences. Discover the best of South Australia's capital with exciting city highlights and scenic day trips.",
    Itinerary = new List<string>
    {
        "Day 01: Arrival & City Exploration - Arrive in Adelaide and transfer to your hotel. Visit Adelaide Central Market for a taste of local flavors. Explore Rundle Mall for shopping and street performances. Enjoy an evening walk at Adelaide Botanic Garden. Overnight stay in Adelaide.",
        "Day 02: Barossa Valley Wine Tour - Breakfast at the hotel. Take a full-day guided tour of Barossa Valley. Visit famous wineries like Penfolds, Jacob's Creek, and Seppeltsfield. Enjoy a gourmet lunch paired with local wines. Return to Adelaide for overnight stay.",
        "Day 03: Kangaroo Island Adventure - Early morning breakfast. Take a ferry or flight to Kangaroo Island. Visit Seal Bay Conservation Park and Flinders Chase National Park. Explore Remarkable Rocks and Admirals Arch. Return to Adelaide for overnight stay.",
        "Day 04: Adelaide Hills & Wildlife Experience - Breakfast at the hotel. Visit Hahndorf, Australia's oldest German settlement. Explore Cleland Wildlife Park to see koalas and kangaroos. Take in panoramic views from Mount Lofty Summit. Enjoy an evening at Glenelg Beach. Overnight stay in Adelaide.",
        "Day 05: Departure - Breakfast at the hotel. Free time for last-minute shopping or sightseeing. Transfer to the airport for departure."
    },
    WhatToDo = new List<string>
    {
        "Visit Adelaide Central Market",
        "Explore Rundle Mall",
        "Take a walk through Adelaide Botanic Garden",
        "Tour Barossa Valley wineries",
        "Visit Kangaroo Island",
        "Explore Seal Bay Conservation Park",
        "Visit Flinders Chase National Park",
        "See Remarkable Rocks and Admirals Arch",
        "Visit Hahndorf",
        "Explore Cleland Wildlife Park",
        "Take in views from Mount Lofty Summit",
        "Enjoy Glenelg Beach"
    },
    Places = new List<string>
    {
        "Adelaide Central Market",
        "Rundle Mall",
        "Adelaide Botanic Garden",
        "Barossa Valley",
        "Kangaroo Island",
        "Seal Bay Conservation Park",
        "Flinders Chase National Park",
        "Remarkable Rocks",
        "Admirals Arch",
        "Hahndorf",
        "Cleland Wildlife Park",
        "Mount Lofty Summit",
        "Glenelg Beach"
    },
    Price = 1600,
    Inclusions = new List<string>
    {
        "Accommodation for 5 nights in 4-star hotels",
        "Daily breakfast",
        "Airport transfers",
        "Guided tours of all mentioned attractions",
        "Comfortable transportation for all intercity travel"
    },
    Exclusions = new List<string>
    {
        "International & domestic airfare",
        "Personal expenses such as shopping, tips, and meals not mentioned",
        "Travel insurance",
        "Additional activities not mentioned in the itinerary",
        "Entry tickets for optional attractions"
    }
},

               new Destination
{
    Name = "Cairns",
    ImageUrls = new List<string>
    {
        "/assets/img/destination/Cairns.webp",
        "/assets/img/destination/Cairns1.webp",
        "/assets/img/destination/Cairns2.webp",
        "/assets/img/destination/Cairns3.webp",
        "/assets/img/destination/Cairns4.webp"
    },
    Description = "Explore the tropical paradise of Cairns, the gateway to the Great Barrier Reef and the lush rainforests of North Queensland. This tour offers an incredible mix of adventure, natural beauty, and cultural experiences. From snorkeling in the world's largest coral reef system to exploring the heritage-listed Daintree Rainforest, Cairns is the perfect destination for nature lovers and thrill-seekers alike.",
    Itinerary = new List<string>
    {
        "Day 01: Arrival & City Exploration - Arrive in Cairns and transfer to your hotel. Visit the Cairns Esplanade and relax by the Lagoon. Explore the local night markets for souvenirs and tropical delicacies. Overnight stay in Cairns.",
        "Day 02: Great Barrier Reef Adventure - Early morning breakfast. Full-day tour to the Great Barrier Reef. Enjoy snorkeling, diving, or a glass-bottom boat ride. Buffet lunch on board the cruise. Return to Cairns for overnight stay.",
        "Day 03: Kuranda Rainforest & Scenic Railway - Breakfast at the hotel. Take the Kuranda Scenic Railway through the rainforest. Explore the Kuranda Village and its markets. Return via the Skyrail Rainforest Cableway with breathtaking views. Overnight stay in Cairns.",
        "Day 04: Daintree Rainforest & Cape Tribulation - Breakfast at the hotel. Visit the Daintree Rainforest and Mossman Gorge. Explore Cape Tribulation, where the rainforest meets the reef. Crocodile-spotting cruise on the Daintree River. Return to Cairns for overnight stay.",
        "Day 05: Departure - Breakfast at the hotel. Free time for last-minute shopping or relaxing. Transfer to the airport for departure."
    },
    WhatToDo = new List<string>
    {
        "Visit Cairns Esplanade",
        "Explore local night markets",
        "Snorkel or dive at Great Barrier Reef",
        "Take Kuranda Scenic Railway",
        "Explore Kuranda Village",
        "Return via Skyrail Rainforest Cableway",
        "Visit Daintree Rainforest",
        "Explore Mossman Gorge",
        "Visit Cape Tribulation",
        "Take crocodile-spotting cruise on Daintree River"
    },
    Places = new List<string>
    {
        "Cairns Esplanade",
        "Great Barrier Reef",
        "Kuranda Scenic Railway",
        "Skyrail Rainforest Cableway",
        "Daintree Rainforest",
        "Mossman Gorge",
        "Cape Tribulation",
        "Daintree River"
    },
    Price = 1500,
    Inclusions = new List<string>
    {
        "Accommodation for 5 nights in 4-star hotels",
        "Daily breakfast",
        "Airport transfers",
        "Guided tours of all mentioned attractions",
        "Comfortable transportation for all intercity travel"
    },
    Exclusions = new List<string>
    {
        "International & domestic airfare",
        "Personal expenses such as shopping, tips, and meals not mentioned",
        "Travel insurance",
        "Additional activities not mentioned in the itinerary",
        "Entry tickets for optional attractions"
    }
},



               new Destination
{
    Name = "Hobart",
    ImageUrls = new List<string>
    {
        "/assets/img/destination/Hobart.webp",
        "/assets/img/destination/Hobart1.webp",
        "/assets/img/destination/Hobart2.webp",
        "/assets/img/destination/Hobart3.webp",
        "/assets/img/destination/Hobart4.webp"
    },
    Description = "Experience the charm of Hobart, the capital of Tasmania, known for its stunning landscapes, rich history, and vibrant cultural scene. From breathtaking coastal views to unique wildlife encounters, this tour offers a perfect blend of nature, adventure, and gourmet experiences. Explore iconic landmarks like Mount Wellington, MONA, and the historic Port Arthur site while indulging in Tasmania's world-renowned produce and wines.",
    Itinerary = new List<string>
    {
        "Day 01: Arrival & City Exploration - Arrive in Hobart and transfer to the hotel. Explore Salamanca Place and Battery Point. Visit the waterfront and enjoy fresh seafood at a local restaurant. Overnight stay in Hobart.",
        "Day 02: Mount Wellington & MONA - Breakfast at the hotel. Drive up to Mount Wellington for spectacular views. Visit MONA (Museum of Old and New Art) for a unique cultural experience. Enjoy a relaxing afternoon at the Royal Tasmanian Botanical Gardens. Overnight stay in Hobart.",
        "Day 03: Port Arthur & Tasman Peninsula - Breakfast at the hotel. Full-day guided tour of the Port Arthur Historic Site. Explore Tasman Arch, Devil's Kitchen, and the Remarkable Cave. Visit the Coal Mines Historic Site (optional). Return to Hobart for overnight stay.",
        "Day 04: Bruny Island Adventure - Breakfast at the hotel. Take a ferry to Bruny Island. Visit The Neck Lookout for breathtaking coastal views. Enjoy cheese, oysters, and honey tastings at local farms. Spot wildlife like seals, penguins, and white wallabies. Return to Hobart for overnight stay.",
        "Day 05: Wildlife & Departure - Breakfast at the hotel. Visit Bonorong Wildlife Sanctuary to see Tasmanian devils. Explore Richmond Village and its historic bridge. Free time for last-minute shopping or sightseeing. Transfer to the airport for departure."
    },
    WhatToDo = new List<string>
    {
        "Explore Salamanca Place",
        "Visit Battery Point",
        "Drive up Mount Wellington",
        "Visit MONA",
        "Relax at Royal Tasmanian Botanical Gardens",
        "Tour Port Arthur Historic Site",
        "Explore Tasman Arch",
        "Visit Devil's Kitchen",
        "See Remarkable Cave",
        "Visit Coal Mines Historic Site",
        "Take a ferry to Bruny Island",
        "Visit The Neck Lookout",
        "Enjoy local tastings",
        "Spot wildlife",
        "Visit Bonorong Wildlife Sanctuary",
        "Explore Richmond Village"
    },
    Places = new List<string>
    {
        "Salamanca Place",
        "Battery Point",
        "Mount Wellington",
        "MONA",
        "Port Arthur Historic Site",
        "Tasman Arch",
        "Devil's Kitchen",
        "Remarkable Cave",
        "Coal Mines Historic Site",
        "Bruny Island",
        "The Neck Lookout",
        "Bonorong Wildlife Sanctuary",
        "Richmond Village"
    },
    Price = 1500,
    Inclusions = new List<string>
    {
        "Accommodation for 5 nights in 4-star hotels",
        "Daily breakfast",
        "Airport transfers",
        "Guided tours of all mentioned attractions",
        "Comfortable transportation for all intercity travel"
    },
    Exclusions = new List<string>
    {
        "International & domestic airfare",
        "Personal expenses such as shopping, tips, and meals not mentioned",
        "Travel insurance",
        "Additional activities not mentioned in the itinerary",
        "Entry tickets for optional attractions"
    }
},

               new Destination
{
    Name = "Canberra",
    ImageUrls = new List<string>
    {
        "/assets/img/destination/Canberra.webp",
        "/assets/img/destination/Canberra1.webp",
        "/assets/img/destination/Canberra2.webp",
        "/assets/img/destination/Canberra3.webp",
        "/assets/img/destination/Canberra4.webp"
    },
    Description = "Discover the capital city of Australia, Canberra, a perfect blend of history, culture, and nature. This tour will take you through iconic landmarks, world-class museums, and scenic landscapes. Visit Parliament House, explore the Australian War Memorial, enjoy stunning views from Mount Ainslie, and relax at Lake Burley Griffin. Whether you are a history buff, a nature lover, or an art enthusiast, Canberra has something to offer for everyone.",
    Itinerary = new List<string>
    {
        "Day 01: Arrival & City Exploration - Arrive in Canberra and transfer to your hotel. Visit the Australian War Memorial to explore its rich history. Take a walk along Lake Burley Griffin and enjoy the scenic views. Overnight stay in Canberra.",
        "Day 02: Parliament House & Cultural Tour - Breakfast at the hotel. Explore the iconic Parliament House and witness the Australian government in action. Visit the National Museum of Australia to learn about the country's heritage. Explore the National Gallery of Australia and admire its vast art collection. Enjoy the vibrant dining scene at Kingston Foreshore. Overnight stay in Canberra.",
        "Day 03: Nature & Science Exploration - Breakfast at the hotel. Head to Tidbinbilla Nature Reserve for a wildlife experience. Visit Questacon for interactive science exhibits and fun activities. Take a drive to Mount Ainslie Lookout for stunning city views. Free time for shopping or leisure activities. Overnight stay in Canberra.",
        "Day 04: Departure - Breakfast at the hotel. Free time for last-minute sightseeing or relaxation. Transfer to the airport for departure."
    },
    WhatToDo = new List<string>
    {
        "Visit Australian War Memorial",
        "Take a walk along Lake Burley Griffin",
        "Explore Parliament House",
        "Visit National Museum of Australia",
        "Explore National Gallery of Australia",
        "Enjoy Kingston Foreshore",
        "Visit Tidbinbilla Nature Reserve",
        "Visit Questacon",
        "Take a drive to Mount Ainslie Lookout"
    },
    Places = new List<string>
    {
        "Australian War Memorial",
        "Lake Burley Griffin",
        "Parliament House",
        "National Museum of Australia",
        "National Gallery of Australia",
        "Kingston Foreshore",
        "Tidbinbilla Nature Reserve",
        "Questacon",
        "Mount Ainslie Lookout"
    },
    Price = 1400,
    Inclusions = new List<string>
    {
        "Accommodation for 4 nights in 4-star hotels",
        "Daily breakfast",
        "Airport transfers",
        "Guided tours of all mentioned attractions",
        "Comfortable transportation for all intercity travel"
    },
    Exclusions = new List<string>
    {
        "International & domestic airfare",
        "Personal expenses such as shopping, tips, and meals not mentioned",
        "Travel insurance",
        "Additional activities not mentioned in the itinerary",
        "Entry tickets for optional attractions"
    }
},

                new Destination
{
    Name = "Victoria",
    ImageUrls = new List<string>
    {
        "/assets/img/destination/Victoria.webp",
        "/assets/img/destination/Victoria1.webp",
        "/assets/img/destination/Victoria2.webp",
        "/assets/img/destination/Victoria3.webp",
        "/assets/img/destination/Victoria4.webp"
    },
    Description = "Victoria, Australia, offers a perfect mix of natural beauty, cultural heritage, and vibrant city life. This tour takes you through the bustling city of Melbourne, the stunning Great Ocean Road, the picturesque Yarra Valley, and the scenic landscapes of Phillip Island. Discover iconic landmarks, enjoy wildlife encounters, and indulge in world-class food and wine experiences.",
    Itinerary = new List<string>
    {
        "Day 01: Arrival & Melbourne City Tour - Arrive in Melbourne and transfer to the hotel. Explore Federation Square, Flinders Street Station, and Southbank. Visit Queen Victoria Market for shopping and local delicacies. Relax at the Royal Botanic Gardens. Overnight stay in Melbourne.",
        "Day 02: Great Ocean Road Adventure - Breakfast at the hotel. Full-day tour along the Great Ocean Road. Stop at the Twelve Apostles and Loch Ard Gorge. Visit coastal towns like Lorne and Apollo Bay. Return to Melbourne for overnight stay.",
        "Day 03: Phillip Island Wildlife Experience - Breakfast at the hotel. Visit the Koala Conservation Centre and The Nobbies. Witness the famous Penguin Parade at sunset. Return to Melbourne for overnight stay.",
        "Day 04: Yarra Valley Wine & Dandenong Ranges - Breakfast at the hotel. Enjoy a scenic drive to the Yarra Valley. Wine tasting at premium wineries and gourmet lunch. Visit the Dandenong Ranges and ride the Puffing Billy steam train. Return to Melbourne for overnight stay.",
        "Day 05: Grampians National Park Exploration - Breakfast at the hotel. Full-day trip to Grampians National Park. Visit MacKenzie Falls and Reeds Lookout. Spot native wildlife like kangaroos and emus. Return to Melbourne for overnight stay.",
        "Day 06: Mornington Peninsula & Departure - Breakfast at the hotel. Visit the Mornington Peninsula hot springs (optional). Explore wineries and coastal trails. Transfer to the airport for departure."
    },
    WhatToDo = new List<string>
    {
        "Explore Federation Square",
        "Visit Flinders Street Station",
        "Shop at Queen Victoria Market",
        "Relax at Royal Botanic Gardens",
        "Tour Great Ocean Road",
        "Stop at Twelve Apostles",
        "Visit Loch Ard Gorge",
        "Visit Koala Conservation Centre",
        "Witness Penguin Parade",
        "Enjoy Yarra Valley wine tasting",
        "Ride Puffing Billy steam train",
        "Explore Grampians National Park",
        "Visit MacKenzie Falls",
        "Spot native wildlife",
        "Visit Mornington Peninsula hot springs",
        "Explore coastal trails"
    },
    Places = new List<string>
    {
        "Federation Square",
        "Flinders Street Station",
        "Queen Victoria Market",
        "Great Ocean Road",
        "Phillip Island",
        "Koala Conservation Centre",
        "Nobbies",
        "Penguin Parade",
        "Yarra Valley",
        "Dandenong Ranges",
        "Grampians National Park",
        "Mornington Peninsula"
    },
    Price = 1800,
    Inclusions = new List<string>
    {
        "Accommodation for 6 nights in 4-star hotels",
        "Daily breakfast",
        "Airport transfers",
        "Guided tours of all mentioned attractions",
        "Comfortable transportation for all intercity travel"
    },
    Exclusions = new List<string>
    {
        "International & domestic airfare",
        "Personal expenses such as shopping, tips, and meals not mentioned",
        "Travel insurance",
        "Additional activities not mentioned in the itinerary",
        "Entry tickets for optional attractions"
    }
},
new Destination
{
    Name = "Queensland",
    ImageUrls = new List<string>
    {
        "/assets/img/destination/Queensland.webp",
        "/assets/img/destination/Queensland1.webp",
        "/assets/img/destination/Queensland2.webp",
        "/assets/img/destination/Queensland3.webp",
        "/assets/img/destination/Queensland4.webp"
    },
    Description = "Discover the stunning natural beauty and vibrant cities of Queensland, Australia. From the world-famous Great Barrier Reef to the lush rainforests, this tour offers an incredible mix of adventure, relaxation, and wildlife encounters. Explore the Gold Coast's pristine beaches, visit the Daintree Rainforest, and immerse yourself in the cultural and natural wonders of this diverse state.",
    Itinerary = new List<string>
    {
        "Day 01: Arrival in Brisbane & City Tour - Arrive in Brisbane and transfer to the hotel. Explore South Bank Parklands and Brisbane River. Visit Lone Pine Koala Sanctuary for wildlife encounters. Overnight stay in Brisbane.",
        "Day 02: Gold Coast Adventure - Breakfast at the hotel. Travel to the Gold Coast. Visit Surfers Paradise and enjoy beach activities. Experience thrill rides at Movie World or Dreamworld. Return to Brisbane for overnight stay.",
        "Day 03: Cairns & Great Barrier Reef - Breakfast at the hotel. Fly to Cairns and check into the hotel. Full-day Great Barrier Reef cruise with snorkeling or scuba diving. Return to Cairns for overnight stay.",
        "Day 04: Daintree Rainforest & Cape Tribulation - Breakfast at the hotel. Explore Daintree Rainforest and Mossman Gorge. Visit Cape Tribulation where the rainforest meets the reef. Return to Cairns for overnight stay.",
        "Day 05: Kuranda Scenic Railway & Skyrail - Breakfast at the hotel. Travel on the Kuranda Scenic Railway through the rainforest. Explore Kuranda Village and return via the Skyrail Rainforest Cableway. Overnight stay in Cairns.",
        "Day 06: Whitsundays & Whitehaven Beach - Breakfast at the hotel. Fly to Airlie Beach and check into the hotel. Full-day Whitsundays cruise, including Whitehaven Beach. Return to Airlie Beach for overnight stay.",
        "Day 07: Departure - Breakfast at the hotel. Free time for shopping or relaxation. Transfer to the airport for departure."
    },
    WhatToDo = new List<string>
    {
        "Explore South Bank Parklands",
        "Visit Lone Pine Koala Sanctuary",
        "Travel to Gold Coast",
        "Enjoy Surfers Paradise",
        "Experience Movie World or Dreamworld",
        "Fly to Cairns",
        "Take Great Barrier Reef cruise",
        "Explore Daintree Rainforest",
        "Visit Mossman Gorge",
        "Visit Cape Tribulation",
        "Travel on Kuranda Scenic Railway",
        "Explore Kuranda Village",
        "Return via Skyrail Rainforest Cableway",
        "Fly to Airlie Beach",
        "Take Whitsundays cruise",
        "Visit Whitehaven Beach"
    },
    Places = new List<string>
    {
        "Brisbane",
        "South Bank Parklands",
        "Lone Pine Koala Sanctuary",
        "Gold Coast",
        "Surfers Paradise",
        "Movie World",
        "Dreamworld",
        "Cairns",
        "Great Barrier Reef",
        "Daintree Rainforest",
        "Mossman Gorge",
        "Cape Tribulation",
        "Kuranda",
        "Airlie Beach",
        "Whitsundays",
        "Whitehaven Beach"
    },
    Price = 1900,
    Inclusions = new List<string>
    {
        "Accommodation for 7 nights in 4-star hotels",
        "Daily breakfast",
        "Airport transfers",
        "Guided tours of all mentioned attractions",
        "Comfortable transportation for all intercity travel"
    },
    Exclusions = new List<string>
    {
        "International & domestic airfare",
        "Personal expenses such as shopping, tips, and meals not mentioned",
        "Travel insurance",
        "Additional activities not mentioned in the itinerary",
        "Entry tickets for optional attractions"
    }
},

            new Destination
{
    Name = "Tasmania",
    ImageUrls = new List<string>
    {
        "/assets/img/destination/Tasmania.webp",
        "/assets/img/destination/Tasmania1.webp",
        "/assets/img/destination/Tasmania2.webp",
        "/assets/img/destination/Tasmania3.webp",
        "/assets/img/destination/Tasmania4.webp"
    },
    Description = "Experience the breathtaking natural beauty and rich history of Tasmania, Australia's island state. From the stunning landscapes of Cradle Mountain to the charming city of Hobart, this tour offers a perfect blend of adventure, culture, and relaxation. Discover world-class food and wine, explore pristine national parks, and witness unique wildlife encounters in this unforgettable journey.",
    Itinerary = new List<string>
    {
        "Day 01: Arrival in Hobart & City Exploration. Arrive in Hobart and transfer to the hotel. Visit Salamanca Market (Saturday only) and Battery Point. Explore the famous MONA (Museum of Old and New Art). Enjoy dinner at Hobart's waterfront. Overnight stay in Hobart.",
        "Day 02: Port Arthur & Tasman Peninsula. Breakfast at the hotel. Travel to the UNESCO-listed Port Arthur Historic Site. Explore the Tasman Peninsula, including Remarkable Cave and Devil's Kitchen. Return to Hobart for overnight stay.",
        "Day 03: Bruny Island Adventure. Breakfast at the hotel. Full-day tour of Bruny Island. Visit The Neck Lookout, South Bruny National Park, and sample local cheeses and oysters. Return to Hobart for overnight stay.",
        "Day 04: Freycinet National Park & Wineglass Bay. Breakfast at the hotel. Travel to Freycinet National Park. Hike to Wineglass Bay Lookout and explore the pristine beaches. Overnight stay in Freycinet or return to Hobart.",
        "Day 05: Cradle Mountain & Wildlife Encounters. Breakfast at the hotel. Travel to Cradle Mountain-Lake St Clair National Park. Explore scenic trails and spot Tasmanian wildlife like wombats and echidnas. Return to Launceston for overnight stay.",
        "Day 06: Bay of Fires & Departure. Breakfast at the hotel. Visit the stunning Bay of Fires for its iconic red-hued rocks and white sandy beaches. Transfer to the airport for departure."
    },
    WhatToDo = new List<string>
    {
        "Visit Salamanca Market",
        "Explore Battery Point",
        "Explore MONA",
        "Travel to Port Arthur Historic Site",
        "Explore Tasman Peninsula",
        "Visit Remarkable Cave",
        "Visit Devil's Kitchen",
        "Tour Bruny Island",
        "Visit The Neck Lookout",
        "Sample local cheeses and oysters",
        "Travel to Freycinet National Park",
        "Hike to Wineglass Bay Lookout",
        "Explore Cradle Mountain-Lake St Clair National Park",
        "Spot Tasmanian wildlife",
        "Visit Bay of Fires"
    },
    Places = new List<string>
    {
        "Hobart",
        "Salamanca Market",
        "Battery Point",
        "MONA",
        "Port Arthur Historic Site",
        "Tasman Peninsula",
        "Remarkable Cave",
        "Devil's Kitchen",
        "Bruny Island",
        "The Neck Lookout",
        "Freycinet National Park",
        "Wineglass Bay",
        "Cradle Mountain-Lake St Clair National Park",
        "Bay of Fires"
    },
    Price = 1800,
    Inclusions = new List<string>
    {
        "Accommodation for 6 nights in 4-star hotels",
        "Daily breakfast",
        "Airport transfers",
        "Guided tours of all mentioned attractions",
        "Comfortable transportation for all intercity travel"
    },
    Exclusions = new List<string>
    {
        "International & domestic airfare",
        "Personal expenses such as shopping, tips, and meals not mentioned",
        "Travel insurance",
        "Additional activities not mentioned in the itinerary",
        "Entry tickets for optional attractions"
    }
},

               new Destination
{
    Name = "Ujjain Mahakaleshwar",
    ImageUrls = new List<string>
    {
        "/assets/img/destination/ujjain_mahakaleshwar.webp",
        "/assets/img/destination/ujjainmandir.webp",
        "/assets/img/destination/ujjain_mahakaleshwar2.webp",
        "/assets/img/destination/ujjain_mahakaleshwar3.webp",
        "/assets/img/destination/ujjain_mahakaleshwar4.webp"
    },
    Description = "Exploring Ujjain Mahakaleshwar Tour Packages and its revered Mahakaleshwar Temple promises a spiritually enriching vacation. Here's a suggested itinerary to make the most of your trip. Arrive in Ujjain, a city known for its religious significance and historical charm.",
    Itinerary = new List<string>
    {
        "Day 1: Arrival at Indore/Ujjain and Ujjain Sightseeing. Get picked up by our representative from Indore/Ujjain Airport/Railway station and get transferred to Ujjain hotel. Check-in to the hotel freshen up and move Ujain sightseeing which included place like Mahakaleshwar Dham, which is among the 12 Jyotirlingas and is dedicated to Lord Shiva. The presiding deity is situated here in the form of lingam and it is believed to be Swayambhu. This temple is located on the bank of the River Kshipra. It comprises the idols of several gods including Lord Ganesha, Lord Katikeya, and Goddess Parvati. Seek blessings from Lord Shiva and other deities. Offer prayers and enjoy darshan at the temple. In the evening enjoy the lighting in the newly formed corridor. Overnight stay at Ujjain.",
        "Day 2: Indore Sightseeing. Have your breakfast at the hotel and move for Indore sightseeing which includes places like Bada Ganpati, Khajrana Ganesh, Rajwada, Lal Bag Palace, 56 street food market. In the evening return back to the hotel. Overnight stay at Indore.",
        "Day 3: Departure. Have your breakfast at the hotel and check out from the hotel. You can keep your luggage in the hotel cloakroom. The remaining day is free for leisure. We will drop you at the airport or Railway station as per your flight or Train timing."
    },
    WhatToDo = new List<string>
    {
        "Participate in the Bhasma Aarti",
        "Explore the temple complex",
        "Marvel at the architectural grandeur",
        "Take a day trip to Omkareshwar",
        "Visit the Omkareshwar Temple",
        "Seek blessings from Lord Shiva",
        "Offer prayers and enjoy darshan at the temple",
        "Enjoy the lighting in the newly formed corridor",
        "Visit Bada Ganpati",
        "Visit Khajrana Ganesh",
        "Visit Rajwada",
        "Visit Lal Bag Palace",
        "Explore 56 street food market"
    },
    Places = new List<string>
    {
        "Ujjain",
        "Mahakaleshwar Dham",
        "River Kshipra",
        "Omkareshwar",
        "Narmada River",
        "Indore",
        "Bada Ganpati",
        "Khajrana Ganesh",
        "Rajwada",
        "Lal Bag Palace",
        "56 street food market"
    },
    Price = 1400,
    Inclusions = new List<string>
    {
        "Accommodation for 3 nights in 4-star hotels",
        "Daily breakfast",
        "Airport transfers",
        "Guided tours of all mentioned attractions",
        "Comfortable transportation for all intercity travel"
    },
    Exclusions = new List<string>
    {
        "International & domestic airfare",
        "Personal expenses such as shopping, tips, and meals not mentioned",
        "Travel insurance",
        "Additional activities not mentioned in the itinerary",
        "Entry tickets for optional attractions"
    }
},

              new Destination
{
    Name = "Ayodhya Ram Mandir",
    ImageUrls = new List<string>
    {
        "/assets/img/destination/ayodhya_ram_mandir.webp",
        "/assets/img/destination/ayodhya_ram_mandir1.webp",
        "/assets/img/destination/ayodhya_ram_mandir2.webp",
        "/assets/img/destination/ayodhya_ram_mandir3.webp",
        "/assets/img/destination/ayodhya_ram_mandir4.webp"
    },
    Description = "Immerse yourself in Ayodhya Ram Mandir Tour Vacation rich cultural heritage by attending local events, festivals, and cultural performances showcasing the city's vibrant traditions and folklore.",
    Itinerary = new List<string>
    {
        "Day 1: Arrival at Varanasi Day Free for Leisure and Ganga Aarti...",
        "Day 2: Full Day Local Varanasi Sight Seen...",
        "Day 3: Varanasi to Prayagraj via Vindhyachal Mirzapur...",
        "Day 4: Prayagraj to Ayodhya Ram Lala Temple...",
        "Day 5: Transfer From Ayodhya to Lucknow...",
        "Day 6: Day Free for Leisure Shopping and Departure..."
    },
    WhatToDo = new List<string>
    {
        "Spend ample time exploring the temple complex",
        "Marvel at the architecture",
        "Soak in the spiritual atmosphere",
        "Engage with locals to learn more about their traditions and way of life",
        "Take one last stroll along the banks of the Sarayu River"
    },
    Places = new List<string>
    {
        "Ayodhya",
        "Ram Mandir",
        "Varanasi",
        "Tulsi Manas Mandir",
        "Durga Mandir",
        "Sankat Mochan Temple",
        "New Vishwanath Temple",
        "Sarnath",
        "Dhamek Stupa",
        "Chaukhandi Stupa",
        "Jain Temple",
        "Buddha Museum",
        "Ashok Pillar",
        "Deer Park",
        "Vindhyachal",
        "Vindhyachal Temple",
        "Aastha Bhuja Temple",
        "Kaalikoh Kali Mata Temple",
        "Triveni Sangam",
        "Allahabad Fort",
        "Anand Bhawan",
        "Ayodhya Ram Lala Temple",
        "Lord Rama Temple",
        "Janki Bhawan",
        "Kanak Bhawan",
        "Hanuman Gari",
        "Mani Parbat",
        "Sugriv Parbat",
        "Treta Ke Thakur",
        "Nageswarnath Temple",
        "Lucknow"
    },
    Price = 1500,
    Inclusions = new List<string>
    {
        "Accommodation for 6 nights in 4-star hotels",
        "Daily breakfast",
        "Airport transfers",
        "Guided tours of all mentioned attractions",
        "Comfortable transportation for all intercity travel"
    },
    Exclusions = new List<string>
    {
        "International & domestic airfare",
        "Personal expenses such as shopping, tips, and meals not mentioned",
        "Travel insurance",
        "Additional activities not mentioned in the itinerary",
        "Entry tickets for optional attractions"
    }
},

                new Destination
{
    Name = "Kerala",
    ImageUrls = new List<string>
    {
        "/assets/img/destination/Kerala.webp",
        "/assets/img/destination/Kerala1.webp",
        "/assets/img/destination/Kerala2.webp",
        "/assets/img/destination/Kerala3.webp",
        "/assets/img/destination/Kerala4.webp"
    },
    Description = "Kerala backwaters Tour Package offers a serene and rejuvenating experience amidst the lush greenery, tranquil waterways, and unique cultural heritage of India's southern state. Embark on a houseboat cruise through the scenic backwaters of Alleppey, where you can relax and unwind while drifting past coconut groves, paddy fields, and picturesque villages.",
    Itinerary = new List<string>
    {
        "Day 01: Arrive Cochin, Drive to Munnar. You will be welcomed and joined by our executive upon your arrival at the Cochin airport/ Railway station. You will then head on to Munnar which is approximately a 4 hour drive from Cochin. The winding roads up the hill, the changing scenery, the air gradually turning crisp and the slow drop in temperature should let you know that the beautiful experience you were looking for has just begun. Arrive at Munnar and check into the resort. Later in the evening, visit Tea Museum. Learn how tea is processed from raw fresh tea leaves to its final packaging. You can also visit Munnar town and shop for spices and handmade chocolates. Overnight stay at Munnar.",
        "Day 02: Munnar Day Tour. After breakfast we take you on a full day sightseeing trip of Munnar, the tea town of Kerala. The sprawling tea gardens are sure to take your breath away. We visit the Eravikulam National Park to get a glimpse of the Nilagiri Thar. The soft trek up the hill in the middle of the park will be worth your while. We then proceed to visit Mattuppetty Dam, Eco Point, Kundala Lake, Blossom Garden, Pothenmedu View point and Top station. Overnight at the resort.",
        "Day 03: Munnar to Thekkady. On the next morning, we drive to Thekkady covering a distance of 110kms / 4 hrs amidst the cardamom hills flanking the road with the occasional waterfalls. After checking into the resort at Thekkady and having lunch, you can proceed to take a boat ride on the lake and the Periyar dam. During the boat ride that will take approximate 2 hours you can get rare glimpses of wild animals such as elephants, deer and rare birds. The forest department also offers many programs including Periyar tiger trail, nature walk, bamboo rafting, etc. which you can take part in. Overnight stay at Thekkady resort.",
        "Day 04: Thekkady to Alleppey. After breakfast we set out on an early start to Alleppey. The drive is approximately 5 hour long, covering a distance of around 160kms. The Venice of the East will welcome you with its serene backwaters and mouth watering cuisine. You can board your houseboat by around 1pm. Sit back and relax while the boat cruises through the narrow canals of the Vembanad lake. Explore the scenic backwaters while passing through lagoons, canals, lakes, rivers and inlets. See the traditional villages and coconut groves from the deck of the boat. After 6pm the houseboat will drop anchor for the day. Enjoy your stay on the houseboat with twinkling stars above and the tranquil waters beneath.",
        "Day 05: Departure from Cochin. The houseboat will start cruising again at 8 am and drop you at Alleppey jetty at 9 am. You will then proceed to Cochin. You will say good bye to Kerala, but we promise, you will do so with a heavy heart and with a promise to return back soon."
    },
    WhatToDo = new List<string>
    {
        "Embark on a houseboat cruise through the scenic backwaters of Alleppey",
        "Relax and unwind while drifting past coconut groves, paddy fields, and picturesque villages",
        "Make a stop at Kochi, a historic port city known for its rich cultural heritage, colonial architecture, and vibrant blend of influences from around the world",
        "Conclude your Kerala Backwaters Tour Package in Thiruvananthapuram, the capital city of Kerala, known for its rich cultural heritage and stunning beaches",
        "Visit Tea Museum and learn how tea is processed",
        "Explore Munnar town and shop for spices and handmade chocolates",
        "Visit Eravikulam National Park to get a glimpse of the Nilagiri Thar",
        "Take a boat ride on the lake and the Periyar dam",
        "Participate in programs offered by the forest department such as Periyar tiger trail, nature walk, bamboo rafting, etc."
    },
    Places = new List<string>
    {
        "Kerala",
        "Alleppey (Alappuzha)",
        "Cochin",
        "Munnar",
        "Thekkady",
        "Thiruvananthapuram",
        "Eravikulam National Park",
        "Mattuppetty Dam",
        "Eco Point",
        "Kundala Lake",
        "Blossom Garden",
        "Pothenmedu View point",
        "Top station",
        "Periyar dam",
        "Vembanad lake"
    },
    Price = 1600,
    Inclusions = new List<string>
    {
        "Accommodation for 5 nights in 4-star hotels",
        "Daily breakfast",
        "Airport transfers",
        "Guided tours of all mentioned attractions",
        "Comfortable transportation for all intercity travel"
    },
    Exclusions = new List<string>
    {
        "International & domestic airfare",
        "Personal expenses such as shopping, tips, and meals not mentioned",
        "Travel insurance",
        "Additional activities not mentioned in the itinerary",
        "Entry tickets for optional attractions"
    }
},
new Destination
{
    Name = "Golden Temple",
    ImageUrls = new List<string>
    {
        "/assets/img/destination/golden_temple.webp",
        "/assets/img/destination/golden_temple1.webp",
        "/assets/img/destination/golden_temple2.webp",
        "/assets/img/destination/golden_temple3.webp",
        "/assets/img/destination/golden_temple4.webp"
    },
    Description = "The Golden Temple Tour Package, also known as Sri Harmandir Sahib, is not only a central religious site for Sikhs but also a marvel of architectural beauty and spiritual significance. Golden Temple Tour Package itself. Marvel at its golden domes, serene sarovar (holy tank), and the beautiful architecture. The community kitchen of the Golden Temple Tour Package, where volunteers serve free meals to all visitors regardless of their religion, caste, or creed.",
    Itinerary = new List<string>
    {
        "Day 1: Arrive in Amritsar and visit the Golden Temple, known for its stunning architecture and the serene Sarovar. Experience the evening Palki Sahib ceremony with soulful hymns. Sample local delicacies like Amritsari kulcha and lassi nearby.",
        "Day 2: Explore the poignant Jallianwala Bagh memorial and its historical significance. Shop for Punjabi attire and souvenirs at Hall Bazaar. Witness the patriotic Beating Retreat Ceremony at the Wagah Border in the evening.",
        "Day 3: Visit the Durgiana Temple and its beautiful architecture. Explore the Partition Museum to learn about India's partition history. Discover the life of Maharaja Ranjit Singh at his namesake museum. Relax at Ram Bagh Gardens.",
        "Day 4: Enjoy a leisurely morning in Amritsar. Optionally visit local markets or attractions. Depart with cherished memories of Amritsar's cultural richness and hospitality."
    },
    WhatToDo = new List<string>
    {
        "Marvel at the golden domes and serene sarovar of the Golden Temple",
        "Experience the evening Palki Sahib ceremony with soulful hymns",
        "Visit Jallianwala Bagh memorial and learn about its historical significance",
        "Shop for Punjabi attire and souvenirs at Hall Bazaar",
        "Witness the Beating Retreat Ceremony at the Wagah Border",
        "Visit Durgiana Temple and its beautiful architecture",
        "Explore the Partition Museum to learn about India's partition history",
        "Discover the life of Maharaja Ranjit Singh at his namesake museum",
        "Relax at Ram Bagh Gardens"
    },
    Places = new List<string>
    {
        "Amritsar",
        "Golden Temple (Sri Harmandir Sahib)",
        "Jallianwala Bagh",
        "Hall Bazaar",
        "Wagah Border",
        "Durgiana Temple",
        "Partition Museum",
        "Maharaja Ranjit Singh Museum",
        "Ram Bagh Gardens"
    },
    Price = 1500,
    Inclusions = new List<string>
    {
        "Accommodation for 4 nights in 4-star hotels",
        "Daily breakfast",
        "Airport transfers",
        "Guided tours of all mentioned attractions",
        "Comfortable transportation for all intercity travel"
    },
    Exclusions = new List<string>
    {
        "International & domestic airfare",
        "Personal expenses such as shopping, tips, and meals not mentioned",
        "Travel insurance",
        "Additional activities not mentioned in the itinerary",
        "Entry tickets for optional attractions"
    }
},

                new Destination
{
    Name = "Italy",
    ImageUrls = new List<string>
    {
        "/assets/img/destination/Italy.webp",
        "/assets/img/destination/Italy1.webp",
        "/assets/img/destination/Italy2.webp",
        "/assets/img/destination/Italy3.webp",
        "/assets/img/destination/Italy4.webp"
    },
    Description = "Italy Tour Packages with its rich history, vibrant culture, delectable cuisine, and stunning landscapes, offers a Italy Tour Packages experience like no other. Whether you're drawn to the ancient ruins of Rome, the Renaissance art of Florence, the romantic canals of Venice, or the sun-soaked beaches of the Amalfi Coast, Italy has something to enchant every traveler. Here's a customizable itinerary to help you plan your dream Italy Tour Packages.",
    Itinerary = new List<string>
    {
        "Day 1: Arrival in Rome. Upon arrival in Rome, check into your hotel. In the afternoon, explore the Colosseum and Roman Forum to immerse yourself in ancient Roman history. Later, visit the Trevi Fountain to toss a coin and make a wish, then enjoy dinner at a local trattoria.",
        "Day 2: Rome. Start your day by exploring Vatican City, including St. Peter's Basilica and the Vatican Museums where you can marvel at the Sistine Chapel. Afterward, visit the Pantheon to admire its impressive dome and explore the lively atmosphere of Piazza Navona. In the evening, take a leisurely stroll through Rome's historic center.",
        "Day 3: Florence. Travel to Florence in the morning (approximately 1.5-2 hours by train). Upon arrival, visit the Uffizi Gallery to see Renaissance masterpieces by Botticelli, Michelangelo, and Leonardo da Vinci. Spend your evening walking across the iconic Ponte Vecchio, known for its jewelry shops and stunning views.",
        "Day 4: Florence. Begin your day by climbing to the top of the Florence Cathedral's Duomo for breathtaking views of the city. In the afternoon, explore the Accademia Gallery to see Michelangelo's David and other significant artworks. Finish your day at the historic Piazza della Signoria, home to impressive sculptures and the Palazzo Vecchio.",
        "Day 5: Venice. Take a morning train to Venice (approximately 2 hours by train). Upon arrival, explore St. Mark's Basilica and Square, then enjoy a scenic vaporetto ride along the Grand Canal. In the afternoon, visit the Doge's Palace and cross the iconic Bridge of Sighs. In the evening, enjoy a delightful dinner overlooking the Venetian canals.",
        "Day 6: Venice. Start your day by visiting the Rialto Bridge and exploring its bustling surroundings. Then, take a boat trip to Murano for glassblowing demonstrations and visit Burano to admire its colorful houses. Return to Venice in the evening for a final dinner with views of the Grand Canal.",
        "Day 7: Departure. Depending on your departure time, spend your last morning in Venice exploring more of its charming streets or indulging in some last-minute shopping. Depart Venice, taking with you unforgettable memories of Italy's rich cultural heritage and stunning landscapes."
    },
    WhatToDo = new List<string>
    {
        "Explore the Colosseum and Roman Forum",
        "Visit the Trevi Fountain",
        "Explore Vatican City, including St. Peter's Basilica and the Vatican Museums",
        "Visit the Pantheon",
        "Explore the Uffizi Gallery",
        "Walk across the Ponte Vecchio",
        "Climb to the top of the Florence Cathedral's Duomo",
        "Explore the Accademia Gallery",
        "Visit St. Mark's Basilica and Square",
        "Enjoy a vaporetto ride along the Grand Canal",
        "Visit the Doge's Palace and the Bridge of Sighs",
        "Visit the Rialto Bridge",
        "Take a boat trip to Murano for glassblowing demonstrations",
        "Visit Burano to admire its colorful houses"
    },
    Places = new List<string>
    {
        "Rome",
        "Florence",
        "Venice",
        "Vatican City",
        "Colosseum",
        "Roman Forum",
        "Trevi Fountain",
        "St. Peter's Basilica",
        "Vatican Museums",
        "Pantheon",
        "Piazza Navona",
        "Uffizi Gallery",
        "Ponte Vecchio",
        "Florence Cathedral's Duomo",
        "Accademia Gallery",
        "Piazza della Signoria",
        "Palazzo Vecchio",
        "St. Mark's Basilica",
        "St. Mark's Square",
        "Grand Canal",
        "Doge's Palace",
        "Bridge of Sighs",
        "Rialto Bridge",
        "Murano",
        "Burano"
    },
    Price = 1900,
    Inclusions = new List<string>
    {
        "Accommodation for 7 nights in 4-star hotels",
        "Daily breakfast",
        "Airport transfers",
        "Guided tours of all mentioned attractions",
        "Comfortable transportation for all intercity travel"
    },
    Exclusions = new List<string>
    {
        "International & domestic airfare",
        "Personal expenses such as shopping, tips, and meals not mentioned",
        "Travel insurance",
        "Additional activities not mentioned in the itinerary",
        "Entry tickets for optional attractions"
    }
},
new Destination
{
    Name = "Paris",
    ImageUrls = new List<string>
    {
        "/assets/img/destination/Paris.webp",
        "/assets/img/destination/Paris1.webp",
        "/assets/img/destination/Paris2.webp",
        "/assets/img/destination/Paris3.webp",
        "/assets/img/destination/Paris4.webp"
    },
    Description = "Paris Tour Package, the City of Lights, is a dream destination for many travelers. With its world-famous landmarks, romantic atmosphere, and rich history, Paris has something to offer everyone. Here's a suggested itinerary for a 3-day trip to Paris.",
    Itinerary = new List<string>
    {
        "Day 1: Arrival in Paris. Upon arrival in Paris, check into your hotel and settle in. In the afternoon, start your exploration by visiting the iconic Eiffel Tower. Admire its beauty from below and consider ascending to the top for panoramic views of the city. In the evening, enjoy a delightful dinner at a local bistro to experience Parisian cuisine.",
        "Day 2: Louvre Museum and Seine River Cruise. Begin your day with a visit to the Louvre Museum, home to thousands of artworks including the Mona Lisa and Venus de Milo. Spend the morning exploring this vast museum. In the afternoon, take a relaxing boat cruise along the Seine River. Admire Paris landmarks such as Notre-Dame Cathedral, Musée d'Orsay, and Pont Neuf from a different perspective. After the cruise, enjoy dinner at a riverside restaurant.",
        "Day 3: Montmartre and Champs-Élysées. Head to Montmartre in the morning, known for its artistic history and charming streets. Visit the Sacré-Cœur Basilica and enjoy stunning views of Paris from the hilltop. Explore Montmartre, including the bustling Place du Tertre. In the afternoon, stroll down the famous Champs-Élysées. Shop at luxury boutiques and visit the Arc de Triomphe for panoramic views. Dinner can be enjoyed at a cozy restaurant along the avenue.",
        "Day 4: Latin Quarter and Île de la Cité. Explore the historic Latin Quarter in the morning, visiting landmarks like the Panthéon and enjoying the vibrant atmosphere. Wander through its streets filled with bookshops and cafés. In the afternoon, visit the Sainte-Chapelle to admire its stunning stained glass windows. Explore Île de la Cité, where you can visit Notre-Dame Cathedral and enjoy a peaceful walk along the Seine. Dinner can be at a traditional brasserie.",
        "Day 5: Musée d'Orsay and Versailles Palace. Start your day with a visit to the Musée D'Orsay, housed in a former railway station and known for its collection of impressionist and post-impressionist masterpieces. Spend the morning exploring the museum. In the afternoon, take a day trip to the Palace of Versailles. Explore the opulent rooms, Hall of Mirrors, and expansive gardens. Return to Paris in the evening and enjoy a farewell dinner at a gourmet restaurant.",
        "Day 6: Departure from Paris. Depending on your departure time, spend your morning exploring more of Paris, shopping, or visiting a favorite spot. Check out from your hotel and transfer to the airport or train station for your return journey home."
    },
    WhatToDo = new List<string>
    {
        "Visit the Eiffel Tower and enjoy panoramic views",
        "Explore the Louvre Museum and admire famous artworks",
        "Take a boat cruise along the Seine River",
        "Visit Montmartre and the Sacré-Cœur Basilica",
        "Stroll down the Champs-Élysées and visit the Arc de Triomphe",
        "Explore the Latin Quarter and Île de la Cité",
        "Visit the Sainte-Chapelle and Notre-Dame Cathedral",
        "Explore the Musée d'Orsay and its collection of masterpieces",
        "Take a day trip to the Palace of Versailles"
    },
    Places = new List<string>
    {
        "Paris",
        "Eiffel Tower",
        "Louvre Museum",
        "Seine River",
        "Montmartre",
        "Sacré-Cœur Basilica",
        "Champs-Élysées",
        "Arc de Triomphe",
        "Latin Quarter",
        "Île de la Cité",
        "Sainte-Chapelle",
        "Notre-Dame Cathedral",
        "Musée d'Orsay",
        "Palace of Versailles"
    },
    Price = 1800,
    Inclusions = new List<string>
    {
        "Accommodation for 6 nights in 4-star hotels",
        "Daily breakfast",
        "Airport transfers",
        "Guided tours of all mentioned attractions",
        "Comfortable transportation for all intercity travel"
    },
    Exclusions = new List<string>
    {
        "International & domestic airfare",
        "Personal expenses such as shopping, tips, and meals not mentioned",
        "Travel insurance",
        "Additional activities not mentioned in the itinerary",
        "Entry tickets for optional attractions"
    }
},

              new Destination
{
    Name = "London",
    ImageUrls = new List<string>
    {
        "/assets/img/destination/London.webp",
        "/assets/img/destination/London1.webp",
        "/assets/img/destination/London2.webp",
        "/assets/img/destination/London3.webp",
        "/assets/img/destination/London4.webp"
    },
    Description = "London Tour Packages, a city steeped in history, culture, and innovation, offers a plethora of experiences for travelers of all tastes. Walk in the footsteps of royalty as you explore the majestic palaces and learn about the city's intriguing past from knowledgeable guides.",
    Itinerary = new List<string>
    {
        "Day 1 : LONDON. Arrive at the airport and transfer to the hotel. Rest at leisure and overnight at the hotel.",
        "Day 2 : LONDON. In the morning we include a panoramic tour of London: Westminster, the Parliament, the City, the Thames. Free time in the afternoon. Optionally, we offer you the possibility to visit the valley of the River Thames, together with the village of Windsor and its imposing castle. In the evening, we'll meet at a meeting point in Piccadilly for a walk through Leicester Square, China Town and Soho, with its atmosphere, theatres and entertainment (this transfer can also be made the day before after arrival in London). Return to the hotel after that by coach.",
        "Day 3 : LONDON - CAMBRIDGE - YORK - DURHAM. We start the day leaving London towards northern England. Our first stop is CAMBRIDGE to admire its beautiful residential colleges. After lunch we will continue to YORK where its cathedral, Roman walls and lively shopping streets will create a charming memory, some free time to stroll around. Afterwards, we will continue north to DURHAM. Explore this charming medieval town full of life which has a wonderful cathedral right in front of the castle.",
        "Day 4 : DURHAM - ALNWICK - EDINBURGH. Today we will travel to Scotland, stopping in ALNWICK, a picturesque town with lovely gardens and a large medieval castle which was used in the Harry Potter films. We will continue to EDINBURGH, arriving in the afternoon. This is Scotland's capital city and one of the most active cities in Northern Europe. Its monumental center has been declared a UNESCO World Heritage Site. Its castle overlooks the city's steep streets and parks. We will enjoy a sightseeing tour with a local guide.",
        "Day 5 : EDINBURGH - INVERNESS - LOCH NESS - FORT AUGUSTUS - GLASGOW. We continue to enjoy the stunning landscapes of the Scottish Highlands with a coffee and stroll in PITLOCHRY, a charming little village. We will continue and pass by Inverness in the north of Scotland, a city located very near the mysterious LOCH NESS. After lunch we visit (entrance included) the URQUHART medieval castle, from here we will take a boat trip on the dark waters of the lake. Afterwards, we will pass through FORT AUGUST with its sluice gate system and FORT WILLIAM, the tourist center at the foot of Ben Nevis (the highest summit in the United Kingdom). We will return south through the high plateaus where it is possible to glimpse the snow before visiting Loch Lomond, one of the most popular lakes in Scotland. We arrive in GLASGOW at the end of the day. Short stroll around the city center and accommodation. VERY IMPORTANT NOTE: Groups arriving in Edinburgh from November 15 until the end of February, due to the short duration of the days and frequent snow problems on the roads, will have the following stage: Departure from Edinburgh to PITLOCHRY, a picturesque city in the center of the Scottish region, where the majority of whiskey is produced. We will visit a traditional whiskey distillery in the area (entrance included). We continue towards STIRLING, with an impressive historic center dominated by the great castle, one of the most important in Scotland having witnessed numerous historical events, including not only historical battles, but also the coronation of several of its kings. Reservation and tickets included. Time for lunch. After that we will make a stop to photograph The Kelpies, enormous sculptures of two horses with water heads from Scottish mythology. We continue to GLASGOW, time to walk through the city center and accommodation.",
        "Day 6 : GLASGOW. After breakfast, end of our services."
    },
    WhatToDo = new List<string>
    {
        "Explore the majestic palaces and learn about the city's intriguing past",
        "Embark on a gastronomic journey through the London scene",
        "Sample traditional British fare at historic pubs",
        "Indulge in gourmet delights at Michelin-starred restaurants",
        "Explore bustling food markets brimming with international flavors",
        "Visit Windsor Castle, the oldest and largest inhabited castle in the world",
        "Witness the Changing of the Guard ceremony at Buckingham Palace",
        "Enjoy shopping experiences from luxury boutiques in Mayfair to quirky markets in Notting Hill"
    },
    Places = new List<string>
    {
        "London",
        "London Bridge",
        "York",
        "London Eye",
        "Cambridge",
        "Edinburgh",
        "Borough Market",
        "Inverness",
        "Glasgow",
        "Birmingham",
        "Lochness",
        "Westminster",
        "Parliament",
        "Thames",
        "Windsor",
        "Piccadilly",
        "Leicester Square",
        "China Town",
        "Soho",
        "Durham",
        "Alnwick",
        "Pitlochry",
        "Urquhart Castle",
        "Fort Augustus",
        "Fort William",
        "Loch Lomond",
        "Stirling",
        "The Kelpies"
    },
    Price = 1700,
    Inclusions = new List<string>
    {
        "Accommodation for 6 nights in 4-star hotels",
        "Daily breakfast",
        "Airport transfers",
        "Guided tours of all mentioned attractions",
        "Comfortable transportation for all intercity travel"
    },
    Exclusions = new List<string>
    {
        "International & domestic airfare",
        "Personal expenses such as shopping, tips, and meals not mentioned",
        "Travel insurance",
        "Additional activities not mentioned in the itinerary",
        "Entry tickets for optional attractions"
    }
},

              new Destination
{
    Name = "Switzerland",
    ImageUrls = new List<string>
    {
        "/assets/img/destination/switzerland.webp",
        "/assets/img/destination/switzerland1.webp",
        "/assets/img/destination/switzerland2.webp",
        "/assets/img/destination/switzerland3.webp",
        "/assets/img/destination/switzerland4.webp"
    },
    Description = "Nestled in the heart of Europe, Switzerland Tour Package is a picturesque paradise renowned for its stunning landscapes, charming cities, and vibrant culture. From snow-capped mountains to serene lakes and quaint villages, this alpine wonderland offers a myriad of experiences for travelers seeking adventure, relaxation, and everything in between.",
    Itinerary = new List<string>
    {
        "Day 1 : ZURICH. Upon arriving at the airport, we will be waiting to transfer you to your hotel. Check the information boards in the hotel reception area for details of the welcome meeting with your guide and fellow travelers.",
        "Day 2 : ZURICH - BERNE - FRIBOURG - NYON - YVOIRE - GENEVA. We will set off for the capital of the country, BERN, one of the most beautiful historic cities in Switzerland. We will go to the Garden of Roses whose viewpoint offers unrivaled views of the city. Free time for a walk and lunch. In the afternoon we will get back on the road and head to the French-speaking region of Switzerland, making a stop in FRIBOURG, a beautiful bilingual city. After that we will travel towards Lake Geneva and make a stop in NYON, a little Roman city. From there we will travel to France by boat. We will arrive in YVOIRE, a beautiful medieval village with fortified gates, a castle, flowery streets and beautiful views over the lake. GENEVA arrival at the end of the day. Accommodation (usually our hotel is located in the French area of the town).",
        "Day 3 : GENEVA - CHILLON CASTLE - GRUYERES - LEYSIN. After breakfast we include a city tour in Geneva, a city by Lake Leman that houses the European headquarters of the International Labor Organization, the Red Cross and numerous other international entities. We will go to the Palais des Nations (headquarters of the United Nations in Europe), the beautiful English Garden with its flower clock and admire the “Jet d'Eau”, the highest fountain in Europe. Through vineyard landscapes we go on to CHILLON CASTLE, built in the waters of the lake. Entrance included to this fantastic medieval castle. After this, we will continue to the picturesque walled village of GRUYERES, known worldwide for its cheese. Nearby this area there are several of the main chocolate Swiss factories (NESTLE amongst them), and we visit the Cailler chocolate factory, with a tasting included! We continue along small roads between bucolic landscapes of mountains and typical wooden villages. Arrival in the evening to LEYSIN, a beautiful holiday town. Dinner included.",
        "Day 4 : LEYSIN - INTERLAKEN - AARESCHLUCHT - CHIASSO. Today's stage has beautiful high mountain scenery. Don’t forget your winter clothes, we climb to one of the most spectacular spots in Switzerland where you can see ice and snow all year round. We include the cable car ride up to GLACIER 3000, where you can walk on the suspended bridge in the gap between two mountain peaks, enter the Ice Cathedral or play in the Fun Park. Stop in INTERLAKEN and time for lunch. Afterwards we travel to the AARESCHLUCHT GORGES. We then take the Sustenpass, one of the most beautiful roads in Switzerland that runs between glaciers and takes us to Italian-speaking Switzerland. Overnight stay in CHIASSO, a Swiss town on the border with Italy.",
        "Day 5 : CHIASSO - LUGANO - BURGLEN - LUCERNE. We will visit LUGANO, the cosmopolitan capital of the Italian Switzerland with its beautiful lake. Time to stroll. We continue on the highway, crossing St. Gotthard Pass. We stop in BURGLEN, the tiny village where William Tell was born, to see the chapel of the 16th century with paintings illustrating his life. If you want, you can also visit the museum of William Tell. In the neighboring city of ALTDORF we can also find traces of his history. Continue to LUCERNE, free time to explore this beautiful city by the lake that bears its name. Optionally, take a cruise along the Lake of the Four Cantons, considered one of the most beautiful lakes in Switzerland.",
        "Day 6 : LUCERNE - EINSIEDELN - VADUZ - FELDKIRCH. We leave Lucerne driving along the Four Cantons Lake towards EINSIEDELN, its immense baroque abbey is the most important pilgrimage center in Switzerland. After that we continue towards the east of Switzerland. MAIENFELD is the village which inspired Heidi's story. In HEIDIDORF we will be visiting Heidi's House (ticket not included), a picturesque place with beautiful landscapes that we will reach through a pedestrian path. Then we enter into the independent country of Liechtenstein, and its capital city VADUZ with its impressive castle. Time to stroll and have lunch. Subsequently, we go to the neighbor city of FELDKIRCH, in Austria, located next to the borders of Switzerland, Liechtenstein and Germany: a charming walled city with a genuine historic center and a castle. Free time.",
        "Day 7 : FELDKIRCH - ST. GALLEN - CONSTANCE - STEIN AM RHEIN - RHINE FALLS - ZURICH. We start our day heading to ST. GALLEN, whose center is part of the World Heritage List by UNESCO with a wonderful cathedral and its historic center. Afterwards we will continue by Constance Lake, between Germany and Switzerland. We will have time for a walk at Constance’s center before visiting MAINAU Island (access by pedestrian bridge) with its stunning botanic garden (entry included). Then, again in Switzerland, we will take a stroll in STEIN AM RHEIN, a typical village on the Rhine River with precious houses with painted walls. We will also visit Rhine Waterfalls (entry included), the waterfalls with the most abundant flow in Europe. Arrival in ZURICH at the end of the day.",
        "Day 8 : ZURICH. After breakfast, end of our services."
    },
    WhatToDo = new List<string>
    {
        "Explore the picturesque towns surrounding Lake Geneva, Lake Lucerne, and Lake Zurich",
        "Go skydiving over the Swiss Alps for an exhilarating bird's-eye view of the majestic mountains",
        "Try canyoning, bungee jumping, or white-water rafting in Switzerland's pristine rivers and gorges",
        "Take a cable car ride up to GLACIER 3000 for panoramic views",
        "Walk on the suspended bridge between two mountain peaks",
        "Enter the Ice Cathedral or play in the Fun Park",
        "Visit the Palais des Nations (headquarters of the United Nations in Europe)",
        "Admire the Jet d'Eau, the highest fountain in Europe",
        "Explore the walled village of GRUYERES, known worldwide for its cheese",
        "Visit the Cailler chocolate factory with a tasting included",
        "Stroll in STEIN AM RHEIN, a village on the Rhine River with painted houses",
        "Visit Rhine Waterfalls, the most abundant waterfall in Europe"
    },
    Places = new List<string>
    {
        "Zurich",
        "Grindelwald",
        "Jungfrau",
        "Aletsch Glacier",
        "Lucerne",
        "Geneva",
        "Interlaken",
        "Bern",
        "Fribourg",
        "Nyon",
        "Yvoire",
        "Chillon Castle",
        "Gruyeres",
        "Leysin",
        "Glacier 3000",
        "Aareschlucht Gorges",
        "Chiasso",
        "Lugano",
        "Burglen",
        "Altdorf",
        "Einsiedeln",
        "Heididorf",
        "Vaduz",
        "Feldkirch",
        "St. Gallen",
        "Constance",
        "Mainau Island",
        "Stein am Rhein",
        "Rhine Falls"
    },
    Price = 1900,
    Inclusions = new List<string>
    {
        "Accommodation for 8 nights in 4-star hotels",
        "Daily breakfast",
        "Airport transfers",
        "Guided tours of all mentioned attractions",
        "Comfortable transportation for all intercity travel"
    },
    Exclusions = new List<string>
    {
        "International & domestic airfare",
        "Personal expenses such as shopping, tips, and meals not mentioned",
        "Travel insurance",
        "Additional activities not mentioned in the itinerary",
        "Entry tickets for optional attractions"
    }
},

                new Destination
{
    Name = "Bali",
    ImageUrls = new List<string>
    {
        "/assets/img/destination/Bali1.webp",
        "/assets/img/destination/Bali.webp",
        "/assets/img/destination/Bali2.webp",
        "/assets/img/destination/Bali3.webp",
        "/assets/img/destination/Bali4.webp"
    },
    Description = "Bali Tour Package is a tropical paradise renowned for its lush landscapes, vibrant culture, and warm hospitality.",
    Itinerary = new List<string>
    {
        "Day 1: Arrival in Bali. Welcome to Bali. Upon your arrival, meet our representative at the arrival hall who will escort you to your private car. Arrive at the hotel and check in. The day is free at leisure. Unwind at the hotel or relax at the nearest beach. In the evening, explore in-house hotel bars or nightlife spots such as Ku De Ta on the Seminyak coast or Rock Bar at Ayana Resort perched above the Indian Ocean. Overnight stay at the hotel.",
        "Day 2: Full Day Kintamani & Ubud Village. After breakfast, visit the famous Ubud Village and Mount Batur. Stop at Batubulan village for Barong and Keris performances. Explore the Balinese artisan villages of Celuk and Mas. Journey to Kintamani and Mount Batur to enjoy the views of the active volcano and Batur caldera lake. Enjoy Indian lunch. Overnight stay at the hotel.",
        "Day 3: Water Sports at Benoa Beach. Enjoy thrilling water sports at Tanjung Benoa Beach Resort, including banana boat rides, jet skiing, and parasailing. Overnight stay at the hotel.",
        "Day 4: Day at Leisure & Tanah Lot Tour. Visit Tanah Lot Temple, built on a rock in the ocean. Experience Balinese culture and admire flora & fauna. Overnight stay at the hotel.",
        "Day 5: Day at Leisure & Seminyak. Visit Seminyak for beach relaxation or a luxury spa experience. Overnight stay at the hotel.",
        "Day 6: Departure. After breakfast, check out and transfer to Bali airport for your onward journey."
    },
    WhatToDo = new List<string>
    {
        "Relax on the white sands of Kuta Beach",
        "Trek to the summit of Mount Batur for a breathtaking sunrise hike",
        "Enjoy a rejuvenating spa day with traditional Balinese massages, herbal body scrubs, and yoga classes",
        "Explore hidden gems and off-the-beaten-path treasures with island-hopping tours to nearby Nusa Penida, Nusa Lembongan, and the Gili Islands",
        "Experience the laid-back island life and immerse yourself in the natural beauty of Bali's neighboring islands"
    },
    Places = new List<string>
    {
        "Bali",
        "Ubud",
        "Uluwatu Temple",
        "Mount Batur",
        "Ricefields Tegallalang",
        "Tirta Empul",
        "Nusa Dua",
        "Seminyak",
        "Kuta Beach",
        "Penida Island",
        "Batubulan",
        "Celuk",
        "Mas",
        "Kintamani",
        "Batur Caldera Lake",
        "Tanjung Benoa Beach Resort",
        "Tanah Lot Temple"
    },
    Price = 1099,
    Inclusions = new List<string>
    {
        "Accommodation for 6 nights in 4-star hotels",
        "Daily breakfast",
        "Airport transfers",
        "Guided tours of all mentioned attractions",
        "Comfortable transportation for all intercity travel"
    },
    Exclusions = new List<string>
    {
        "International & domestic airfare",
        "Personal expenses such as shopping, tips, and meals not mentioned",
        "Travel insurance",
        "Additional activities not mentioned in the itinerary",
        "Entry tickets for optional attractions"
    }
},
new Destination
{
    Name = "Dubai",
    ImageUrls = new List<string>
    {
        "/assets/img/destination/Dubai.webp",
        "/assets/img/destination/Dubai1.webp",
        "/assets/img/destination/Dubai2.webp",
        "/assets/img/destination/Dubai3.webp",
        "/assets/img/destination/Dubai4.webp"
    },
    Description = "Dubai Tour Vacation offers a wealth of experiences for travelers seeking excitement, luxury, and cultural immersion. Stay in world-class hotels overlooking the iconic Burj Khalifa and enjoy VIP access to exclusive attractions and experiences.",
    Itinerary = new List<string>
    {
        "Day 1: Arrival in Dubai. Explore Dubai with TTL Holidays. Experience a desert safari with camel rides, rolling sand dunes, and a barbecue dinner with professional belly dancers. Arrive at Dubai International Airport and proceed to your hotel. Overnight stay.",
        "Day 2: Dubai city tour. After breakfast, enjoy a sightseeing tour of Dubai's iconic attractions and stunning architectural marvels. Evening at leisure. Overnight stay at the hotel.",
        "Day 3: Desert Safari with BBQ Dinner. Morning at leisure for shopping or relaxation. Afternoon desert safari including dune bashing, camel riding, quad biking, and sandboarding (additional cost). Evening entertainment with belly dancing and Tanoura Show, followed by dinner. Overnight stay at hotel.",
        "Day 4: Dhow Creek Cruise. Morning at leisure. Evening traditional dinner on the Dhow Cruise with views of Dubai Creek and illuminated cityscape, accompanied by music. Overnight stay at hotel.",
        "Day 5: Departure. Enjoy breakfast, check out (by 12:00 PM), and transfer to Dubai airport for your flight home."
    },
    WhatToDo = new List<string>
    {
        "Stay in world-class hotels overlooking the iconic Burj Khalifa",
        "Enjoy VIP access to exclusive attractions and experiences",
        "Embark on an exhilarating desert safari adventure",
        "Discover the rugged beauty of Dubai's vast sand dunes",
        "Experience the thrill of sandboarding down steep dunes",
        "Witness a breathtaking desert sunset against the backdrop of the Arabian sky",
        "Discover the rich heritage and history of Dubai at the Dubai Museum, Al Fahidi Fort, and the historic Al Bastakiya district"
    },
    Places = new List<string>
    {
        "Dubai",
        "Burj Khalifa",
        "Burj Al Arab",
        "Global Village",
        "Dubai Mall",
        "Ski Dubai",
        "Desert Safari",
        "Dubai Garden Glow",
        "Palm Jumeirah",
        "Dubai Miracle Garden",
        "Dubai International Airport",
        "Dubai Creek"
    },
    Price = 1500,
    Inclusions = new List<string>
    {
        "Accommodation for 5 nights in 4-star hotels",
        "Daily breakfast",
        "Airport transfers",
        "Guided tours of all mentioned attractions",
        "Comfortable transportation for all intercity travel"
    },
    Exclusions = new List<string>
    {
        "International & domestic airfare",
        "Personal expenses such as shopping, tips, and meals not mentioned",
        "Travel insurance",
        "Additional activities not mentioned in the itinerary",
        "Entry tickets for optional attractions"
    }
},

               new Destination
{
    Name = "Thailand",
    ImageUrls = new List<string>
    {
        "/assets/img/destination/Thailand.webp",
        "/assets/img/destination/Thailand1.webp",
        "/assets/img/destination/Thailand2.webp",
        "/assets/img/destination/Thailand3.webp",
        "/assets/img/destination/Thailand4.webp"
    },
    Description = "Thailand Tour Packages offers a tapestry of experiences that will captivate your senses and leave you longing for more. Explore the bustling streets, vibrant markets, and iconic landmarks of Bangkok. Visit the majestic Grand Palace and Wat Phra Kaew, home to the revered Emerald Buddha.",
    Itinerary = new List<string>
    {
        "Day 1: Arrival in Bangkok and transfer to Pattaya. Check in to the hotel and relax. Optional evening tour of the Alcazar Show, a world-famous cabaret. Overnight stay at the hotel in Pattaya.",
        "Day 2: Day at leisure in Pattaya. Enjoy the hotel facilities or explore the city at your own pace. Overnight stay at the hotel in Pattaya.",
        "Day 3: Pattaya to Bangkok. After breakfast, check out from the hotel and transfer back to Bangkok. Rest at leisure. Overnight stay at the hotel in Bangkok.",
        "Day 4: Phi Phi Island Tour with lunch. Depart at around 09:00 AM for a full-day speedboat tour of Phi Phi Island, including time for swimming, snorkeling, relaxing, and taking photos. Return to your hotel in the evening. Overnight stay at the hotel in Bangkok.",
        "Day 5: Departure. Enjoy buffet breakfast, complete check-out formalities, and transfer to the airport for your onward journey."
    },
    WhatToDo = new List<string>
    {
        "Explore the bustling streets, vibrant markets, and iconic landmarks of Bangkok",
        "Visit the majestic Grand Palace and Wat Phra Kaew, home to the revered Emerald Buddha",
        "Embark on an adrenaline-fueled adventure in Thailand's wild jungles and rugged landscapes",
        "Trek through the dense forests of Khao Sok National Park, home to exotic wildlife, towering limestone cliffs, and pristine lakes",
        "Enjoy an optional tour of the Alcazar Show, a world-famous cabaret"
    },
    Places = new List<string>
    {
        "Bangkok",
        "Pattaya",
        "Chiang Mai",
        "Chiang Rai",
        "Phuket",
        "Ko Samui",
        "Railay Beach",
        "Phang Nga",
        "Phi Phi Island"
    },
    Price = 1400,
    Inclusions = new List<string>
    {
        "Accommodation for 5 nights in 4-star hotels",
        "Daily breakfast",
        "Airport transfers",
        "Guided tours of all mentioned attractions",
        "Comfortable transportation for all intercity travel"
    },
    Exclusions = new List<string>
    {
        "International & domestic airfare",
        "Personal expenses such as shopping, tips, and meals not mentioned",
        "Travel insurance",
        "Additional activities not mentioned in the itinerary",
        "Entry tickets for optional attractions"
    }
},

              new Destination
{
    Name = "Jamaica",
    ImageUrls = new List<string>
    {
        "/assets/img/destination/Jamaica.webp",
        "/assets/img/destination/Jamaica1.webp",
        "/assets/img/destination/Jamaica2.webp",
        "/assets/img/destination/Jamaica3.webp",
        "/assets/img/destination/Jamaica4.webp"
    },
    Description = "Jamaica Tour Packages, the land of reggae rhythms, lush rainforests, and breathtaking beaches. Whether you're drawn to the vibrant culture, the stunning landscapes, or the warm hospitality of its people, Jamaica Tour Packages offers a diverse array of experiences for every traveler.",
    Itinerary = new List<string>
    {
        "Day 1: Arrival in Kingston. Transfer to your hotel at Spanish Court Hotel Hill. Afternoon at leisure. Dinner included. Overnight at the hotel.",
        "Day 2: Kingston City Tour & Bob Marley Museum (Optional). Explore Kingston including the National Gallery, Holy Trinity Cathedral, Devon House, and Bob Marley Museum. Meals included: Breakfast, Lunch & Dinner. Overnight at Spanish Court Hotel.",
        "Day 3: Kingston to Ocho Rios (Optional). Drive to Ocho Rios via Cascade Rainforest, Island Gully Falls, and Blue Hole. Lunch at a local jerk center (not included). Overnight at RIU Ocho Rios with Dinner.",
        "Day 4: Dunn's River Falls and Jungle River Tubing Tour (Optional). Visit Dunn's River Falls and enjoy tubing at White River. Lunch at a local jerk center. Meals included: Breakfast, Lunch & Dinner. Overnight at RIU Ocho Rios.",
        "Day 5: Ocho Rios to Montego Bay – Evening Reggae Catamaran Cruise (Optional). Drive to Montego Bay, check-in at SeaGarden Beach Resort, and enjoy an evening catamaran cruise with snorkeling, music, and drinks. Meals included: Breakfast, Lunch & Dinner. Overnight at RIU Reggae Resort.",
        "Day 6: Negril Day Trip and Rick's Café (Optional). Explore Negril, visit Rick's Café for cliff diving, swimming, and sunset views. Meals included: Breakfast, Lunch & Dinner. Overnight at RIU Reggae Resort.",
        "Day 7: Departure. Transfer to Montego Bay airport for onward flight. Breakfast included."
    },
    WhatToDo = new List<string>
    {
        "Immerse yourself in the laid-back vibes of Negril's Seven Mile Beach",
        "Embark on an adrenaline-fueled adventure with the Explorer Experience package",
        "Experience the rhythms of reggae music at a live concert or dancehall party",
        "Savor the flavors of Jamaican cuisine with a culinary tour of local markets and eateries",
        "Retreat to secluded luxury resorts in Ocho Rios or Montego Bay"
    },
    Places = new List<string>
    {
        "Montego Bay",
        "Negril",
        "Ocho Rios",
        "Kingston",
        "Falmouth"
    },
    Price = 1600,
    Inclusions = new List<string>
    {
        "Accommodation for 7 nights in 4-star hotels",
        "Daily breakfast",
        "Airport transfers",
        "Guided tours of all mentioned attractions",
        "Comfortable transportation for all intercity travel"
    },
    Exclusions = new List<string>
    {
        "International & domestic airfare",
        "Personal expenses such as shopping, tips, and meals not mentioned",
        "Travel insurance",
        "Additional activities not mentioned in the itinerary",
        "Entry tickets for optional attractions"
    }
},

               new Destination
{
    Name = "Mexico",
    ImageUrls = new List<string>
    {
        "/assets/img/destination/Mexico.webp",
        "/assets/img/destination/Mexico1.webp",
        "/assets/img/destination/Mexico2.webp",
        "/assets/img/destination/Mexico3.webp",
        "/assets/img/destination/Mexico4.webp"
    },
    Description = "From ancient ruins and colonial cities to pristine beaches and lush jungles, there's something for everyone in this diverse and captivating country. Discover the colonial charm of cities like Mexico City, Oaxaca, and Puebla, with their cobblestone streets, colorful buildings, and historic landmarks.",
    Itinerary = new List<string>
    {
        "Day 1: Arrival in Cancun. Check-in at the hotel and enjoy the day at leisure.",
        "Day 2: Chichen Itza, Cenote, and Valladolid – All-Inclusive Tour (10-11 hrs). Explore the ancient Maya site of Chichen Itza (admission not included), Valladolid, and refresh in a cenote. Buffet meal included.",
        "Day 3: Cancun ATV Jungle Adventure, Ziplines, Cenote, and Tequila Tasting (4 hrs). Ride ATVs over jungle trails, try three zipline flights, swim in a cenote, and enjoy a tequila tasting.",
        "Day 4: Isla Mujeres Luxury Catamaran Sailing plus Lunch and Open Bar (5-6 hrs). Snorkel along Cancun's coastline, enjoy a gourmet lunch onboard, and visit Isla Mujeres.",
        "Day 5: Cancun – Day at Leisure. Relax at the hotel or enjoy Cancun Beach and local attractions.",
        "Day 6: Departure. Transfer to the airport for your flight back home or onward destination."
    },
    WhatToDo = new List<string>
    {
        "Indulge in beachfront accommodations",
        "Swim in crystal-clear cenotes",
        "Snorkel among vibrant coral reefs",
        "Sample authentic Mexican cuisine with guided food tours",
        "Discover natural wonders with eco-adventure expeditions",
        "Enjoy candlelit dinners under the stars, couples' spa treatments, and sunset cruises along the coast"
    },
    Places = new List<string>
    {
        "Mexico City",
        "Cancún",
        "Puerto Vallarta",
        "Cabo San Lucas",
        "Playa del Carmen",
        "Cozumel",
        "Riviera Maya",
        "Chichén Itzá",
        "Valladolid",
        "Isla Mujeres"
    },
    Price = 1500,
    Inclusions = new List<string>
    {
        "Accommodation for 6 nights in 4-star hotels",
        "Daily breakfast",
        "Airport transfers",
        "Guided tours of all mentioned attractions",
        "Comfortable transportation for all intercity travel"
    },
    Exclusions = new List<string>
    {
        "International & domestic airfare",
        "Personal expenses such as shopping, tips, and meals not mentioned",
        "Travel insurance",
        "Additional activities not mentioned in the itinerary",
        "Entry tickets for optional attractions"
    }
},

             new Destination
{
    Name = "Bahamas",
    ImageUrls = new List<string>
    {
        "/assets/img/destination/Bahamas.webp",
        "/assets/img/destination/Bahamas1.webp",
        "/assets/img/destination/Bahamas2.webp",
        "/assets/img/destination/Bahamas3.webp",
        "/assets/img/destination/Bahamas4.webp"
    },
    Description = "Whether you're dreaming of lazy days on the sand, thrilling water sports adventures, or exploring the colorful streets of historic towns, the Bahamas Tour Vacation offers an unforgettable vacation experience for every traveler.",
    Itinerary = new List<string>
    {
        "Day 1: Arrive in Nassau. Check into the hotel upon arrival and enjoy the stunning views of the Bahamas' clear waters and surrounding islands.",
        "Day 2: Nassau City Tour. Explore Fort Fincastle, climb the Queens Staircase, visit Fort Charlotte, admire statues of historical figures, and shop at the Straw Market.",
        "Day 3: Full Day Excursion to Blue Lagoon Island. Cruise through Nassau's harbor, relax on the white sands, swim in the lagoon, swing in a hammock, and spot dolphins and sea lions in the Marine Park.",
        "Day 4: Departure. Check out from the hotel and transfer to the airport for your flight back home or to your next destination."
    },
    WhatToDo = new List<string>
    {
        "Snorkel among vibrant coral reefs",
        "Kayak through mangrove forests",
        "Unwind with a refreshing cocktail as you watch the sunset over the ocean",
        "Explore the crystal-clear waters of the Bahamas with water sports activities like snorkeling, scuba diving, parasailing, and jet skiing",
        "Enjoy guided tours and authentic cultural experiences"
    },
    Places = new List<string>
    {
        "Nassau",
        "Bimini",
        "Pig Beach",
        "Freeport",
        "Little San Salvador",
        "Green Turtle Cay",
        "Blue Lagoon Island (Salt Cay)",
        "Fort Fincastle",
        "Queens Staircase",
        "Fort Charlotte",
        "Straw Market"
    },
    Price = 1400,
    Inclusions = new List<string>
    {
        "Accommodation for 4 nights in 4-star hotels",
        "Daily breakfast",
        "Airport transfers",
        "Guided tours of all mentioned attractions",
        "Comfortable transportation for all intercity travel"
    },
    Exclusions = new List<string>
    {
        "International & domestic airfare",
        "Personal expenses such as shopping, tips, and meals not mentioned",
        "Travel insurance",
        "Additional activities not mentioned in the itinerary",
        "Entry tickets for optional attractions"
    }
},

               new Destination
{
    Name = "Dominican Republic",
    ImageUrls = new List<string>
    {
        "/assets/img/destination/dominic_republic.webp",
        "/assets/img/destination/dominic_republic1.webp",
        "/assets/img/destination/dominic_republic2.webp",
        "/assets/img/destination/dominic_republic3.webp",
        "/assets/img/destination/dominic_republic4.webp"
    },
    Description = "Whether you're a beach lover, an adventure seeker, a history buff, or a food enthusiast, there's something for everyone in this vibrant Caribbean nation. Explore the pristine beaches of Punta Cana, Bavaro, and Juanillo, where palm-fringed sands stretch as far as the eye can see.",
    Itinerary = new List<string>
    {
        "Day 1: Arrive in Punta Cana. Arrive at Punta Cana International Airport, complete immigration formalities, and transfer to hotel.",
        "Day 2: Santo Domingo city tour. Experience natural beauty, colonial architecture, and rich traditions. Visit UNESCO Colonial Zone, Los Tres Ojos, Columbus Lighthouse, Santa Maria la Menor Cathedral, and more.",
        "Day 3: Free Day. Explore the city or enjoy hotel/resort facilities.",
        "Day 4: Transfer to Samana from Punta Cana. Morning check-out and transfer to Samana.",
        "Day 5: Whale Watching Tour & Cayo Levantado Island. Whale and dolphin watching cruise on Samana Bay, followed by a full-day trip to Cayo Levantado Island with buffet lunch and beach activities.",
        "Day 6: Departure from Puerto Plata. Morning check-out and transfer to airport for flight back home."
    },
    WhatToDo = new List<string>
    {
        "Explore the pristine beaches of Punta Cana, Bavaro, and Juanillo",
        "Embark on a thrilling adventure through the Dominican Republic's diverse landscapes",
        "Retreat to tranquil eco-resorts nestled in the countryside",
        "Enjoy personalized service and unforgettable romantic experiences",
        "Experience the natural beauty, colonial architecture, and rich traditions of Santo Domingo",
        "Take a whale watching tour and visit Cayo Levantado Island"
    },
    Places = new List<string>
    {
        "Punta Cana",
        "Santo Domingo",
        "Puerto Plata",
        "Samana",
        "Cabarete",
        "Cayo Levantado Island",
        "Samana Bay"
    },
    Price = 1500,
    Inclusions = new List<string>
    {
        "Accommodation for 6 nights in 4-star hotels",
        "Daily breakfast",
        "Airport transfers",
        "Guided tours of all mentioned attractions",
        "Comfortable transportation for all intercity travel"
    },
    Exclusions = new List<string>
    {
        "International & domestic airfare",
        "Personal expenses such as shopping, tips, and meals not mentioned",
        "Travel insurance",
        "Additional activities not mentioned in the itinerary",
        "Entry tickets for optional attractions"
    }
},
new Destination
{
    Name = "Blue Mountains",
    ImageUrls = new List<string>
    {
        "/assets/img/destination/Clutha.webp",
        "/assets/img/destination/Clutha1.webp",
        "/assets/img/destination/Clutha2.webp",
        "/assets/img/destination/Clutha3.webp",
        "/assets/img/destination/Clutha4.webp"
    },
    Description = "The Blue Mountains, a UNESCO World Heritage site, offers a unique blend of natural beauty, rich history, and adventure. From the iconic Three Sisters to the serene Jamison Valley, this region is perfect for hikers, nature lovers, and anyone seeking a tranquil escape.",
    Itinerary = new List<string>
    {
        "Day 1: Arrival in Sydney. Transfer to Blue Mountains. Check into accommodation and enjoy the stunning views. Relax and unwind after your journey.",
        "Day 2: Blue Mountains National Park Tour. Explore the famous Three Sisters rock formation, visit Scenic World, and take rides on the Scenic Railway, Cableway, and Skyway. Enjoy a guided walk through the ancient rainforest.",
        "Day 3: Full Day Excursion to Katoomba. Discover the charming town of Katoomba, visit Katoomba Falls, and stroll through the town's markets.",
        "Day 4: Departure. Transfer back to Sydney for flight home or next destination."
    },
    WhatToDo = new List<string>
    {
        "Hike through lush rainforests and eucalyptus groves",
        "Visit the iconic Three Sisters rock formation",
        "Take scenic rides on the Scenic Railway, Cableway, and Skyway",
        "Explore the charming town of Katoomba and its markets",
        "Enjoy a guided tour of the Blue Mountains National Park"
    },
    Places = new List<string>
    {
        "Sydney",
        "Blue Mountains National Park",
        "Katoomba",
        "Jamison Valley",
        "Scenic World"
    },
    Price = 1400,
    Inclusions = new List<string>
    {
        "Accommodation for 4 nights in 4-star hotels",
        "Daily breakfast",
        "Airport transfers",
        "Guided tours of all mentioned attractions",
        "Comfortable transportation for all intercity travel"
    },
    Exclusions = new List<string>
    {
        "International & domestic airfare",
        "Personal expenses such as shopping, tips, and meals not mentioned",
        "Travel insurance",
        "Additional activities not mentioned in the itinerary",
        "Entry tickets for optional attractions"
    }
},

                new Destination
{
    Name = "Australian Outback",
    ImageUrls = new List<string>
    {
        "/assets/img/destination/australian_outback.webp",
        "/assets/img/destination/australian_outback1.webp",
        "/assets/img/destination/australian_outback2.webp",
        "/assets/img/destination/australian_outback3.webp",
        "/assets/img/destination/australian_outback4.webp"
    },
    Description = "The Australian Outback is a vast, arid region that stretches across much of Australia. It is a land of breathtaking landscapes, rich indigenous culture, and unique wildlife. Whether you're looking for adventure or a peaceful escape, the Outback offers an unforgettable experience.",
    Itinerary = new List<string>
    {
        "Day 1: Arrival in Alice Springs. Upon arrival, transfer to your accommodation. Relax and enjoy the evening in this vibrant town, known as the heart of the Outback.",
        "Day 2: Uluru-Kata Tjuta National Park Tour. Travel to Uluru (Ayers Rock) and explore this sacred site with a guided tour. Learn about the indigenous Anangu culture and the significance of Uluru. Visit the nearby Kata Tjuta (The Olgas) and take a scenic walk through the domes.",
        "Day 3: Full Day Excursion to Kings Canyon. Enjoy a guided walk through the stunning Kings Canyon, known for its dramatic sandstone cliffs and lush palm oases. Visit the North and South Walls, and the Garden of Eden.",
        "Day 4: Departure. Transfer back to Alice Springs for your flight home or to the next destination."
    },
    WhatToDo = new List<string>
    {
        "Visit Uluru (Ayers Rock) and learn about its cultural significance",
        "Explore the dramatic landscapes of Kings Canyon",
        "Take a scenic flight over the Outback",
        "Experience the rich indigenous culture through guided tours and local stories",
        "Enjoy a sunset drink at Uluru"
    },
    Places = new List<string>
    {
        "Alice Springs",
        "Uluru-Kata Tjuta National Park",
        "Kings Canyon",
        "The Olgas",
        "Garden of Eden"
    },
    Price = 1500,
    Inclusions = new List<string>
    {
        "Accommodation for 4 nights in 4-star hotels",
        "Daily breakfast",
        "Airport transfers",
        "Guided tours of all mentioned attractions",
        "Comfortable transportation for all intercity travel"
    },
    Exclusions = new List<string>
    {
        "International & domestic airfare",
        "Personal expenses such as shopping, tips, and meals not mentioned",
        "Travel insurance",
        "Additional activities not mentioned in the itinerary",
        "Entry tickets for optional attractions"
    }
},

                new Destination
                {
                    Name = "Auckland",
                    ImageUrls = new List<string> { "/assets/img/destination/Auckland.webp",
        "/assets/img/destination/Auckland1.webp",
        "/assets/img/destination/Auckland2.webp",
        "/assets/img/destination/Auckland3.webp",
        "/assets/img/destination/Auckland4.webp"},
                    Description ="Discover the vibrant city of Auckland, known as the 'City of Sails,' and its stunning natural surroundings. This tour takes you through Auckland's iconic landmarks, the picturesque Waitomo Caves, the geothermal wonders of Rotorua, and the famous Hobbiton Movie Set. Enjoy breathtaking landscapes, cultural experiences, and adventure activities in this unforgettable New Zealand getaway.",
                    Itinerary = new List<string> {"Day 1: Arrival & Auckland City Tour. Arrive in Auckland and transfer to the hotel. Visit Sky Tower for a panoramic view of the city. Explore Auckland Harbour, Viaduct, and Queen Street. Relax at Mission Bay Beach. Overnight stay in Auckland.",
        "Day 2: Waitomo Glowworm Caves & Rotorua. Breakfast at the hotel. Travel to Waitomo Caves and explore the glowworm-lit caverns. Continue to Rotorua and visit Te Puia Geothermal Park. Enjoy a traditional Maori cultural performance and Hangi dinner. Overnight stay in Rotorua.",
        "Day 3: Hobbiton Movie Set & Return to Auckland. Breakfast at the hotel. Travel to Matamata for the Hobbiton Movie Set tour. Explore iconic locations from The Lord of the Rings. Return to Auckland for overnight stay.",
        "Day 4: Waiheke Island Wine & Scenic Tour. Breakfast at the hotel. Take a ferry to Waiheke Island. Enjoy wine tasting at renowned vineyards. Explore beautiful beaches and art galleries. Return to Auckland for overnight stay.",
        "Day 5: Mount Eden & Departure. Breakfast at the hotel. Visit Mount Eden for stunning views of the city. Free time for shopping or leisure. Transfer to the airport for departure."},
                    WhatToDo = new List<string> { "Enjoy panoramic views of the city from the Sky Tower",
        "Explore the bustling waterfront area of Auckland Harbour & Viaduct",
        "Witness magical glowworms in the limestone caves of Waitomo",
        "Experience the geothermal wonders and Maori culture in Rotorua",
        "Visit the real-life set of The Lord of the Rings & The Hobbit at Hobbiton Movie Set",
        "Enjoy wine tasting and explore beautiful beaches on Waiheke Island",
        "Take in scenic views of Auckland's volcanic landscapes from Mount Eden"},
                    Places = new List<string> {"Auckland",
        "Waitomo Caves",
        "Rotorua",
        "Hobbiton Movie Set",
        "Waiheke Island",
        "Mount Eden"},
                    Price = 1600,
                    Inclusions = new List<string> {  "Accommodation for 5 nights in 4-star hotels",
        "Daily breakfast",
        "Airport transfers",
        "Guided tours of all mentioned attractions",
        "Comfortable transportation for all intercity travel"},
                    Exclusions = new List<string> {"International & domestic airfare",
        "Personal expenses such as shopping, tips, and meals not mentioned",
        "Travel insurance",
        "Additional activities not mentioned in the itinerary",
        "Entry tickets for optional attractions"},
                },
                new Destination
                {
                    Name = "Hamilton",
                    ImageUrls = new List<string> {"/assets/img/destination/Hamilton.webp",
        "/assets/img/destination/Hamilton1.webp",
        "/assets/img/destination/Hamilton2.webp",
        "/assets/img/destination/Hamilton3.webp",
        "/assets/img/destination/Hamilton4.webp"},
                    Description = "Explore the charming city of Hamilton, New Zealand, known for its stunning gardens, vibrant culture, and proximity to incredible attractions. This tour takes you through the breathtaking Hamilton Gardens, the iconic Hobbiton Movie Set, the magical Waitomo Glowworm Caves, and the serene Waikato River. Enjoy a perfect blend of nature, history, and adventure in this memorable journey.",
                    Itinerary = new List<string> { "Day 1: Arrival & Hamilton City Exploration. Arrive in Hamilton and transfer to the hotel. Visit Hamilton Gardens and explore its themed sections. Walk along the Waikato River and enjoy the scenic views. Explore Waikato Museum and learn about the region's rich history. Overnight stay in Hamilton.",
        "Day 2: Waitomo Glowworm Caves & Hobbiton Movie Set. Breakfast at the hotel. Travel to Waitomo and take a guided boat tour through the Glowworm Caves. Continue to Matamata and enjoy a guided tour of the Hobbiton Movie Set. Return to Hamilton for overnight stay.",
        "Day 3: Raglan Beach & Bridal Veil Falls. Breakfast at the hotel. Travel to Raglan, a beautiful coastal town known for its surf beaches. Visit Bridal Veil Falls and enjoy a short nature walk. Explore Raglan's cafes and art scene. Return to Hamilton for overnight stay.",
        "Day 4: Zealong Tea Estate & Departure. Breakfast at the hotel. Enjoy a tea-tasting session at Zealong Tea Estate. Free time for shopping or leisure. Transfer to the airport for departure."},
                    WhatToDo = new List<string> { "Visit Hamilton Gardens and explore its themed sections",
        "Take a guided boat tour through the Waitomo Glowworm Caves",
        "Enjoy a guided tour of the Hobbiton Movie Set",
        "Explore Raglan Beach and Bridal Veil Falls",
        "Learn about the region's rich history at Waikato Museum",
        "Enjoy a tea-tasting session at Zealong Tea Estate"},
                    Places = new List<string> { "Hamilton",
        "Hamilton Gardens",
        "Waitomo Glowworm Caves",
        "Hobbiton Movie Set",
        "Waikato Museum",
        "Zealong Tea Estate",
        "Raglan Beach",
        "Bridal Veil Falls"},
                    Price =  1500,
                    Inclusions = new List<string> { "Accommodation for 4 nights in 4-star hotels",
        "Daily breakfast",
        "Airport transfers",
        "Guided tours of all mentioned attractions",
        "Comfortable transportation for all intercity travel"},
                    Exclusions = new List<string> { "International & domestic airfare",
        "Personal expenses such as shopping, tips, and meals not mentioned",
        "Travel insurance",
        "Additional activities not mentioned in the itinerary",
        "Entry tickets for optional attractions"},
                },
                new Destination
                {
                    Name = "Wellington",
                    ImageUrls = new List<string> { "/assets/img/destination/Wellington.webp",
        "/assets/img/destination/Wellington1.webp",
        "/assets/img/destination/Wellington2.webp",
        "/assets/img/destination/Wellington3.webp",
        "/assets/img/destination/Wellington4.webp"},
                    Description =  "Discover the capital city of New Zealand, Wellington, known for its stunning waterfront, vibrant arts scene, and rich history. This tour takes you through the iconic Te Papa Museum, the breathtaking views from Mount Victoria, the lush greenery of the Wellington Botanic Garden, and the cinematic wonders of Weta Workshop. Experience the charm and culture of Wellington in an unforgettable journey.",
                    Itinerary = new List<string> { "Day 1: Arrival & Wellington City Tour. Arrive in Wellington and transfer to the hotel. Visit Te Papa Tongarewa Museum for an interactive cultural experience. Walk along the Wellington Waterfront and enjoy the scenic Oriental Bay. Explore Cuba Street for dining and shopping. Overnight stay in Wellington.",
        "Day 2: Mount Victoria & Weta Workshop. Breakfast at the hotel. Drive up to Mount Victoria Lookout for stunning panoramic views. Visit Weta Workshop for an insider's look at movie magic. Explore the Wellington Botanic Garden via the iconic Wellington Cable Car. Overnight stay in Wellington.",
        "Day 3: Zealandia & Free Time. Breakfast at the hotel. Take a guided eco-tour of Zealandia, a world-renowned urban wildlife sanctuary. Enjoy free time to explore Wellington at your own pace. Optional activities: craft beer tasting, Parliament tour, or shopping. Overnight stay in Wellington.",
        "Day 4: Final Exploration & Departure. Breakfast at the hotel. Enjoy a relaxing morning at Oriental Bay or visit local markets. Transfer to the airport for departure."},
                    WhatToDo = new List<string> { "Visit Te Papa Tongarewa Museum for an interactive cultural experience",
        "Walk along the Wellington Waterfront and enjoy the scenic Oriental Bay",
        "Explore Cuba Street for dining and shopping",
        "Drive up to Mount Victoria Lookout for stunning panoramic views",
        "Visit Weta Workshop for an insider's look at movie magic",
        "Explore the Wellington Botanic Garden via the iconic Wellington Cable Car",
        "Take a guided eco-tour of Zealandia, a world-renowned urban wildlife sanctuary"},
                    Places = new List<string> {"Wellington",
        "Te Papa Tongarewa Museum",
        "Wellington Waterfront",
        "Oriental Bay",
        "Cuba Street",
        "Mount Victoria Lookout",
        "Weta Workshop",
        "Wellington Botanic Garden",
        "Zealandia Ecosanctuary"},
                    Price = 1400,
                    Inclusions = new List<string> {  "Accommodation for 4 nights in 4-star hotels",
        "Daily breakfast",
        "Airport transfers",
        "Guided tours of all mentioned attractions",
        "Comfortable transportation for all intercity travel"},
                    Exclusions = new List<string> { "International & domestic airfare",
        "Personal expenses such as shopping, tips, and meals not mentioned",
        "Travel insurance",
        "Additional activities not mentioned in the itinerary",
        "Entry tickets for optional attractions"},
                },
                new Destination
                {
                    Name = "Waikato",
                    ImageUrls = new List<string> { "/assets/img/destination/Waikato.webp",
        "/assets/img/destination/Waikato1.webp",
        "/assets/img/destination/Waikato2.webp",
        "/assets/img/destination/Waikato3.webp",
        "/assets/img/destination/Waikato4.webp"},
                    Description = "Explore the stunning Waikato region, known for its lush landscapes, rich Maori culture, and world-famous attractions. This tour takes you through the magical Waitomo Glowworm Caves, the iconic Hobbiton Movie Set, the beautiful Hamilton Gardens, and the scenic Waikato River. Experience a mix of adventure, nature, and cultural heritage in this unforgettable journey.",
                    Itinerary = new List<string> {"Day 1: Arrival & Hamilton City Exploration. Arrive in Waikato and transfer to the hotel. Visit Hamilton Gardens and explore its themed sections. Take a relaxing Waikato River cruise. Free time to explore Hamilton's shopping and dining areas. Overnight stay in Hamilton.",
        "Day 2: Waitomo Glowworm Caves & Hobbiton Movie Set. Breakfast at the hotel. Travel to Waitomo and take a guided boat tour through the Glowworm Caves. Continue to Matamata for a guided tour of the Hobbiton Movie Set. Return to Hamilton for overnight stay.",
        "Day 3: Raglan Beach & Bridal Veil Falls. Breakfast at the hotel. Travel to Raglan, a beautiful coastal town known for its surf beaches. Visit Bridal Veil Falls and enjoy a short nature walk. Explore Raglan's cafes and art scene. Return to Hamilton for overnight stay.",
        "Day 4: Zealong Tea Estate & Departure. Breakfast at the hotel. Enjoy a tea-tasting session at Zealong Tea Estate. Free time for shopping or leisure. Transfer to the airport for departure."},
                    WhatToDo = new List<string> {"Visit Hamilton Gardens and explore its themed sections",
        "Take a relaxing Waikato River cruise",
        "Take a guided boat tour through the Waitomo Glowworm Caves",
        "Enjoy a guided tour of the Hobbiton Movie Set",
        "Visit Raglan Beach and Bridal Veil Falls",
        "Explore Raglan's cafes and art scene",
        "Enjoy a tea-tasting session at Zealong Tea Estate"},
                    Places = new List<string> { "Waikato",
        "Hamilton Gardens",
        "Waitomo Glowworm Caves",
        "Hobbiton Movie Set",
        "Raglan Beach",
        "Bridal Veil Falls",
        "Zealong Tea Estate"},
                    Price = 1500,
                    Inclusions = new List<string> {"Accommodation for 4 nights in 4-star hotels",
        "Daily breakfast",
        "Airport transfers",
        "Guided tours of all mentioned attractions",
        "Comfortable transportation for all intercity travel"},
                    Exclusions = new List<string> { "International & domestic airfare",
        "Personal expenses such as shopping, tips, and meals not mentioned",
        "Travel insurance",
        "Additional activities not mentioned in the itinerary",
        "Entry tickets for optional attractions"},
                },
    new Destination
{
    Name = "Rotorua",
    ImageUrls = new List<string>
    {
        "/assets/img/destination/Rotorua.webp",
        "/assets/img/destination/Rotorua1.webp",
        "/assets/img/destination/Rotorua2.webp",
        "/assets/img/destination/Rotorua3.webp",
        "/assets/img/destination/Rotorua4.webp"
    },
    Description = "Rotorua, known as the geothermal and cultural heart of New Zealand, offers breathtaking landscapes, rich Maori heritage, and thrilling adventure activities. This tour takes you through bubbling geothermal parks, Maori cultural experiences, stunning lakes, and thrilling adventure activities for a well-rounded and unforgettable journey.",
    Itinerary = new List<string>
    {
        "Day 1: Arrival & Rotorua City Exploration. Arrive in Rotorua and transfer to the hotel. Visit the Rotorua Museum and Government Gardens. Enjoy a relaxing soak at Polynesian Spa. Free time to explore the local area. Overnight stay in Rotorua.",
        "Day 2: Geothermal Wonders & Maori Culture. Breakfast at the hotel. Explore Te Puia Geothermal Park and witness the Pohutu Geyser. Visit Wai-O-Tapu Thermal Wonderland to see the famous Champagne Pool. Evening cultural experience at Tamaki Maori Village with a Hangi feast. Return to the hotel for overnight stay.",
        "Day 3: Adventure & Nature Exploration. Breakfast at the hotel. Walk through Redwoods Whakarewarewa Forest. Visit the Agrodome for a farm tour and sheep show. Enjoy thrilling luge rides and gondola rides at Skyline Rotorua. Free time for optional activities such as ziplining or river rafting. Overnight stay in Rotorua.",
        "Day 4: Lake Rotorua & Departure. Breakfast at the hotel. Enjoy a scenic cruise on Lake Rotorua. Free time for shopping or relaxation. Transfer to the airport for departure."
    },
    WhatToDo = new List<string>
    {
        "Visit Te Puia Geothermal Park and witness the Pohutu Geyser",
        "Relax in natural geothermal hot pools at Polynesian Spa",
        "Enjoy a traditional Maori performance and Hangi feast at Tamaki Maori Village",
        "Walk through the Redwoods Whakarewarewa Forest",
        "Visit the Agrodome for a farm tour and sheep show",
        "Enjoy thrilling luge rides and gondola rides at Skyline Rotorua",
        "Explore Wai-O-Tapu Thermal Wonderland to see the famous Champagne Pool",
        "Enjoy a scenic cruise on Lake Rotorua"
    },
    Places = new List<string>
    {
        "Rotorua",
        "Te Puia Geothermal Park",
        "Polynesian Spa",
        "Tamaki Maori Village",
        "Redwoods Whakarewarewa Forest",
        "Skyline Rotorua",
        "Wai-O-Tapu Thermal Wonderland",
        "Agrodome",
        "Lake Rotorua"
    },
    Price = 1600,
    Inclusions = new List<string>
    {
        "Accommodation for 4 nights in 4-star hotels",
        "Daily breakfast",
        "Airport transfers",
        "Guided tours of all mentioned attractions",
        "Comfortable transportation for all intercity travel"
    },
    Exclusions = new List<string>
    {
        "International & domestic airfare",
        "Personal expenses such as shopping, tips, and meals not mentioned",
        "Travel insurance",
        "Additional activities not mentioned in the itinerary",
        "Entry tickets for optional attractions"
    }
    },
     new Destination
                {
                    Name ="Christchurch",
                    ImageUrls = new List<string> { "/assets/img/destination/Christchurch.webp",
        "/assets/img/destination/Christchurch1.webp",
        "/assets/img/destination/Christchurch2.webp",
        "/assets/img/destination/Christchurch3.webp",
        "/assets/img/destination/Christchurch4.webp"},
                    Description = "Christchurch, the largest city in the South Island of New Zealand, is renowned for its stunning gardens, historic architecture, and vibrant cultural scene. This tour will take you through the city's must-see attractions, including the beautiful Botanic Gardens, scenic Avon River, and the historic tram ride. Experience the charm of Christchurch alongside breathtaking landscapes and adventure opportunities in the surrounding Canterbury region.",
                    Itinerary = new List<string> {"Day 1: Arrival & Christchurch City Exploration. Arrive in Christchurch and transfer to the hotel. Visit the Christchurch Botanic Gardens. Explore Canterbury Museum & Arts Centre. Enjoy a relaxing punting experience on the Avon River. Overnight stay in Christchurch.",
        "Day 2: Scenic Adventures & Wildlife. Breakfast at the hotel. Take a ride on the Christchurch Gondola for panoramic views. Visit Willowbank Wildlife Reserve to see native New Zealand animals. Explore the historic New Regent Street for shopping and dining. Overnight stay in Christchurch.",
        "Day 3: Akaroa & Banks Peninsula Day Trip. Breakfast at the hotel. Travel to Akaroa, a picturesque French-inspired coastal town. Enjoy a harbor cruise to spot dolphins and marine life. Explore the charming streets and local boutiques of Akaroa. Return to Christchurch for an overnight stay.",
        "Day 4: International Antarctic Centre & Departure. Breakfast at the hotel. Visit the International Antarctic Centre for a unique polar experience. Free time for last-minute shopping or sightseeing. Transfer to the airport for departure."
  },
                    WhatToDo = new List<string> {  "Visit Christchurch Botanic Gardens for a stunning display of native and exotic flora",
        "Enjoy a relaxing punting experience on the Avon River",
        "Take a ride on the Christchurch Gondola for breathtaking panoramic views",
        "Experience the wonders of Antarctica at the International Antarctic Centre",
        "Explore the charming streets and local boutiques of Akaroa",
        "Enjoy a harbor cruise to spot dolphins and marine life",
        "See New Zealand's native kiwi bird up close at Willowbank Wildlife Reserve"},
                    Places = new List<string> { "Christchurch",
        "Christchurch Botanic Gardens",
        "Avon River",
        "Christchurch Gondola",
        "International Antarctic Centre",
        "Canterbury Museum & Arts Centre",
        "Port Hills",
        "Akaroa",
        "Banks Peninsula",
        "Willowbank Wildlife Reserve"},
                    Price =1500,
                    Inclusions = new List<string> {"Accommodation for 4 nights in 4-star hotels",
        "Daily breakfast",
        "Airport transfers",
        "Guided tours of all mentioned attractions",
        "Comfortable transportation for all intercity travel"},
                    Exclusions = new List<string> { "International & domestic airfare",
        "Personal expenses such as shopping, tips, and meals not mentioned",
        "Travel insurance",
        "Additional activities not mentioned in the itinerary",
        "Entry tickets for optional attractions"},
                },
                new Destination
                {
                    Name = "Dunedin",
                    ImageUrls = new List<string> {  "/assets/img/destination/Dunedin.webp",
        "/assets/img/destination/Dunedin1.webp",
        "/assets/img/destination/Dunedin2.webp",
        "/assets/img/destination/Dunedin3.webp",
        "/assets/img/destination/Dunedin4.webp"},
                    Description =  "Dunedin, known as the 'Edinburgh of the South,' is famous for its Scottish heritage, stunning natural landscapes, and rich wildlife. This tour takes you through Dunedin's historic landmarks, breathtaking coastal scenery, and unique wildlife experiences. Discover the beauty of Otago Peninsula, explore Larnach Castle, and witness rare albatross colonies in this unforgettable journey.",

                    Itinerary = new List<string> { "Day 1: Arrival & City Exploration. Arrive in Dunedin and transfer to the hotel. Visit Baldwin Street, the steepest street in the world. Explore the historic Dunedin Railway Station. Free time to explore the city center. Overnight stay in Dunedin.",
        "Day 2: Otago Peninsula & Wildlife Tour. Breakfast at the hotel. Travel to the Otago Peninsula for scenic views. Visit the Royal Albatross Centre to see albatross in the wild. Explore Larnach Castle and its beautiful gardens. Return to Dunedin for overnight stay.",
        "Day 3: Coastal Adventures & Local Culture. Breakfast at the hotel. Hike to Tunnel Beach for breathtaking ocean views. Visit Speight's Brewery for a guided tour and beer tasting. Explore the Dunedin Botanic Garden. Free time for shopping and local dining. Overnight stay in Dunedin.",
        "Day 4: Leisure & Departure. Breakfast at the hotel. Free time for last-minute sightseeing or shopping. Transfer to the airport for departure."},
                    WhatToDo = new List<string> { "Visit Larnach Castle and its stunning gardens",
        "Explore the scenic Otago Peninsula",
        "Witness majestic albatross birds at the Royal Albatross Centre",
        "Hike to Tunnel Beach for breathtaking ocean views",
        "Visit Speight's Brewery for a guided tour and beer tasting",
        "Explore the Dunedin Botanic Garden",
        "Discover the historic Dunedin Railway Station",
        "Experience the world's steepest residential street at Baldwin Street"},
                    Places = new List<string> {"Dunedin",
        "Larnach Castle",
        "Otago Peninsula",
        "Royal Albatross Centre",
        "Baldwin Street",
        "Dunedin Railway Station",
        "Tunnel Beach",
        "Speight's Brewery",
        "Dunedin Botanic Garden"},
                    Price = 1400,
                    Inclusions = new List<string> { "Accommodation for 4 nights in 4-star hotels",
        "Daily breakfast",
        "Airport transfers",
        "Guided tours of all mentioned attractions",
        "Comfortable transportation for all intercity travel"},
                    Exclusions = new List<string> {  "International & domestic airfare",
        "Personal expenses such as shopping, tips, and meals not mentioned",
        "Travel insurance",
        "Additional activities not mentioned in the itinerary",
        "Entry tickets for optional attractions"},
                },
                 new Destination
                {
                    Name = "Fiordland",
                    ImageUrls = new List<string> {  "/assets/img/destination/Fiordland.webp",
        "/assets/img/destination/Fiordland1.webp",
        "/assets/img/destination/Fiordland2.webp",
        "/assets/img/destination/Fiordland3.webp",
        "/assets/img/destination/Fiordland4.webp"},
                    Description = "Fiordland, located in the southwest of New Zealand's South Island, is a world-renowned region of breathtaking natural beauty. Home to the famous Milford Sound and Doubtful Sound, Fiordland offers pristine wilderness, towering waterfalls, and unique wildlife encounters. This tour will take you on an unforgettable journey through lush rainforests, mirror-like lakes, and dramatic mountain landscapes.",
                    Itinerary = new List<string> { "Day 1: Arrival & Te Anau Exploration. Arrive in Queenstown and transfer to Te Anau, the gateway to Fiordland. Explore Lake Te Anau and its surroundings. Visit the Te Anau Bird Sanctuary to see native New Zealand birds. Overnight stay in Te Anau.",
        "Day 2: Milford Sound Cruise & Scenic Drive. Breakfast at the hotel. Enjoy a scenic drive through Fiordland National Park, stopping at Mirror Lakes. Take a Milford Sound cruise to experience waterfalls, dolphins, and seals. Visit The Chasm, a powerful waterfall cutting through rock formations. Return to Te Anau for an overnight stay.",
        "Day 3: Doubtful Sound Wilderness Tour. Breakfast at the hotel. Take a boat across Lake Manapouri and a coach over Wilmot Pass. Experience the remote and pristine Doubtful Sound on a nature cruise. Enjoy the serene fjords, unique marine life, and breathtaking views. Return to Te Anau for an overnight stay.",
        "Day 4: Glowworm Caves & Departure. Breakfast at the hotel. Visit the Te Anau Glowworm Caves for a magical underground experience. Free time for last-minute sightseeing or relaxation. Transfer back to Queenstown for departure."},
                    WhatToDo = new List<string> {  "Explore the stunning fjord with cascading waterfalls and wildlife on a Milford Sound cruise",
        "Experience the serene and untouched natural paradise of Doubtful Sound on a wilderness tour",
        "Witness thousands of glowworms in an underground wonderland at Te Anau Glowworm Caves",
        "Enjoy the beautiful reflective Mirror Lakes along the scenic drive to Milford Sound",
        "Relax and explore the largest lake in the South Island, Lake Te Anau",
        "Hike the Key Summit Track for panoramic views of Fiordland National Park",
        "Discover the remote and spectacular Hollyford Valley, rich in history and wildlife"},
                    Places = new List<string> {"Fiordland",
        "Milford Sound",
        "Doubtful Sound",
        "Te Anau",
        "Te Anau Bird Sanctuary",
        "Mirror Lakes",
        "Lake Te Anau",
        "Fiordland National Park",
        "Key Summit Track",
        "Hollyford Valley"},
                    Price = 1700,
                    Inclusions = new List<string> { "Accommodation for 4 nights in 4-star hotels",
        "Daily breakfast",
        "Airport transfers",
        "Guided tours of all mentioned attractions",
        "Comfortable transportation for all intercity travel"},
                    Exclusions = new List<string> { "International & domestic airfare",
        "Personal expenses such as shopping, tips, and meals not mentioned",
        "Travel insurance",
        "Additional activities not mentioned in the itinerary",
        "Entry tickets for optional attractions"},
                },
                new Destination
                {
                    Name ="Southland",
                    ImageUrls = new List<string> {"/assets/img/destination/Southland.webp",
        "/assets/img/destination/Southland1.webp",
        "/assets/img/destination/Southland2.webp",
        "/assets/img/destination/Southland3.webp",
        "/assets/img/destination/Southland4.webp"},
                    Description =  "Southland, located at the southernmost part of New Zealand, is known for its rugged landscapes, stunning coastal scenery, and rich wildlife. From the breathtaking Catlins to the charming town of Invercargill and the untouched beauty of Stewart Island, this tour offers an unforgettable adventure through one of New Zealand's most scenic regions.",
                    Itinerary = new List<string> { "Day 1: Arrival & Invercargill Exploration. Arrive in Invercargill and transfer to the hotel. Visit Bill Richardson Transport World. Explore Queens Park, a beautiful botanical garden. Enjoy local cuisine in the city center. Overnight stay in Invercargill.",
        "Day 2: The Catlins Scenic Adventure. Breakfast at the hotel. Travel to The Catlins and explore Nugget Point Lighthouse. Visit Purakaunui Falls, one of New Zealand's most picturesque waterfalls. Explore Curio Bay and see the fossilized forest and Hector's dolphins. Return to Invercargill for an overnight stay.",
        "Day 3: Stewart Island & Rakiura National Park. Breakfast at the hotel. Take a ferry to Stewart Island. Explore Rakiura National Park and enjoy scenic walking trails. Visit the Oban township and enjoy fresh seafood. Return to Invercargill for an overnight stay.",
        "Day 4: Bluff & Departure. Breakfast at the hotel. Travel to Bluff and visit Stirling Point, the iconic southernmost landmark. Enjoy fresh Bluff oysters (seasonal availability). Free time for last-minute sightseeing or shopping. Transfer to the airport for departure."},
                    WhatToDo = new List<string> {"Explore the rugged landscapes and stunning coastal scenery of Southland",
        "Visit The Catlins for waterfalls, caves, and wildlife",
        "Admire the panoramic ocean views from Nugget Point Lighthouse",
        "Discover Curio Bay's rare Hector's dolphins and ancient fossilized forests",
        "Experience the charm of Invercargill, Southland's largest city",
        "Visit Bill Richardson Transport World, a world-class vintage vehicle museum",
        "Take a ferry to Stewart Island and explore Rakiura National Park",
        "Enjoy fresh seafood in the Oban township on Stewart Island",
        "Visit Stirling Point, the iconic southernmost landmark in Bluff"},
                    Places = new List<string> {"Southland",
        "The Catlins",
        "Nugget Point Lighthouse",
        "Curio Bay",
        "Invercargill",
        "Bill Richardson Transport World",
        "Stewart Island",
        "Rakiura National Park",
        "Bluff"},
                    Price = 1600,
                    Inclusions = new List<string> {  "Accommodation for 4 nights in 4-star hotels",
        "Daily breakfast",
        "Airport transfers",
        "Guided tours of all mentioned attractions",
        "Comfortable transportation for all intercity travel"},
                    Exclusions = new List<string> { "International & domestic airfare",
        "Personal expenses such as shopping, tips, and meals not mentioned",
        "Travel insurance",
        "Additional activities not mentioned in the itinerary",
        "Entry tickets for optional attractions"},
                },
                new Destination
                {
                    Name ="Clutha",
                    ImageUrls = new List<string> { "/assets/img/destination/Clutha.webp",
        "/assets/img/destination/Clutha1.webp",
        "/assets/img/destination/Clutha2.webp",
        "/assets/img/destination/Clutha3.webp",
        "/assets/img/destination/Clutha4.webp"},
                    Description ="Clutha, located in the Otago region of New Zealand, is a scenic paradise with rugged coastlines, beautiful waterfalls, and rich historical heritage. From the iconic Nugget Point Lighthouse to the stunning Balclutha Bridge and the tranquil beauty of the Clutha River, this tour offers an authentic and off-the-beaten-path experience in New Zealand's southern region.",
                    Itinerary = new List<string> { "Day 1: Arrival & Balclutha Exploration. Arrive in Balclutha and transfer to the hotel. Explore Balclutha Bridge and Clutha River walks. Visit local cafes and restaurants for an authentic culinary experience. Overnight stay in Balclutha.",
        "Day 2: Coastal Wonders & Nugget Point. Breakfast at the hotel. Visit Nugget Point Lighthouse for breathtaking views. Explore Kaka Point and enjoy its sandy beaches. Stop at Jacks Blowhole to witness the power of the ocean. Return to Balclutha for an overnight stay.",
        "Day 3: Waterfalls & Scenic Walks. Breakfast at the hotel. Visit the famous Purakaunui Falls for a nature-filled adventure. Explore Tunnel Beach and admire its natural rock formations. Take a scenic drive through the Clutha region, stopping at viewpoints. Return to Balclutha for an overnight stay.",
        "Day 4: Historic Lawrence & Departure. Breakfast at the hotel. Travel to Lawrence, a historic gold-mining town. Explore the Gabriel's Gully heritage site. Free time for last-minute sightseeing or shopping. Transfer to the airport for departure."},
                    WhatToDo = new List<string> {"Visit Nugget Point Lighthouse for breathtaking coastal views",
        "Explore Kaka Point and its sandy beaches",
        "Witness the power of the ocean at Jacks Blowhole",
        "Discover the multi-tiered Purakaunui Falls in native forest",
        "Admire natural rock formations at Tunnel Beach",
        "Take scenic walks along the Clutha River",
        "Explore the historic gold-mining town of Lawrence and Gabriel's Gully heritage site"},
                    Places = new List<string> {"Clutha",
        "Nugget Point Lighthouse",
        "Purakaunui Falls",
        "Jacks Blowhole",
        "Tunnel Beach",
        "Clutha River",
        "Balclutha Bridge",
        "Kaka Point",
        "Lawrence",
        "Gabriel's Gully"},
                    Price = 1500,
                    Inclusions = new List<string> {"Accommodation for 4 nights in 4-star hotels",
        "Daily breakfast",
        "Airport transfers",
        "Guided tours of all mentioned attractions",
        "Comfortable transportation for all intercity travel"},
                    Exclusions = new List<string> { "International & domestic airfare",
        "Personal expenses such as shopping, tips, and meals not mentioned",
        "Travel insurance",
        "Additional activities not mentioned in the itinerary",
        "Entry tickets for optional attractions"},
                },
            };
            if (!string.IsNullOrEmpty(searchQuery))
            {
                CountryDestinations = CountryDestinations
                  .Where(d => d.Name.Equals(searchQuery, StringComparison.OrdinalIgnoreCase))
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
