using MauiApp1.Services;
using MauiApp1.ViewModels;
using Microsoft.Extensions.Logging;

namespace MauiApp1;

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

        builder.Services.AddSingleton<IBotPlayer, BotPlayer>();
        builder.Services.AddSingleton<IGameHistoryService, GameHistoryService>();
        builder.Services.AddTransient<GameViewModel>();
        builder.Services.AddTransient<MainPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}