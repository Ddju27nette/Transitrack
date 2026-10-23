namespace TransiTrack.Models;

public class BusPassage
{
    public string LineCode { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public int MinutesRemaining { get; set; }
    public string CrowdLevel { get; set; } = "Faible"; // Faible, Modéré, Saturé
    public Color CrowdColor => CrowdLevel switch
    {
        "Faible" => Color.FromArgb("#34A853"),
        "Modéré" => Color.FromArgb("#FBBC04"),
        "Saturé" => Color.FromArgb("#EA4335"),
        _ => Color.FromArgb("#5F6368")
    };
}