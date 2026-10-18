using transitrack.Views;

namespace transitrack;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(AuthPage), typeof(AuthPage));
    }
}