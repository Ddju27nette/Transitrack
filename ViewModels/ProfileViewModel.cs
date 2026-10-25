using System.Windows.Input;
using TransiTrack.Views.Auth;
using TransiTrack.Views.Driver;

namespace TransiTrack.ViewModels;

public class ProfileViewModel : BaseViewModel
{
    public string UserName { get; set; } = "Dorcas Ambo'o";
    public string UserEmail { get; set; } = "dorcas@transitrack.cm";
    public string VersionInfo { get; set; } = "Version 1.0.0 (Build 2026)";

    public ICommand LogoutCommand { get; }
    public ICommand OpenDriverCommand { get; }

    public ProfileViewModel()
    {
        OpenDriverCommand = new Command(async () =>
            await Shell.Current.GoToAsync(nameof(DriverPage)));

        LogoutCommand = new Command(async () =>
        {
            bool confirm = await Shell.Current.DisplayAlertAsync("Déconnexion", "Voulez-vous vraiment vous déconnecter ?", "Oui", "Non");
            if (confirm)
            {
                await Shell.Current.GoToAsync(nameof(LoginPage));
            }
        });
    }
}