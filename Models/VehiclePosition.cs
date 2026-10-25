namespace TransiTrack.Models;

public class VehiclePosition
{
    public string VehicleRegistration { get; set; } = string.Empty;
    public string AgencyName { get; set; } = string.Empty;
    public string LineId { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double SpeedKmh { get; set; }
}