using System.Collections.ObjectModel;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;
using TransiTrack.Models;
using TransiTrack.Services;

namespace TransiTrack.ViewModels;

public sealed class LiveFleetViewModel : BaseViewModel
{
    private readonly ITransitRealtimeClient _realtimeClient;
    private readonly ConcurrentDictionary<string, VehiclePosition> _pendingPositions = new();
    private readonly ConcurrentDictionary<string, byte> _pendingRemovals = new();
    private CancellationTokenSource? _sessionCancellation;
    private Task? _updatePump;
    private string _connectionStatus = "Connexion en attente";
    private string _errorMessage = string.Empty;

    public ObservableCollection<VehiclePosition> Vehicles { get; } = new();
    public Command RetryConnectionCommand { get; }

    public string ConnectionStatus
    {
        get => _connectionStatus;
        private set => SetProperty(ref _connectionStatus, value);
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        private set
        {
            if (SetProperty(ref _errorMessage, value))
                OnPropertyChanged(nameof(HasError));
        }
    }

    public bool HasError => !string.IsNullOrWhiteSpace(ErrorMessage);

    public LiveFleetViewModel(ITransitRealtimeClient realtimeClient)
    {
        _realtimeClient = realtimeClient;
        RetryConnectionCommand = new Command(async () => await StartAsync());
    }

    public async Task StartAsync()
    {
        if (_sessionCancellation is not null)
            return;

        var cancellation = new CancellationTokenSource();
        _sessionCancellation = cancellation;
        _updatePump = DrainUpdatesAsync(cancellation.Token);
        _realtimeClient.PositionReceived += OnPositionReceived;
        _realtimeClient.VehicleRemoved += OnVehicleRemoved;
        IsBusy = true;
        ErrorMessage = string.Empty;
        ConnectionStatus = "Connexion au réseau...";

        try
        {
            var vehicles = await _realtimeClient.SubscribeCityAsync(
                "douala",
                cancellation.Token);

            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                Vehicles.Clear();
                foreach (var vehicle in vehicles)
                    Vehicles.Add(vehicle);
            });

            ConnectionStatus = $"En direct · {Vehicles.Count} bus";
        }
        catch (Exception exception) when (
            exception is HttpRequestException or
            HubException or
            OperationCanceledException)
        {
            ErrorMessage = $"API indisponible : {exception.Message}";
            ConnectionStatus = "Hors ligne";
            _realtimeClient.PositionReceived -= OnPositionReceived;
            _realtimeClient.VehicleRemoved -= OnVehicleRemoved;
            _sessionCancellation = null;
            cancellation.Cancel();
            if (_updatePump is not null)
                await _updatePump;
            _updatePump = null;
            cancellation.Dispose();
        }
        finally
        {
            IsBusy = false;
        }
    }

    public async Task StopAsync()
    {
        var cancellation = _sessionCancellation;
        if (cancellation is null)
            return;

        _sessionCancellation = null;
        cancellation.Cancel();
        _realtimeClient.PositionReceived -= OnPositionReceived;
        _realtimeClient.VehicleRemoved -= OnVehicleRemoved;
        if (_updatePump is not null)
        {
            await _updatePump;
            _updatePump = null;
        }

        try
        {
            await _realtimeClient.UnsubscribeAsync("douala", CancellationToken.None);
        }
        finally
        {
            cancellation.Dispose();
            ConnectionStatus = "Déconnecté";
        }
    }

    private void OnPositionReceived(VehiclePosition position) =>
        _pendingPositions.AddOrUpdate(
            position.VehicleId,
            position,
            (_, current) => position.Sequence > current.Sequence ? position : current);

    private void OnVehicleRemoved(string vehicleId)
    {
        _pendingPositions.TryRemove(vehicleId, out _);
        _pendingRemovals[vehicleId] = 0;
    }

    private async Task DrainUpdatesAsync(CancellationToken cancellationToken)
    {
        try
        {
            using var timer = new PeriodicTimer(TimeSpan.FromMilliseconds(100));
            while (await timer.WaitForNextTickAsync(cancellationToken))
            {
                var positions = new List<VehiclePosition>();
                foreach (var vehicleId in _pendingPositions.Keys)
                {
                    if (_pendingPositions.TryRemove(vehicleId, out var position))
                        positions.Add(position);
                }

                var removed = new List<string>();
                foreach (var vehicleId in _pendingRemovals.Keys)
                {
                    if (_pendingRemovals.TryRemove(vehicleId, out _))
                        removed.Add(vehicleId);
                }

                if (positions.Count == 0 && removed.Count == 0)
                    continue;

                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    foreach (var position in positions)
                        ApplyPosition(position);
                    foreach (var vehicleId in removed)
                        RemoveVehicle(vehicleId);
                    ConnectionStatus = $"En direct · {Vehicles.Count} bus";
                });
            }
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
        }
    }

    private void ApplyPosition(VehiclePosition position)
    {
        var index = FindVehicleIndex(position.VehicleId);
        if (index < 0)
            Vehicles.Add(position);
        else if (position.Sequence > Vehicles[index].Sequence)
            Vehicles[index] = position;
    }

    private void RemoveVehicle(string vehicleId)
    {
        var index = FindVehicleIndex(vehicleId);
        if (index >= 0)
            Vehicles.RemoveAt(index);
    }

    private int FindVehicleIndex(string vehicleId)
    {
        for (var index = 0; index < Vehicles.Count; index++)
        {
            if (Vehicles[index].VehicleId == vehicleId)
                return index;
        }

        return -1;
    }
}
