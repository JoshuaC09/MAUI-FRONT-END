using MauiApp1.Services;
using Microsoft.Maui.Controls;
using System.Net.Http;

namespace MauiApp1
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var httpClient = new HttpClient();
            var httpClientService = new HttpClientService(httpClient);
            MainPage = new NavigationPage(new MainPage(httpClientService));
        }
    }
}
