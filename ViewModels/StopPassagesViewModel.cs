using System.Collections.ObjectModel;
using TransiTrack.Models;

namespace TransiTrack.ViewModels;

[QueryProperty(nameof(Stop), "Stop")]
public class StopPassagesViewModel : BaseViewModel
{
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

    public StopPassagesViewModel()
    {
    }

    private void LoadPassages(string stopId)
    {
        IsBusy = true;
        Passages.Clear();
        Passages.Add(new BusPassage { LineCode = "L10", Destination = "Bonamoussadi", MinutesRemaining = 4, CrowdLevel = "Faible" });
        Passages.Add(new BusPassage { LineCode = "L03", Destination = "Akwa", MinutesRemaining = 11, CrowdLevel = "Modéré" });
        Passages.Add(new BusPassage { LineCode = "L17", Destination = "Ndogbong", MinutesRemaining = 19, CrowdLevel = "Saturé" });
        IsBusy = false;
    }
}