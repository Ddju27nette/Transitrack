using System.Collections.ObjectModel;
using System.Windows.Input;
using TransiTrack.Models;
using TransiTrack.Services;
using TransiTrack.Views.Stops;

namespace TransiTrack.ViewModels;

public class FavoritesViewModel : BaseViewModel
{
    private readonly ITransitService _transitService;
    public ObservableCollection<BusStop> Favorites { get; } = new();

    public ICommand LoadFavoritesCommand { get; }
    public ICommand SelectStopCommand { get; }

    public FavoritesViewModel(ITransitService transitService)
    {
        _transitService = transitService;
        LoadFavoritesCommand = new Command(async () => await LoadFavoritesAsync());
        SelectStopCommand = new Command<BusStop>(async (stop) =>
        {
            if (stop == null) return;
            var navParams = new Dictionary<string, object> { { "Stop", stop } };
            await Shell.Current.GoToAsync(nameof(StopPassagesPage), navParams);
        });
    }

    public async Task LoadFavoritesAsync()
    {
        IsBusy = true;
        try
        {
            var list = await _transitService.GetFavoriteStopsAsync();
            Favorites.Clear();
            foreach (var item in list)
                Favorites.Add(item);
        }
        finally
        {
            IsBusy = false;
        }
    }
}