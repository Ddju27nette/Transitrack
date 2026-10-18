namespace transitrack.Views;

public partial class AuthPage : ContentPage
{
    private bool _isRegisterMode = false;

    public AuthPage()
    {
        InitializeComponent();
    }

    private void OnTogglePasswordClicked(object sender, TappedEventArgs e)
    {
        EntryPassword.IsPassword = !EntryPassword.IsPassword;
        LblTogglePwd.Text = EntryPassword.IsPassword ? "Afficher" : "Masquer";
    }

    private void OnSwitchModeClicked(object sender, EventArgs e)
    {
        _isRegisterMode = !_isRegisterMode;

        if (_isRegisterMode)
        {
            LblAuthTitle.Text = "Créer un compte";
            FieldUsername.IsVisible = true;
            LblForgotPassword.IsVisible = false;
            BtnSubmit.Text = "Créer un compte";
            BtnSwitchMode.Text = "Déjà un compte ? Se connecter";
        }
        else
        {
            LblAuthTitle.Text = "Connexion";
            FieldUsername.IsVisible = false;
            LblForgotPassword.IsVisible = true;
            BtnSubmit.Text = "Se connecter";
            BtnSwitchMode.Text = "Créer un compte";
        }
    }

    private async void OnSubmitClicked(object sender, EventArgs e)
    {
        string action = _isRegisterMode ? "Compte créé avec succès !" : "Connexion réussie !";
        await DisplayAlert("TransiTrack", action, "OK");
        await Shell.Current.GoToAsync("//HomeStopsPage");
    }

    private async void OnContinueAsGuestClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//HomeStopsPage");
    }

    private async void OnForgotPasswordClicked(object sender, TappedEventArgs e)
    {
        await DisplayAlert("Réinitialisation", "Un lien de réinitialisation a été envoyé si l'adresse est reconnue.", "OK");
    }
}