using System.Net.Http.Json;
using TransiTrack.Models;

namespace TransiTrack.Services;

public sealed class TransitDriverApiClient(
    HttpClient httpClient,
    Uri apiBaseUri) : ITransitDriverApiClient
{
    public async Task<DriverTripSession> StartTripAsync(
        StartDriverTripRequest request,
        string driverApiKey,
        CancellationToken cancellationToken)
    {
        using var message = new HttpRequestMessage(
            HttpMethod.Post,
            new Uri(apiBaseUri, "api/v1/trips"))
        {
            Content = JsonContent.Create(request)
        };
        message.Headers.Add("X-Driver-Key", driverApiKey);

        using var response = await httpClient.SendAsync(message, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<DriverTripSession>(
                   cancellationToken: cancellationToken)
               ?? throw new InvalidDataException("L'API n'a pas renvoyé la session chauffeur.");
    }

    public async Task EndTripAsync(
        string tripId,
        string sessionToken,
        CancellationToken cancellationToken)
    {
        using var response = await httpClient.PostAsJsonAsync(
            new Uri(apiBaseUri, $"api/v1/trips/{Uri.EscapeDataString(tripId)}/end"),
            new { DriverSessionToken = sessionToken },
            cancellationToken);
        response.EnsureSuccessStatusCode();
    }
}
