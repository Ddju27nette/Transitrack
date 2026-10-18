namespace transitrack.Views;

public partial class AuthPage : ContentPage
{
    private bool isRegister = false;

    public AuthPage()
    {
        InitializeComponent();
    }

    private void OnTogglePasswordClicked(object sender, TappedEventArgs e)
    {
        TxtPassword.IsPassword = !TxtPassword.IsPassword;
        LblTogglePwd.Text = TxtPassword.IsPassword ? "Afficher" : "Masquer";
    }

    private void OnSwitchModeClicked(object sender, EventArgs e)
    {
        isRegister = !isRegister;
        if (isRegister)
        {
            LblTitle.Text = "Créer un compte";
            FieldUsername.IsVisible = true;
            LblForgotPassword.IsVisible = false;
            BtnMainSubmit.Text = "Créer un compte";
            BtnSecondaryAction.Text = "Déjà un compte ? Se connecter";
        }
        else
        {
            LblTitle.Text = "Connexion";
            FieldUsername.IsVisible = false;
            LblForgotPassword.IsVisible = true;
            BtnMainSubmit.Text = "Se connecter";
            BtnSecondaryAction.Text = "Créer un compte";
        }
    }

    private async void OnSubmitClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//HomeStopsPage");
    }

    private async void OnContinueWithoutAccountClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//HomeStopsPage");
    }

    private async void OnForgotPasswordClicked(object sender, TappedEventArgs e)
    {
        await DisplayAlert("Mot de passe", "Instructions envoyées par email.", "OK");
    }
}