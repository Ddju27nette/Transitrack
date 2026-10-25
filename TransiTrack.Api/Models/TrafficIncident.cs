namespace TransiTrack.Api.Models;

public class TrafficIncident
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string IncidentType { get; set; } = string.Empty; // Embouteillage, Accident, Ralentissement
    public string LocationName { get; set; } = string.Empty; // Ex: Rond-point Ndokoti
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime ReportedAt { get; set; } = DateTime.UtcNow;
}