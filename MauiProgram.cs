using transitrack;
using transitrack.Services;
using transitrack.ViewModels;
using transitrack.Views.Auth;
using transitrack.Views.Home;
using transitrack.Views.Report;
using transitrack.Views.Stops;
using TransiTrack.Services;
using TransiTrack.ViewModels;
using TransiTrack.Views.Auth;
using TransiTrack.Views.Favorites;
using TransiTrack.Views.History;
using TransiTrack.Views.Home;
using TransiTrack.Views.Profile;
using TransiTrack.Views.Report;
using TransiTrack.Views.Stops;

namespace TransiTrack;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Services
        builder.Services.AddSingleton<ITransitService, MockTransitService>();

        // ViewModels
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<RegisterViewModel>();
        builder.Services.AddTransient<HomeViewModel>();
        builder.Services.AddTransient<FavoritesViewModel>();
        builder.Services.AddTransient<StopPassagesViewModel>();
        builder.Services.AddTransient<HistoryViewModel>();
        builder.Services.AddTransient<ReportViewModel>();
        builder.Services.AddTransient<ProfileViewModel>();

        // Views
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<RegisterPage>();
        builder.Services.AddTransient<HomePage>();
        builder.Services.AddTransient<StopMapPage>();
        builder.Services.AddTransient<StopPassagesPage>();
        builder.Services.AddTransient<FavoritesPage>();
        builder.Services.AddTransient<HistoryPage>();
        builder.Services.AddTransient<ReportPage>();
        builder.Services.AddTransient<ProfilePage>();

        return builder.Build();
    }
}