using System.Windows.Input;
using TransiTrack.Views.Driver;
using transitrack;

namespace TransiTrack.ViewModels;

public class ProfileViewModel : BaseViewModel
{
    public string UserName { get; set; } = "Dorcas Ambo'o";
    public string UserEmail { get; set; } = "dorcas@transitrack.cm";
    public string VersionInfo { get; set; } = "Version 1.0.0 (Build 2026)";

    public ICommand OpenDriverCommand { get; }
    public ICommand LogoutCommand { get; }
    public ICommand ResetPreferencesCommand { get; }

    public ProfileViewModel()
    {
        OpenDriverCommand = new Command(async () =>
        {
            if (Shell.Current != null)
                await Shell.Current.GoToAsync($"//{nameof(DriverPage)}");
        });

        LogoutCommand = new Command(async () =>
        {
            if (Shell.Current != null)
            {
                await Shell.Current.DisplayAlert("Information", "Session réinitialisée.", "OK");
                await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
            }
        });

        ResetPreferencesCommand = new Command(async () =>
        {
            if (Shell.Current != null)
            {
                bool confirm = await Shell.Current.DisplayAlert("Réinitialisation", "Voulez-vous réinitialiser vos paramètres et le cache local ?", "Oui", "Non");
                if (confirm)
                {
                    await Shell.Current.DisplayAlert("Succès", "Préférences réinitialisées.", "OK");
                    await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
                }
            }
        });
    }
}