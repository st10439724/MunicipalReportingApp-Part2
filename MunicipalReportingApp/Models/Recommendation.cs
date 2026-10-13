namespace MunicipalReportingApp.Models
{
    
    /// We want to track accordingly 
    
    public sealed record Recommendation(EventItem Event, int Score, string Reason);
}
