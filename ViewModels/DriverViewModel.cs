using System.Windows.Input;
using TransiTrack.Models;
using TransiTrack.Services;

namespace TransiTrack.ViewModels;

public class DriverViewModel : BaseViewModel
{
    private readonly RealtimeTrackingService _trackingService;
    private bool _isDriving;
    private CancellationTokenSource? _gpsCts;

    public List<string> Agencies { get; } = new() { "TransCam Voyages", "Général Express", "Socatur Urbain", "Buca Voyages" };
    public List<string> Lines { get; } = new() { "L1 - Ndokoti / Akwa", "L4 - Bonanjo / PK14", "L10 - Ndokoti / Bonamoussadi" };

    public string SelectedAgency { get; set; } = "Socatur Urbain";
    public string SelectedLine { get; set; } = "L10 - Ndokoti / Bonamoussadi";
    public string Registration { get; set; } = string.Empty;

    public bool IsDriving
    {
        get => _isDriving;
        set
        {
            _isDriving = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(StatusText));
            OnPropertyChanged(nameof(ActionBtnText));
        }
    }

    public string StatusText => IsDriving ? "Course en cours (Position diffusée aux usagers)" : "Course arrêtée";
    public string ActionBtnText => IsDriving ? "Arrêter la course" : "Démarrer la course";

    public ICommand ToggleTripCommand { get; }

    public DriverViewModel(RealtimeTrackingService trackingService)
    {
        _trackingService = trackingService;
        ToggleTripCommand = new Command(async () => await ToggleTripAsync());
    }

    private async Task ToggleTripAsync()
    {
        if (string.IsNullOrWhiteSpace(Registration))
        {
            await Shell.Current.DisplayAlert("Information requise", "Veuillez saisir le matricule de votre bus.", "OK");
            return;
        }

        await _trackingService.ConnectAsync();

        if (IsDriving)
        {
            _gpsCts?.Cancel();
            await _trackingService.StopDriverTripAsync(Registration);
            IsDriving = false;
        }
        else
        {
            IsDriving = true;
            _gpsCts = new CancellationTokenSource();
            _ = Task.Run(() => BroadcastLocationLoopAsync(_gpsCts.Token));
        }
    }

    private async Task BroadcastLocationLoopAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            try
            {
                var location = await Geolocation.Default.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.High), token);
                if (location != null)
                {
                    var data = new VehiclePosition
                    {
                        VehicleRegistration = Registration,
                        AgencyName = SelectedAgency,
                        LineId = SelectedLine,
                        Latitude = location.Latitude,
                        Longitude = location.Longitude,
                        SpeedKmh = (location.Speed.HasValue && location.Speed.Value > 0) ? location.Speed.Value * 3.6 : 25.0
                    };

                    await _trackingService.SendDriverLocationAsync(data);
                }
            }
            catch
            {
                // Gestion des interruptions ou absence de signal GPS
            }

            await Task.Delay(4000, token); // Envoi toutes les 4 secondes
        }
    }
}