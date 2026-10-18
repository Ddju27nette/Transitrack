using Microsoft.Extensions.Logging;

namespace transitrack
{
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

#if WINDOWS
    		Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("BorderlessEntry", (handler, view) =>
    		{
                    handler.PlatformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
    		});
#endif

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
