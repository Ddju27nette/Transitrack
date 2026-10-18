using transitrack.Views;

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
    }
}