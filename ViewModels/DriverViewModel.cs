using System.Windows.Input;
using TransiTrack.Models;
using TransiTrack.Services;

namespace TransiTrack.ViewModels;

public class DriverViewModel : BaseViewModel
{
    private readonly RealtimeTrackingService _trackingService;
    private bool _isAuthenticated;
    private bool _isDriving;
    private CancellationTokenSource? _gpsCts;

    // Champs de connexion chauffeur
    public string DriverCode { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    // Configuration de course
    public List<string> Agencies { get; } = new() { "Socatur Urbain", "TransCam Voyages", "Général Express" };
    public List<string> Lines { get; } = new() { "L1 - Ndokoti / Akwa", "L4 - Bonanjo / PK14", "L10 - Ndokoti / Bonamoussadi" };

    public string SelectedAgency { get; set; } = "Socatur Urbain";
    public string SelectedLine { get; set; } = "L10 - Ndokoti / Bonamoussadi";
    public string Registration { get; set; } = string.Empty;

    public bool IsAuthenticated
    {
        get => _isAuthenticated;
        set
        {
            SetProperty(ref _isAuthenticated, value);
            OnPropertyChanged(nameof(IsNotAuthenticated));
        }
    }

    public bool IsNotAuthenticated => !IsAuthenticated;

    public bool IsDriving
    {
        get => _isDriving;
        set
        {
            SetProperty(ref _isDriving, value);
            OnPropertyChanged(nameof(StatusText));
            OnPropertyChanged(nameof(ActionBtnText));
        }
    }

    public string StatusText => IsDriving ? "Course en cours (Position GPS diffusée)" : "Course arrêtée";
    public string ActionBtnText => IsDriving ? "Arrêter la course" : "Démarrer la course";

    public ICommand LoginDriverCommand { get; }
    public ICommand LogoutDriverCommand { get; }
    public ICommand ToggleTripCommand { get; }

    public DriverViewModel(RealtimeTrackingService trackingService)
    {
        _trackingService = trackingService;

        LoginDriverCommand = new Command(async () => await LoginAsync());
        LogoutDriverCommand = new Command(() => Logout());
        ToggleTripCommand = new Command(async () => await ToggleTripAsync());
    }

    private async Task LoginAsync()
    {
        if (string.IsNullOrWhiteSpace(DriverCode) || string.IsNullOrWhiteSpace(Password))
        {
            if (Shell.Current != null)
                await Shell.Current.DisplayAlert("Erreur", "Veuillez saisir votre code chauffeur et mot de passe.", "OK");
            return;
        }

        // Vérification locale ou via API backend
        IsBusy = true;
        try
        {
            // Simulation d'authentification valide pour test (ex: code non vide)
            await Task.Delay(500);
            IsAuthenticated = true;
            Registration = DriverCode.ToUpper();
            OnPropertyChanged(nameof(Registration));
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void Logout()
    {
        if (IsDriving)
        {
            _gpsCts?.Cancel();
            IsDriving = false;
        }

        IsAuthenticated = false;
        DriverCode = string.Empty;
        Password = string.Empty;
        OnPropertyChanged(nameof(DriverCode));
        OnPropertyChanged(nameof(Password));
    }

    private async Task ToggleTripAsync()
    {
        if (string.IsNullOrWhiteSpace(Registration))
        {
            if (Shell.Current != null)
                await Shell.Current.DisplayAlert("Information requise", "Veuillez renseigner le matricule du bus.", "OK");
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
            _ = Task.Run(() => BroadcastLoopAsync(_gpsCts.Token));
        }
    }

    private async Task BroadcastLoopAsync(CancellationToken token)
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
                        SpeedKmh = (location.Speed.HasValue && location.Speed.Value > 0) ? location.Speed.Value * 3.6 : 30.0
                    };

                    await _trackingService.SendDriverLocationAsync(data);
                }
            }
            catch { }

            await Task.Delay(4000, token);
        }
    }
}