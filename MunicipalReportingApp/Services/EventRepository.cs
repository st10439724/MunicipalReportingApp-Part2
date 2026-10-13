using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MunicipalReportingApp.Models;

namespace MunicipalReportingApp.Services
{
    
    ///IssueRepository, so every form sees the same data.
    
  
    public sealed class EventRepository
    {
        
        public const string AllCategories = "All categories";

        private static readonly Lazy<EventRepository> _instance = new(() => new EventRepository());
        public static EventRepository Instance => _instance.Value;

        
        private readonly SortedDictionary<DateTime, List<EventItem>> _eventsByDate = new();
        private readonly Dictionary<int, EventItem> _eventsById = new();
        private readonly Hashtable _eventsByCategory = new(StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<string> _categories = new(StringComparer.OrdinalIgnoreCase);
        private readonly SortedSet<DateTime> _eventDates = new();
        private readonly List<Announcement> _announcements = new();

        private int _nextEventId = 1;
        private int _nextAnnouncementId = 1;

        private EventRepository()
        {
            SeedSampleData();
        }

       
        public EventItem AddEvent(string title, string category, DateTime date, string venue, string description)
        {
            var item = new EventItem
            {
                Id = _nextEventId++,
                Title = title,
                Category = category,
                Date = date,
                Venue = venue,
                Description = description
            };

            _eventsById[item.Id] = item;

            // Group by calendar day inside the sorted dictionary.
            var day = item.Date.Date;
            if (!_eventsByDate.TryGetValue(day, out var dayList))
            {
                dayList = new List<EventItem>();
                _eventsByDate[day] = dayList;
            }
            dayList.Add(item);
            dayList.Sort((a, b) => a.Date.CompareTo(b.Date));

            // Sets: duplicates are ignored automatically.
            _eventDates.Add(day);
            _categories.Add(item.Category);

            // Hashtable category index (values are List<int> of event Ids).
            var ids = _eventsByCategory[item.Category] as List<int>;
            if (ids == null)
            {
                ids = new List<int>();
                _eventsByCategory[item.Category] = ids;
            }
            ids.Add(item.Id);

            return item;
        }

        public Announcement AddAnnouncement(string title, string message, AnnouncementPriority priority, DateTime posted)
        {
            var announcement = new Announcement
            {
                Id = _nextAnnouncementId++,
                Title = title,
                Message = message,
                Priority = priority,
                DatePosted = posted
            };
            _announcements.Add(announcement);
            return announcement;
        }

        public List<string> GetCategories()
        {
            return _categories.OrderBy(c => c, StringComparer.OrdinalIgnoreCase).ToList();
        }

        public EventItem? GetEvent(int id)
        {
            return _eventsById.TryGetValue(id, out var item) ? item : null;
        }

        
        public List<EventItem> GetUpcoming()
        {
            return _eventsByDate
                .Where(kv => kv.Key >= DateTime.Today)
                .SelectMany(kv => kv.Value)
                .ToList();
        }

        
        /// Announcements ordered Urgent -> Important -> General, newest first
        /// I want consistency so that i can add more function for Task 3
        public List<Announcement> GetAnnouncementsByPriority()
        {
            var queue = new PriorityQueue<Announcement, (int Level, long NegativeTicks)>();

            foreach (var a in _announcements)
            {
                queue.Enqueue(a, ((int)a.Priority, -a.DatePosted.Ticks));
            }

            var ordered = new List<Announcement>();
            while (queue.Count > 0)
            {
                ordered.Add(queue.Dequeue());
            }
            return ordered;
        }

        
        
        public List<EventItem> Search(string? keyword, string? category, DateTime? from, DateTime? to)
        {
            
            var low = DateTime.Today;
            if (from.HasValue && from.Value.Date > low)
            {
                low = from.Value.Date;
            }
            var high = to.HasValue ? to.Value.Date : DateTime.MaxValue.Date;

            if (low > high)
            {
                return new List<EventItem>();
            }

          
            var datesInRange = _eventDates.GetViewBetween(low, high);

          
            var idsOnDates = new HashSet<int>();
            foreach (var day in datesInRange)
            {
                foreach (var e in _eventsByDate[day])
                {
                    idsOnDates.Add(e.Id);
                }
            }

          
            if (!string.IsNullOrWhiteSpace(category) && category != AllCategories)
            {
                var idsInCategory = new HashSet<int>(
                    _eventsByCategory[category] as List<int> ?? new List<int>());
                idsOnDates.IntersectWith(idsInCategory);
            }

            
            var words = (keyword ?? string.Empty)
                .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            var results = new List<EventItem>();
            foreach (var day in datesInRange)   
            {
                foreach (var e in _eventsByDate[day])
                {
                    if (!idsOnDates.Contains(e.Id)) continue;
                    if (words.Length > 0 && !MatchesAllWords(e, words)) continue;
                    results.Add(e);
                }
            }
            return results;
        }

        private static bool MatchesAllWords(EventItem e, string[] words)
        {
            var text = $"{e.Title} {e.Category} {e.Venue} {e.Description}";
            foreach (var word in words)
            {
                if (text.IndexOf(word, StringComparison.OrdinalIgnoreCase) < 0)
                {
                    return false;
                }
            }
            return true;
        }

        // ------------------------------------------------------------------
        //  Sampled data
        
        private void SeedSampleData()
        {
            var today = DateTime.Today;

            const string culture = "Culture & Arts";
            const string sport = "Sports & Recreation";
            const string community = "Community";
            const string health = "Health & Wellness";
            const string education = "Education & Skills";
            const string business = "Business & Markets";

            AddEvent("Heritage Evening Walk", culture, today.AddDays(2).AddHours(17),
                "Big Hole Precinct, Kimberley",
                "A guided evening walk through the historic centre, with stories about the early days of the city and its people.");

            AddEvent("Community Clean-up Day", community, today.AddDays(3).AddHours(9),
                "Galeshewe Community Hall",
                "Join your neighbours to clean up parks and streets. Gloves and refuse bags are provided. Bring water and a hat.");

            AddEvent("Fun Run & Walk (5 km)", sport, today.AddDays(4).AddHours(7),
                "Local Recreation Grounds",
                "A family-friendly 5 km run or walk. All fitness levels welcome, with water points along the route.");

            AddEvent("Free Health Screening Day", health, today.AddDays(5).AddHours(9),
                "Community Clinic Hall",
                "Free blood pressure, blood sugar and BMI checks, plus advice from clinic nurses. No booking needed.");

            AddEvent("CV Writing & Job Readiness Workshop", education, today.AddDays(6).AddHours(14),
                "Kimberley Public Library",
                "Learn how to write a strong CV, prepare for interviews and find job opportunities. Bring a copy of your current CV if you have one.");

            AddEvent("Saturday Farmers & Crafts Market", business, today.AddDays(7).AddHours(8),
                "Town Hall Square",
                "Fresh produce, baked goods and local crafts from small traders. Live music from mid-morning.");

            AddEvent("Youth Talent Showcase", culture, today.AddDays(9).AddHours(18).AddMinutes(30),
                "Town Hall, Kimberley",
                "Local young people take the stage with music, poetry and dance. Entry is free, and everyone is welcome.");

            AddEvent("Ward Committee Public Meeting", community, today.AddDays(10).AddHours(17),
                "Galeshewe Community Hall",
                "Hear updates on service delivery in your ward and raise your questions with ward committee members.");

            AddEvent("Community Netball Tournament", sport, today.AddDays(12).AddHours(8),
                "Local Recreation Grounds",
                "Teams from across the municipality compete in a friendly one-day tournament. Spectators welcome.");

            AddEvent("Small Business Starter Workshop", business, today.AddDays(14).AddHours(10),
                "Civic Centre",
                "Practical advice on registering a business, basic bookkeeping and finding funding for new entrepreneurs.");

            AddEvent("Mobile Clinic: Vaccination & Wellness Check", health, today.AddDays(16).AddHours(11),
                "Galeshewe Community Hall",
                "A mobile clinic offering routine vaccinations and wellness checks for adults and children.");

            AddEvent("Coding for Kids (Ages 9-13)", education, today.AddDays(19).AddHours(15),
                "Kimberley Public Library",
                "A beginner-friendly introduction to computers and coding through games and simple projects.");

            AddEvent("Open-Air Music Evening", culture, today.AddDays(21).AddHours(17),
                "Big Hole Precinct, Kimberley",
                "An evening of live local music in the open air. Bring a blanket or chair and enjoy food from local vendors.");

            AddEvent("Recycling & Waste Awareness Drive", community, today.AddDays(26).AddHours(8),
                "Town Hall Square",
                "Learn how to separate waste, reduce what goes to landfill and earn rewards for recycling.");

            AddEvent("Fun Soccer Day for Schools", sport, today.AddDays(30).AddHours(9),
                "Local Recreation Grounds",
                "Primary and high school teams play friendly matches in a day of sport and school spirit.");

            AddEvent("Women in Business Networking Breakfast", business, today.AddDays(35).AddHours(7).AddMinutes(30),
                "Civic Centre",
                "Meet other women entrepreneurs, share experiences and hear from successful local business owners.");

            AddEvent("Adult Basic Education Open Day", education, today.AddDays(40).AddHours(9),
                "Kimberley Public Library",
                "Find out about adult literacy and numeracy classes and how to register for the next intake.");

            // ---- Announcements ----
            AddAnnouncement(
                "Planned water supply interruption",
                $"Maintenance work will interrupt water supply in selected suburbs on {today.AddDays(2):dddd d MMMM}, " +
                "from 08:00 to 16:00. Please store enough water in advance. The affected areas will be confirmed by the municipality.",
                AnnouncementPriority.Urgent, today.AddDays(-1));

            AddAnnouncement(
                "Refuse collection schedule update",
                "Refuse collection days may change during the coming public holiday. Please check the updated schedule and " +
                "place your bins out by 06:00 on your collection day.",
                AnnouncementPriority.Important, today.AddDays(-2));

            AddAnnouncement(
                "Municipal offices: holiday opening hours",
                "Municipal offices will close early on the day before a public holiday. Pay accounts online or at a " +
                "participating retailer to avoid queues.",
                AnnouncementPriority.Important, today.AddDays(-3));

            AddAnnouncement(
                "Library extended hours for exam season",
                "Kimberley Public Library will stay open later on weekdays to give learners and students a quiet place to study.",
                AnnouncementPriority.General, today.AddDays(-4));

            AddAnnouncement(
                "Report potholes and streetlights online",
                "See a pothole, a broken streetlight or a water leak? Use the Report Issues option in this app " +
                "to tell the municipality where it is.",
                AnnouncementPriority.General, today.AddDays(-5));
        }
    }
}
