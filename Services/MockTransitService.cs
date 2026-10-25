using TransiTrack.Models;

namespace TransiTrack.Services;

public class MockTransitService : ITransitService
{
    private readonly List<BusStop> _stops = new()
    {
        new BusStop { Id = "1", Name = "Carrefour Ndokoti", Description = "Axe Bassa - Rond Point", Latitude = 4.0435, Longitude = 9.7422, IsFavorite = true, Distance = "80 m" },
        new BusStop { Id = "2", Name = "Gare Bessengue", Description = "Boulevard de la Liberté", Latitude = 4.0511, Longitude = 9.7085, IsFavorite = true, Distance = "320 m" },
        new BusStop { Id = "3", Name = "Campus 1 ENSPD", Description = "Entrée Principale", Latitude = 4.0620, Longitude = 9.7310, IsFavorite = false, Distance = "500 m" },
        new BusStop { Id = "4", Name = "Bonanjo Justice", Description = "Rue des Ministères", Latitude = 4.0450, Longitude = 9.6910, IsFavorite = false, Distance = "1.2 km" }
    };

    public Task<List<BusStop>> GetNearbyStopsAsync() => Task.FromResult(_stops);

    public Task<List<BusStop>> GetFavoriteStopsAsync() => Task.FromResult(_stops.Where(s => s.IsFavorite).ToList());

    public Task<List<BusPassage>> GetPassagesForStopAsync(string stopId)
    {
        var passages = new List<BusPassage>
        {
            new BusPassage { LineCode = "L10", Destination = "Bonamoussadi", MinutesRemaining = 3, CrowdLevel = "Modéré" },
            new BusPassage { LineCode = "L03", Destination = "Akwa Carrefour", MinutesRemaining = 9, CrowdLevel = "Faible" },
            new BusPassage { LineCode = "L17", Destination = "Ndogbong Universités", MinutesRemaining = 14, CrowdLevel = "Saturé" },
            new BusPassage { LineCode = "L10", Destination = "Bonamoussadi", MinutesRemaining = 24, CrowdLevel = "Faible" }
        };
        return Task.FromResult(passages);
    }

    public Task<List<TripHistory>> GetHistoryAsync()
    {
        var history = new List<TripHistory>
        {
            new TripHistory { Origin = "Ndokoti", Destination = "Bonamoussadi", DateFormatted = "Aujourd'hui, 07:45", LineUsed = "L10", Duration = "32 min" },
            new TripHistory { Origin = "Gare Bessengue", Destination = "Bonanjo", DateFormatted = "Hier, 18:20", LineUsed = "L03", Duration = "18 min" },
            new TripHistory { Origin = "Campus 1", Destination = "Ndogpassi", DateFormatted = "20 Oct., 14:10", LineUsed = "L17", Duration = "45 min" }
        };
        return Task.FromResult(history);
    }

    public Task<bool> SubmitReportAsync(IncidentReport report) => Task.FromResult(true);

    public Task ToggleFavoriteAsync(string stopId)
    {
        var item = _stops.FirstOrDefault(s => s.Id == stopId);
        if (item != null) item.IsFavorite = !item.IsFavorite;
        return Task.CompletedTask;
    }
}