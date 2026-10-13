using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using MunicipalReportingApp.Models;

namespace MunicipalReportingApp.Services
{
    
    /// Recommends events based on what the user searches for and opens.
    
    
    
    public sealed class RecommendationEngine
    {
        private static readonly Lazy<RecommendationEngine> _instance = new(() => new RecommendationEngine());
        public static RecommendationEngine Instance => _instance.Value;

        private const int SearchCategoryWeight = 2;
        private const int ViewCategoryWeight = 3;
        private const int ResultCategoryWeight = 1;

        
        private static readonly HashSet<string> StopWords = new(StringComparer.OrdinalIgnoreCase)
        {
            "the", "and", "for", "with", "from", "that", "this", "our", "all", "new", "day", "free"
        };

        private readonly Dictionary<string, int> _categoryScores = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, int> _keywordCounts = new(StringComparer.OrdinalIgnoreCase);
        private readonly string _profileFile;

        private RecommendationEngine()
        {
            _profileFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "user_profile.json");
            LoadProfile();
        }

        
        public bool HasHistory => _categoryScores.Count > 0 || _keywordCounts.Count > 0;

        // ------------------------------------------------------------------
        //  Learning from the the individual
        

        public void RecordSearch(SearchCriteria criteria, IReadOnlyCollection<EventItem> results)
        {
            if (criteria.IsEmpty) return;

            if (!string.IsNullOrWhiteSpace(criteria.Category))
            {
                Bump(_categoryScores, criteria.Category, SearchCategoryWeight);
            }

            var words = Tokenize(criteria.Keyword);
            foreach (var word in words)
            {
                Bump(_keywordCounts, word, 1);
            }

            // A keyword search reveals interest in the kinds of events it found.
            if (words.Count > 0)
            {
                foreach (var category in results.Select(r => r.Category).Distinct())
                {
                    Bump(_categoryScores, category, ResultCategoryWeight);
                }
            }

            SaveProfile();
        }

        public void RecordView(EventItem item)
        {
            Bump(_categoryScores, item.Category, ViewCategoryWeight);

            foreach (var word in Tokenize(item.Title))
            {
                Bump(_keywordCounts, word, 1);
            }

            SaveProfile();
        }

       
        public void ResetProfile()
        {
            _categoryScores.Clear();
            _keywordCounts.Clear();
            SaveProfile();
        }

        // ------------------------------------------------------------------
        ///  Recommending
        

        
        
        public List<Recommendation> GetRecommendations(int count, ISet<int>? excludeIds = null)
        {
            var upcoming = EventRepository.Instance.GetUpcoming();

            
            var best = new PriorityQueue<Recommendation, (int Score, long NegativeTicks)>();
            var noHistoryMatch = new List<EventItem>();   

            foreach (var item in upcoming)
            {
                if (excludeIds != null && excludeIds.Contains(item.Id)) continue;

                int score = ScoreForUser(item, out string reason);
                if (score <= 0)
                {
                    noHistoryMatch.Add(item);
                    continue;
                }

                best.Enqueue(new Recommendation(item, score, reason), (score, -item.Date.Ticks));
                if (best.Count > count)
                {
                    best.Dequeue();
                }
            }

            var picked = new List<Recommendation>();
            while (best.Count > 0)
            {
                picked.Add(best.Dequeue());
            }

            picked = picked
                .OrderByDescending(r => r.Score)
                .ThenBy(r => r.Event.Date)
                .ToList();

            
            foreach (var item in noHistoryMatch)
            {
                if (picked.Count >= count) break;
                picked.Add(new Recommendation(item, 0, "Coming up soon"));
            }

            return picked;
        }

        
        /// Events related to one event: same category, shared title words,
       
        public List<Recommendation> GetRelated(EventItem selected, int count)
        {
            var selectedWords = Tokenize(selected.Title);
            var related = new List<Recommendation>();

            foreach (var item in EventRepository.Instance.GetUpcoming())
            {
                if (item.Id == selected.Id) continue;

                int score = 0;
                string reason = string.Empty;

                if (string.Equals(item.Category, selected.Category, StringComparison.OrdinalIgnoreCase))
                {
                    score += 3;
                    reason = $"Also in {item.Category}";
                }

                var sharedWord = selectedWords.FirstOrDefault(w =>
                    item.Title.Contains(w, StringComparison.OrdinalIgnoreCase));
                if (sharedWord != null)
                {
                    score += 2;
                    reason = $"Similar to \"{sharedWord}\"";
                }

                if (score > 0 && Math.Abs((item.Date - selected.Date).TotalDays) <= 14)
                {
                    score += 1;
                }

                if (score > 0)
                {
                    related.Add(new Recommendation(item, score, reason));
                }
            }

            return related
                .OrderByDescending(r => r.Score)
                .ThenBy(r => r.Event.Date)
                .Take(count)
                .ToList();
        }

        

        private int ScoreForUser(EventItem item, out string reason)
        {
            int score = 0;
            int categoryPoints = 0;
            reason = string.Empty;

            if (_categoryScores.TryGetValue(item.Category, out int categoryScore) && categoryScore > 0)
            {
                score += categoryScore;
                categoryPoints = categoryScore;
                reason = $"Because you like {item.Category}";
            }

            var text = $"{item.Title} {item.Description} {item.Category}";
            string? strongestWord = null;
            int strongestPoints = 0;

            foreach (var pair in _keywordCounts)
            {
                if (text.Contains(pair.Key, StringComparison.OrdinalIgnoreCase))
                {
                    int points = pair.Value * 2;
                    score += points;

                    if (points > strongestPoints)
                    {
                        strongestPoints = points;
                        strongestWord = pair.Key;
                    }
                }
            }

            // Explain the match with whichever signal contributed most.
            if (strongestWord != null && strongestPoints >= categoryPoints)
            {
                reason = $"Matches your interest in \"{strongestWord}\"";
            }

            
            if (score > 0)
            {
                int daysAway = (item.Date.Date - DateTime.Today).Days;
                if (daysAway <= 7) score += 2;
                else if (daysAway <= 14) score += 1;
            }

            return score;
        }

        
        public static List<string> Tokenize(string? text)
        {
            var words = new List<string>();
            if (string.IsNullOrWhiteSpace(text)) return words;

            var separators = new[] { ' ', ',', '.', ';', ':', '-', '&', '/', '(', ')', '\'', '"', '!', '?' };

            foreach (var raw in text.Split(separators, StringSplitOptions.RemoveEmptyEntries))
            {
                var word = raw.Trim().ToLowerInvariant();

                
                if (word.Length > 4 && word.EndsWith('s'))
                {
                    word = word.Substring(0, word.Length - 1);
                }

                if (word.Length >= 3 && !StopWords.Contains(word) && !words.Contains(word))
                {
                    words.Add(word);
                }
            }
            return words;
        }

        private static void Bump(Dictionary<string, int> scores, string key, int points)
        {
            scores[key] = scores.TryGetValue(key, out int current) ? current + points : points;
        }

        // ------------------------------------------------------------------
        //  Saving / loading the profiles
        // ------------------------------------------------------------------

        private sealed class ProfileData
        {
            public Dictionary<string, int> CategoryScores { get; set; } = new();
            public Dictionary<string, int> KeywordCounts { get; set; } = new();
        }

        private void SaveProfile()
        {
            try
            {
                var data = new ProfileData
                {
                    CategoryScores = new Dictionary<string, int>(_categoryScores),
                    KeywordCounts = new Dictionary<string, int>(_keywordCounts)
                };
                var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_profileFile, json);
            }
            catch
            {
                
            }
        }

        private void LoadProfile()
        {
            try
            {
                if (!File.Exists(_profileFile)) return;

                var data = JsonSerializer.Deserialize<ProfileData>(File.ReadAllText(_profileFile));
                if (data == null) return;

                foreach (var pair in data.CategoryScores) _categoryScores[pair.Key] = pair.Value;
                foreach (var pair in data.KeywordCounts) _keywordCounts[pair.Key] = pair.Value;
            }
            catch
            {
                
            }
        }
    }
}
