using System;

namespace MunicipalReportingApp.Models
{
    /// 
    /// Our aanouncements are done here i have attempted counts 
    public enum AnnouncementPriority
    {
        Urgent = 1,
        Important = 2,
        General = 3
    }

    
    public class Announcement
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public AnnouncementPriority Priority { get; set; } = AnnouncementPriority.General;
        public DateTime DatePosted { get; set; } = DateTime.Now;
    }
}
