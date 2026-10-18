namespace transitrack.Views;

public partial class AddReportPage : ContentPage
{
    public AddReportPage()
    {
        InitializeComponent();
    }

    private async void OnConfirmReportClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Succès", "Signalement enregistré, merci !", "OK");
        await Shell.Current.GoToAsync(nameof(HistoryPage));
    }

    private async void OnGoToAuthClicked(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(AuthPage));
    }
}