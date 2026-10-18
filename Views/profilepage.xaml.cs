namespace transitrack.Views;

public partial class ProfilePage : ContentPage
{
    public ProfilePage()
    {
        InitializeComponent();
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("Déconnexion", "Voulez-vous retourner à la page de connexion ?", "Oui", "Annuler");
        if (confirm)
        {
            await Shell.Current.GoToAsync(nameof(AuthPage));
        }
    }
}