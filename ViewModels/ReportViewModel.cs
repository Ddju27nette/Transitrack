using System.Windows.Input;
using TransiTrack.Models;
using TransiTrack.Services;

namespace TransiTrack.ViewModels;

public class ReportViewModel : BaseViewModel
{
    private readonly RealtimeTrackingService _trackingService;

    public List<string> IncidentTypes { get; } = new()
    {
        "Embouteillage important",
        "Panne de bus sur la voie",
        "Accident ralentissant la circulation",
        "Arrêt surchargé"
    };

    public string SelectedType { get; set; } = "Embouteillage important";
    public string LocationName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public ICommand SubmitReportCommand { get; }

    public ReportViewModel(RealtimeTrackingService trackingService)
    {
        _trackingService = trackingService;
        SubmitReportCommand = new Command(async () => await SubmitAsync());
    }

    private async Task SubmitAsync()
    {
        if (string.IsNullOrWhiteSpace(LocationName))
        {
            await Shell.Current.DisplayAlert("Précision", "Veuillez renseigner le lieu ou l'axe concerné.", "OK");
            return;
        }

        await _trackingService.ConnectAsync();

        // Récupération de la position courante pour l'incident
        var location = await Geolocation.Default.GetLastKnownLocationAsync();

        var incident = new TrafficIncident
        {
            IncidentType = SelectedType,
            LocationName = LocationName,
            Description = Description,
            Latitude = location?.Latitude ?? 4.0511,
            Longitude = location?.Longitude ?? 9.7085
        };

        await _trackingService.SendIncidentAsync(incident);

        await Shell.Current.DisplayAlert("Signalement transmis", "Votre alerte a été partagée aux usagers et aux agences.", "Merci");
        LocationName = string.Empty;
        Description = string.Empty;
        OnPropertyChanged(nameof(LocationName));
        OnPropertyChanged(nameof(Description));
    }
}