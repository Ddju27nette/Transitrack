using Microsoft.AspNetCore.SignalR.Client;
using TransiTrack.Models;

namespace TransiTrack.Services;

public class RealtimeTrackingService
{
    private HubConnection? _connection;
    public event Action<VehiclePosition>? OnVehicleReceived;
    public event Action<string>? OnVehicleRemoved;
    public event Action<TrafficIncident>? OnIncidentReceived;

    public async Task ConnectAsync()
    {
        if (_connection != null && _connection.State == HubConnectionState.Connected)
            return;

        // 10.0.2.2 redirige vers localhost sous émulateur Android
        string url = DeviceInfo.Platform == DevicePlatform.Android
            ? "http://10.0.2.2:5000/hubs/transit"
            : "http://localhost:5000/hubs/transit";

        _connection = new HubConnectionBuilder()
            .WithUrl(url)
            .WithAutomaticReconnect()
            .Build();

        _connection.On<VehiclePosition>("VehiclePositionReceived", vehicle =>
        {
            OnVehicleReceived?.Invoke(vehicle);
        });

        _connection.On<string>("VehicleRemoved", reg =>
        {
            OnVehicleRemoved?.Invoke(reg);
        });

        _connection.On<TrafficIncident>("IncidentAlertReceived", inc =>
        {
            OnIncidentReceived?.Invoke(inc);
        });

        await _connection.StartAsync();
    }

    public async Task SendDriverLocationAsync(VehiclePosition position)
    {
        if (_connection?.State == HubConnectionState.Connected)
        {
            await _connection.SendAsync("UpdatePosition", position);
        }
    }

    public async Task StopDriverTripAsync(string registration)
    {
        if (_connection?.State == HubConnectionState.Connected)
        {
            await _connection.SendAsync("StopTrip", registration);
        }
    }

    public async Task SendIncidentAsync(TrafficIncident incident)
    {
        if (_connection?.State == HubConnectionState.Connected)
        {
            await _connection.SendAsync("SendIncident", incident);
        }
    }
}