using System;
using System.Collections.Generic;

namespace MunicipalReportingApp.Models
{
   
    public class IssueReport
    {
        
        public int Id { get; set; }

        
        public string Location { get; set; } = string.Empty;

        public string StreetName { get; set; } = string.Empty;

        
        public string Category { get; set; } = string.Empty;

       
        public string Description { get; set; } = string.Empty;

        
        public List<string> AttachmentPaths { get; set; } = new();

        
        public DateTime DateReported { get; set; } = DateTime.Now;

        
        public string Status { get; set; } = "Submitted";

        public override string ToString()
        {
            return $"[#{Id}] {Category} @ {Location} - {Status} ({DateReported:g})";
        }
    }
}
