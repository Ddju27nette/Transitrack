using System.Collections.ObjectModel;
using System.Windows.Input;
using TransiTrack.Models;
using TransiTrack.Views.Stops;

namespace TransiTrack.ViewModels;

public class FavoritesViewModel : BaseViewModel
{
    public ObservableCollection<BusStop> Favorites { get; } = new();

    public ICommand LoadFavoritesCommand { get; }
    public ICommand SelectStopCommand { get; }

    public FavoritesViewModel()
    {
        LoadFavoritesCommand = new Command(async () => await LoadFavoritesAsync());
        SelectStopCommand = new Command<BusStop>(async (stop) =>
        {
            if (stop == null) return;
            var navParams = new Dictionary<string, object> { { "Stop", stop } };
            await Shell.Current.GoToAsync(nameof(StopPassagesPage), navParams);
        });

        _ = LoadFavoritesAsync();
    }

    public Task LoadFavoritesAsync()
    {
        IsBusy = true;
        try
        {
            Favorites.Clear();
            Favorites.Add(new BusStop { Id = "1", Name = "Carrefour Ndokoti", Description = "Axe Bassa - Rond Point", IsFavorite = true, Distance = "80 m" });
            Favorites.Add(new BusStop { Id = "2", Name = "Gare Bessengue", Description = "Boulevard de la Liberté", IsFavorite = true, Distance = "320 m" });
        }
        finally
        {
            IsBusy = false;
        }

        return Task.CompletedTask;
    }
}