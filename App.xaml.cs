namespace transitrack;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = new Window(new AppShell());

        // Format smartphone iPhone / Android identique à Figma (390 x 844)
        window.Width = 410;
        window.Height = 860;
        window.MinimumWidth = 380;
        window.MinimumHeight = 700;

        return window;
    }
}