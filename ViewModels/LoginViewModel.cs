using System.Windows.Input;
using TransiTrack.Views.Auth;

namespace TransiTrack.ViewModels;

public class LoginViewModel : BaseViewModel
{
    private string _email = string.Empty;
    public string Email { get => _email; set => SetProperty(ref _email, value); }

    private string _password = string.Empty;
    public string Password { get => _password; set => SetProperty(ref _password, value); }

    public ICommand LoginCommand { get; }
    public ICommand GoToRegisterCommand { get; }

    public LoginViewModel()
    {
        LoginCommand = new Command(async () =>
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                await Shell.Current.DisplayAlertAsync("Validation", "Veuillez renseigner tous les champs.", "OK");
                return;
            }
            await Shell.Current.GoToAsync("//HomePage");
        });

        GoToRegisterCommand = new Command(async () =>
        {
            await Shell.Current.GoToAsync(nameof(RegisterPage));
        });
    }
}