using MauiApp1.Models;
using MauiApp1.Services;

namespace Demo.MauiApiConsumer.Pages
{
    public partial class AddEditProduct : ContentPage
    {
        private readonly HttpClientService _apiClient;
        private Inventory _inventory;

        public AddEditProduct(HttpClientService apiClient,Inventory inventory)
        {
            InitializeComponent();
            _apiClient = apiClient;
            _inventory = inventory;
            LoadProductDetails();
        }
        private void LoadProductDetails()
        {
            if(_inventory is not null)
            {
                txtProductCode.Text = _inventory.InventoryID.ToString();
                txtProductName.Text = _inventory.InventoryDescription;
                txtPrice.Text = _inventory.Quantity.ToString();
            }
        }

        private async void btnSave_Clicked(object sender, EventArgs e)
        {
            if (_inventory is null)
            {
                await _apiClient.AddProductAsync(new Inventory
                {
                    InventoryDescription = txtProductName.Text,
                    Quantity = Convert.ToInt32(txtPrice.Text)
                });
            }
            else
            {
                await _apiClient.UpdateProductAsync(_inventory.InventoryID, new Inventory
                {
                    InventoryID = _inventory.InventoryID,
                    InventoryDescription = txtProductName.Text,
                    Quantity = Convert.ToInt32(txtPrice.Text)
                });
            }

            await Navigation.PopModalAsync();
        }


        private async void btnCancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}