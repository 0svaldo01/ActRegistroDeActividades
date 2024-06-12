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

            //HttpClient client = new HttpClient() { BaseAddress = new Uri("https://wichinsapi.websitos256.com/") };
            HttpClient client = new HttpClient() { BaseAddress = new Uri("https://doubledapi.labsystec.net/") };

            builder.Services.AddSingleton(client);
#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
