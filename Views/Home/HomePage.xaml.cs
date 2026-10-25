using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using TransiTrack.Models;
using TransiTrack.Services;
using TransiTrack.ViewModels;

namespace TransiTrack.Views.Home;

public partial class HomePage : ContentPage
{
    private readonly RealtimeTrackingService _trackingService;
    private readonly HomeMapViewModel _viewModel;
    private readonly Dictionary<string, Pin> _pins = new();

    public HomePage(HomeMapViewModel vm, RealtimeTrackingService trackingService)
    {
        InitializeComponent();
        BindingContext = _viewModel = vm;
        _trackingService = trackingService;

        _trackingService.OnVehicleReceived += HandleVehicleReceived;
        _trackingService.OnVehicleRemoved += HandleVehicleRemoved;
        _trackingService.OnIncidentReceived += HandleIncidentReceived;

        Task.Run(async () => await _trackingService.ConnectAsync());
    }

    private void HandleVehicleReceived(VehiclePosition vehicle)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            var location = new Location(vehicle.Latitude, vehicle.Longitude);

            if (_pins.TryGetValue(vehicle.VehicleRegistration, out var existingPin))
            {
                existingPin.Location = location;
                existingPin.Address = $"{vehicle.AgencyName} - {vehicle.SpeedKmh:F0} km/h";
            }
            else
            {
                var pin = new Pin
                {
                    Label = $"Bus {vehicle.VehicleRegistration}",
                    Address = $"{vehicle.AgencyName} - {vehicle.SpeedKmh:F0} km/h",
                    Type = PinType.SearchResult,
                    Location = location
                };

                pin.MarkerClicked += (s, e) =>
                {
                    _viewModel.SelectedVehicle = vehicle;
                };

                _pins[vehicle.VehicleRegistration] = pin;
                BusMap.Pins.Add(pin);
            }
        });
    }

    private void HandleVehicleRemoved(string registration)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            if (_pins.TryGetValue(registration, out var pin))
            {
                BusMap.Pins.Remove(pin);
                _pins.Remove(registration);
            }
        });
    }

    private void HandleIncidentReceived(TrafficIncident incident)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            var incidentPin = new Pin
            {
                Label = $"⚠️ {incident.IncidentType}",
                Address = incident.LocationName,
                Type = PinType.Generic,
                Location = new Location(incident.Latitude, incident.Longitude)
            };
            BusMap.Pins.Add(incidentPin);
        });
    }
}