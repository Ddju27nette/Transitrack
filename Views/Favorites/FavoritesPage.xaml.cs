using TransiTrack.ViewModels;

namespace TransiTrack.Views.Favorites;

public partial class FavoritesPage : ContentPage
{
    private readonly FavoritesViewModel _vm;

    public FavoritesPage(FavoritesViewModel vm)
    {
        InitializeComponent();
        BindingContext = _vm = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _vm.LoadFavoritesAsync();
    }
}