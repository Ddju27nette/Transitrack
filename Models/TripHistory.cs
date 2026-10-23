namespace TransiTrack.Models;

public class TripHistory
{
    public string Origin { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public string DateFormatted { get; set; } = string.Empty;
    public string LineUsed { get; set; } = string.Empty;
    public string Duration { get; set; } = string.Empty;
}