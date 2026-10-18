namespace transitrack.Views;

public partial class FavoritesPage : ContentPage
{
    public FavoritesPage()
    {
        InitializeComponent();
    }

    private async void OnGoToLoginClicked(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(AuthPage));
    }
}