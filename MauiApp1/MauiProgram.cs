using Microsoft.Extensions.Logging;
using MauiApp1.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Extensions.Http;

namespace MauiApp1
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

            // Register HttpClient with Dependency Injection
            builder.Services.AddHttpClient<HttpClientService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7054/api/"); // Replace with your actual API base address
            });

            builder.Services.AddTransient<MainPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
