using Microsoft.AspNetCore.SignalR.Client;
using TransiTrack.Models;

namespace TransiTrack.Services;

public sealed class SignalRTransitRealtimeClient : ITransitRealtimeClient, IAsyncDisposable
{
    private readonly HubConnection _connection;
    private readonly IDisposable _positionSubscription;
    private readonly IDisposable _removedSubscription;
    private string? _subscribedCity;

    public event Action<VehiclePosition>? PositionReceived;
    public event Action<string>? VehicleRemoved;

    public SignalRTransitRealtimeClient(Uri hubUri)
    {
        _connection = new HubConnectionBuilder()
            .WithUrl(hubUri)
            .WithAutomaticReconnect()
            .Build();

        _positionSubscription = _connection.On<VehiclePosition>(
            "VehiclePositionUpdated",
            position => PositionReceived?.Invoke(position));
        _removedSubscription = _connection.On<string>(
            "VehicleRemoved",
            vehicleId => VehicleRemoved?.Invoke(vehicleId));
        _connection.Reconnected += async _ =>
        {
            if (_subscribedCity is not null)
                await _connection.InvokeAsync("SubscribeCity", _subscribedCity);
        };
    }

    public async Task<IReadOnlyList<VehiclePosition>> SubscribeCityAsync(
        string cityId,
        CancellationToken cancellationToken)
    {
        if (_connection.State == HubConnectionState.Disconnected)
            await _connection.StartAsync(cancellationToken);

        _subscribedCity = cityId;
        return await _connection.InvokeAsync<VehiclePosition[]>(
            "SubscribeCity",
            cityId,
            cancellationToken);
    }

    public async Task UnsubscribeAsync(
        string cityId,
        CancellationToken cancellationToken)
    {
        if (_connection.State == HubConnectionState.Connected)
        {
            await _connection.InvokeAsync(
                "UnsubscribeCity",
                cityId,
                cancellationToken);
        }

        if (string.Equals(_subscribedCity, cityId, StringComparison.OrdinalIgnoreCase))
            _subscribedCity = null;
    }

    public async Task PublishDriverPositionAsync(
        string sessionToken,
        DriverPositionRequest position,
        CancellationToken cancellationToken)
    {
        if (_connection.State == HubConnectionState.Disconnected)
            await _connection.StartAsync(cancellationToken);

        await _connection.InvokeAsync(
            "PublishPosition",
            sessionToken,
            position,
            cancellationToken);
    }

    public async ValueTask DisposeAsync()
    {
        _positionSubscription.Dispose();
        _removedSubscription.Dispose();
        await _connection.DisposeAsync();
    }
}
