namespace transitrack.Views;

public partial class FavoritesPage : ContentPage
{
    public FavoritesPage()
    {
        InitializeComponent();
    }

    private async void OnGoToAuthClicked(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(AuthPage));
    }
}