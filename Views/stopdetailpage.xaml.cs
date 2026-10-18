namespace transitrack.Views;

[QueryProperty(nameof(StopName), "StopName")]
public partial class StopDetailPage : ContentPage
{
    private string _stopName = "Rond-point Deïdo";

    public string StopName
    {
        get => _stopName;
        set
        {
            _stopName = Uri.UnescapeDataString(value ?? "Rond-point Deïdo");
            LblStopTitle.Text = _stopName;
        }
    }

    public StopDetailPage()
    {
        InitializeComponent();
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private async void OnReportBusClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"///{nameof(AddReportPage)}");
    }

    private async void OnRefreshClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Actualisé", "Horaires rafraîchis en temps réel.", "OK");
    }

    private async void OnViewHistoryClicked(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(HistoryPage));
    }
}