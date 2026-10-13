using System;

namespace MunicipalReportingApp.Models
{
    
    public class EventItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;

        
        public DateTime Date { get; set; }

        public string Venue { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        ///Used by the "recently viewed" list boxes
        public override string ToString()
        {
            return $"{Date:dd MMM} - {Title}";
        }
    }
}
