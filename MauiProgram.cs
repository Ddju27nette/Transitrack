using transitrack.Services;
using TransiTrack.Services;
using TransiTrack.ViewModels;
using TransiTrack.Views.Driver;
using TransiTrack.Views.Home;
using TransiTrack.Views.Report;

namespace TransiTrack;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiMaps() // Activation impérative de la carte
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Services
        builder.Services.AddSingleton<RealtimeTrackingService>();

        // ViewModels
        builder.Services.AddSingleton<HomeMapViewModel>();
        builder.Services.AddTransient<DriverViewModel>();
        builder.Services.AddTransient<ReportViewModel>();

        // Views
        builder.Services.AddSingleton<HomePage>();
        builder.Services.AddTransient<DriverPage>();
        builder.Services.AddTransient<ReportPage>();

        return builder.Build();
    }
}