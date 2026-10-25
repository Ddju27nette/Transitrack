namespace transitrack.Views;

public partial class HomeStopsPage : ContentPage
{
    public HomeStopsPage()
    {
        InitializeComponent();

        // S'assurer que la barre de navigation et la tab bar sont visibles sur la page d'accueil
        Shell.SetNavBarIsVisible(this, true);
        Shell.SetTabBarIsVisible(this, true);
    }

    private async void OnStopCardClicked(object sender, TappedEventArgs e)
    {
        string stopName = e.Parameter as string ?? "Rond-point Deïdo";
        await Shell.Current.GoToAsync($"{nameof(StopDetailPage)}?StopName={Uri.EscapeDataString(stopName)}");
    }

    // Handler pour le bouton recherche (peut être connecté à votre VM)
    private void OnSearchClicked(object sender, EventArgs e)
    {
        var query = SearchEntry?.Text?.Trim();
        // TODO: appliquer le filtre sur la source de la CollectionView
        Console.WriteLine($"Recherche : {query}");
    }
}