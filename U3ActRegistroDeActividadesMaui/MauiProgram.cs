using Microsoft.Extensions.Logging;

namespace U3ActRegistroDeActividadesMaui
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

            HttpClient client = new() { BaseAddress = new Uri("https://u3eqpo1actapi.labsystec.net/api") };
            builder.Services.AddSingleton(client);
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
