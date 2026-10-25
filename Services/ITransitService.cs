using TransiTrack.Models;

namespace TransiTrack.Services;

public interface ITransitService
{
    Task<List<BusStop>> GetNearbyStopsAsync();
    Task<List<BusStop>> GetFavoriteStopsAsync();
    Task<List<BusPassage>> GetPassagesForStopAsync(string stopId);
    Task<List<TripHistory>> GetHistoryAsync();
    Task<bool> SubmitReportAsync(IncidentReport report);
    Task ToggleFavoriteAsync(string stopId);
}