using System;
using System.Collections.Generic;

namespace MunicipalReportingApp.Models
{
    
    public sealed record SearchCriteria(
        string Keyword = "",
        string Category = "",
        DateTime? From = null,
        DateTime? To = null)
    {
       
        public bool IsEmpty =>
            string.IsNullOrWhiteSpace(Keyword) &&
            string.IsNullOrWhiteSpace(Category) &&
            From == null &&
            To == null;

        
        public string Describe()
        {
            var parts = new List<string>();

            if (!string.IsNullOrWhiteSpace(Keyword)) parts.Add($"\"{Keyword.Trim()}\"");
            if (!string.IsNullOrWhiteSpace(Category)) parts.Add(Category);
            if (From.HasValue) parts.Add($"from {From.Value:d MMM}");
            if (To.HasValue) parts.Add($"until {To.Value:d MMM}");

            return parts.Count == 0 ? "All upcoming events" : string.Join(" | ", parts);
        }

        public override string ToString() => Describe();
    }
}
