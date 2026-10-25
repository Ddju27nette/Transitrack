using System.Collections.ObjectModel;
using TransiTrack.Models;
using TransiTrack.Services;

namespace TransiTrack.ViewModels;

public class HistoryViewModel : BaseViewModel
{
    private readonly ITransitService _transitService;
    public ObservableCollection<TripHistory> Trips { get; } = new();

    public HistoryViewModel(ITransitService transitService)
    {
        _transitService = transitService;
    }

    public async Task LoadHistoryAsync()
    {
        IsBusy = true;
        try
        {
            var items = await _transitService.GetHistoryAsync();
            Trips.Clear();
            foreach (var item in items)
                Trips.Add(item);
        }
        finally
        {
            IsBusy = false;
        }
    }
}