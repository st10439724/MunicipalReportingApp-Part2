using System.Collections.Generic;
using System.Linq;
using MunicipalReportingApp.Models;

namespace MunicipalReportingApp.Services
{
    
    /// Remember what the user has done during visit to the Events page.
    /// A new instance is created each time the page opens.
    ///We have important neew additions:
    /// "Previous" list       
    /// "Recently viewed" list.
    
    public sealed class UserActivityTracker
    {
        private const int MaxRecent = 5;

        private readonly Stack<SearchCriteria> _searchBackStack = new();
        private readonly Queue<SearchCriteria> _recentSearches = new();
        private readonly Stack<EventItem> _recentlyViewed = new();
        public bool CanGoBack => _searchBackStack.Count > 1;

        public void PushSearch(SearchCriteria criteria)
        {
            
            if (_searchBackStack.Count > 0 && _searchBackStack.Peek() == criteria) return;

            _searchBackStack.Push(criteria);

            if (criteria.IsEmpty) return;
            if (_recentSearches.Count > 0 && _recentSearches.Last() == criteria) return;

            _recentSearches.Enqueue(criteria);
            while (_recentSearches.Count > MaxRecent)
            {
                _recentSearches.Dequeue();
            }
        }

       
        public SearchCriteria? GoBack()
        {
            if (!CanGoBack) return null;

            _searchBackStack.Pop();          
            return _searchBackStack.Peek();  
        }

        public IEnumerable<SearchCriteria> RecentSearches => _recentSearches.Reverse();

        public void AddViewed(EventItem item)
        {
            
            var kept = _recentlyViewed.Reverse().Where(e => e.Id != item.Id).ToList();
            kept.Add(item);

            if (kept.Count > MaxRecent)
            {
                kept = kept.Skip(kept.Count - MaxRecent).ToList();
            }

            _recentlyViewed.Clear();
            foreach (var e in kept)
            {
                _recentlyViewed.Push(e);   
            }
        }
        public IEnumerable<EventItem> RecentlyViewed => _recentlyViewed;
        
        public HashSet<int> ViewedIds => _recentlyViewed.Select(e => e.Id).ToHashSet();
    }
}
