using transitrack.Views.Auth;
using transitrack.Views.Stops;
using TransiTrack.Views.Auth;
using TransiTrack.Views.Stops;

namespace TransiTrack;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Enregistrement des routes de navigation empilées
        Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
        Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
        Routing.RegisterRoute(nameof(StopMapPage), typeof(StopMapPage));
        Routing.RegisterRoute(nameof(StopPassagesPage), typeof(StopPassagesPage));
    }
}