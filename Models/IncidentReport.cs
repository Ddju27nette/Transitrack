namespace TransiTrack.Models;

public class IncidentReport
{
    public string IncidentType { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
    public DateTime ReportedAt { get; set; } = DateTime.Now;
}