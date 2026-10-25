using System.Net.Http.Json;

namespace TransiTrack.Services;

public sealed class TransitIncidentApiClient(
    HttpClient httpClient,
    Uri apiBaseUri) : ITransitIncidentApiClient
{
    public async Task SubmitAsync(
        TransitIncidentSubmission report,
        CancellationToken cancellationToken)
    {
        using var response = await httpClient.PostAsJsonAsync(
            new Uri(apiBaseUri, "api/v1/reports"),
            report,
            cancellationToken);
        response.EnsureSuccessStatusCode();
    }
}
