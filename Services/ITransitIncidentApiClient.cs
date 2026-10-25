namespace TransiTrack.Services;

public sealed record TransitIncidentSubmission(
    string IncidentType,
    string LocationDescription,
    double Latitude,
    double Longitude,
    string Details);

public interface ITransitIncidentApiClient
{
    Task SubmitAsync(
        TransitIncidentSubmission report,
        CancellationToken cancellationToken);
}
