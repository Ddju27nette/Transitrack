namespace TransiTrack.Api.Models;

public class VehicleTelemetry
{
    public string VehicleRegistration { get; set; } = string.Empty; // Matricule (ex: LT-458-EB)
    public string AgencyName { get; set; } = string.Empty;         // Agence (ex: TransCam)
    public string LineId { get; set; } = string.Empty;             // Ligne (ex: L10)
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double SpeedKmh { get; set; }                           // Vitesse mesurée en km/h
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}