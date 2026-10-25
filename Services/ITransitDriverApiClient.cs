using TransiTrack.Models;

namespace TransiTrack.Services;

public interface ITransitDriverApiClient
{
    Task<DriverTripSession> StartTripAsync(
        StartDriverTripRequest request,
        string driverApiKey,
        CancellationToken cancellationToken);

    Task EndTripAsync(
        string tripId,
        string sessionToken,
        CancellationToken cancellationToken);
}
