using transitrack.Views;
using Microsoft.Maui.ApplicationModel;

namespace transitrack;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(StopDetailPage), typeof(StopDetailPage));
        Routing.RegisterRoute(nameof(AuthPage), typeof(AuthPage));
        Routing.RegisterRoute(nameof(HistoryPage), typeof(HistoryPage));
        Routing.RegisterRoute(nameof(FavoritesPage), typeof(FavoritesPage));

        // Naviguer vers la page d'authentification au démarrage de l'application
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            // Utilise le nom de route enregistré
            await Shell.Current.GoToAsync(nameof(AuthPage));
        });
    }
}