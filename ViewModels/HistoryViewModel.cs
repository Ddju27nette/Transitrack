using System.Collections.ObjectModel;
using TransiTrack.Models;

namespace TransiTrack.ViewModels;

public class HistoryViewModel : BaseViewModel
{
    public ObservableCollection<TripHistory> Trips { get; } = new();

    public HistoryViewModel()
    {
        _ = LoadHistoryAsync();
    }

    public Task LoadHistoryAsync()
    {
        IsBusy = true;
        try
        {
            Trips.Clear();
            Trips.Add(new TripHistory { Origin = "Ndokoti", Destination = "Bonamoussadi", DateFormatted = "Aujourd'hui, 07:45", LineUsed = "L10", Duration = "32 min" });
            Trips.Add(new TripHistory { Origin = "Gare Bessengue", Destination = "Bonanjo", DateFormatted = "Hier, 18:20", LineUsed = "L03", Duration = "18 min" });
        }
        finally
        {
            IsBusy = false;
        }

        return Task.CompletedTask;
    }
}