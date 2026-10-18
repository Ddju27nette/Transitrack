namespace transitrack.Views;

public partial class HomeStopsPage : ContentPage
{
    public HomeStopsPage()
    {
        InitializeComponent();
    }

    private async void OnStopCardClicked(object sender, TappedEventArgs e)
    {
        string stopName = e.Parameter as string ?? "Rond-point Deïdo";
        await Shell.Current.GoToAsync($"{nameof(StopDetailPage)}?StopName={Uri.EscapeDataString(stopName)}");
    }

    private async void OnGoToLoginClicked(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(AuthPage));
    }
}