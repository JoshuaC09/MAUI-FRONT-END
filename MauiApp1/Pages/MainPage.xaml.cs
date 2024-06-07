using Demo.MauiApiConsumer.Pages;
using MauiApp1.Models;
using MauiApp1.Pages;
using MauiApp1.Services;

namespace MauiApp1
{
    public partial class MainPage : ContentPage
    {
        private readonly HttpClientService _apiClient;

        public MainPage(HttpClientService apiClient)
        {
            InitializeComponent();
            _apiClient = apiClient;
        }

        private async void btnSettings_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SettingsPage());
        }

        private async void btnAdd_Clicked(object sender, EventArgs e)
        {
            var addEditProductPage = new AddEditProduct(_apiClient, null);
            await Navigation.PushModalAsync(addEditProductPage);
        }

        private async void btnShowProducts_Clicked(object sender, EventArgs e)
        {
            await LoadProducts();
        }

        private async void productListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var product = (Inventory)e.Item;
            var action = await DisplayActionSheet("Action", "Cancel", null, "Edit", "Delete");
            switch (action)
            {
                case "Edit":
                    var addEditProductPage = new AddEditProduct(_apiClient, product);
                    await Navigation.PushModalAsync(addEditProductPage);
                    break;
                case "Delete":
                    await _apiClient.DeleteProductAsync(product.InventoryID);
                    await LoadProducts();
                    break;
            }
        }

        private async Task LoadProducts()
        {
            var products = await _apiClient.GetAllAsync();
            productListView.ItemsSource = products; // Ensure products are displayed in ListView
        }

        private async void btnConnStringTextFile_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ConnStringTextFile(_apiClient)); // Pass the HttpClientService instance here
        }
    }
}
