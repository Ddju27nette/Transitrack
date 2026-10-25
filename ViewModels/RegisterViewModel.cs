using System.Windows.Input;

namespace TransiTrack.ViewModels;

public class RegisterViewModel : BaseViewModel
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public ICommand RegisterCommand { get; }
    public ICommand BackToLoginCommand { get; }

    public RegisterViewModel()
    {
        RegisterCommand = new Command(async () =>
        {
            if (string.IsNullOrWhiteSpace(FullName) || string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                await Shell.Current.DisplayAlertAsync("Validation", "Veuillez renseigner les champs requis.", "OK");
                return;
            }
            await Shell.Current.DisplayAlertAsync("Succès", "Compte créé avec succès !", "Continuer");
            await Shell.Current.GoToAsync("..");
        });

        BackToLoginCommand = new Command(async () => await Shell.Current.GoToAsync(".."));
    }
}