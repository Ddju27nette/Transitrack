using TransiTrack.Models;

namespace TransiTrack.Services;

public interface ITransitRealtimeClient
{
    event Action<VehiclePosition>? PositionReceived;
    event Action<string>? VehicleRemoved;

    Task<IReadOnlyList<VehiclePosition>> SubscribeCityAsync(
        string cityId,
        CancellationToken cancellationToken);

    Task UnsubscribeAsync(string cityId, CancellationToken cancellationToken);
    Task PublishDriverPositionAsync(
        string sessionToken,
        DriverPositionRequest position,
        CancellationToken cancellationToken);
}
