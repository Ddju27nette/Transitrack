using System.Collections.ObjectModel;
using TransiTrack.Models;
using TransiTrack.Services;

namespace TransiTrack.ViewModels;

[QueryProperty(nameof(Stop), "Stop")]
public class StopPassagesViewModel : BaseViewModel
{
    private readonly ITransitService _transitService;
    private BusStop _stop = new();

    public BusStop Stop
    {
        get => _stop;
        set
        {
            SetProperty(ref _stop, value);
            LoadPassages(value.Id);
        }
    }

    public ObservableCollection<BusPassage> Passages { get; } = new();

    public StopPassagesViewModel(ITransitService transitService)
    {
        _transitService = transitService;
    }

    private async void LoadPassages(string stopId)
    {
        IsBusy = true;
        var list = await _transitService.GetPassagesForStopAsync(stopId);
        Passages.Clear();
        foreach (var p in list) Passages.Add(p);
        IsBusy = false;
    }
}